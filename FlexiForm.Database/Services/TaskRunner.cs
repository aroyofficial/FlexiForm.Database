using FlexiForm.Database.Configurations;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Logging;
using FlexiForm.Database.Models;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Provides a static orchestration engine for configuring, preparing, and executing database migration tasks.
    /// Supports command-line configuration, task batching, environment-specific filtering, and safe execution
    /// modes such as strict rollback. The class manages the full lifecycle of script-based migrations.
    /// </summary>
    public static class TaskRunner
    {
        private static TaskRunnerConfiguration _configuration;
        private static TaskExecutor _executor;
        private static Queue<Queue<MigrationTask>> _globalTaskQueue;

        /// <summary>
        /// Initializes the static <see cref="TaskRunner"/> class by creating a default configuration instance.
        /// </summary>
        static TaskRunner()
        {
            _configuration = new TaskRunnerConfiguration();
            _executor = TaskExecutor.GetInstance();
            _globalTaskQueue = new Queue<Queue<MigrationTask>>();
        }

        /// <summary>
        /// Parses and applies command-line arguments to configure the task runner settings.
        /// Supports options such as retry count, timeout, migration type, execution mode,
        /// incremental execution, and target script types.
        /// </summary>
        public static void Configure(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var flag = args[i];
                    var flagValueIndex = i + 1;

                    switch (flag.ToLower().Trim())
                    {
                        case "--maxretry":

                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (int.TryParse(args[flagValueIndex], out int maxRetryCount))
                            {
                                if (maxRetryCount > 0 &&
                                    maxRetryCount <= 5)
                                {
                                    _configuration.MaxRetryCount = maxRetryCount;
                                    i++;
                                }
                                else
                                {
                                    throw new InvalidFlagValueException(flag, args[flagValueIndex], "Value should be between 0 to 5.");
                                }
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex]);
                            }

                            break;

                        case "--timeout":

                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (int.TryParse(args[flagValueIndex], out int timeout))
                            {
                                if (timeout > 0 &&
                                    timeout <= 10000)
                                {
                                    _configuration.TaskTimeout = timeout;
                                    i++;
                                }
                                else
                                {
                                    throw new InvalidFlagValueException(flag, args[flagValueIndex], "Value should be between 1 to 10000.");
                                }
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex]);
                            }

                            break;

                        case "--migration":

                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (Enum.TryParse(args[flagValueIndex], ignoreCase: true, out MigrationType migration) &&
                                Enum.IsDefined(typeof(MigrationType), migration))
                            {
                                _configuration.Migration = migration;
                                i++;
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex]);
                            }

                            break;

                        case "--mode":

                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (Enum.TryParse(args[flagValueIndex], ignoreCase: true, out TaskRunnerMode mode) &&
                                Enum.IsDefined(typeof(TaskRunnerMode), mode))
                            {
                                _configuration.Mode = mode;
                                i++;
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex]);
                            }

                            break;

                        case "--incremental":

                            _configuration.ExecuteIncrementally = true;
                            break;

                        case "--targets":
                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (Enum.TryParse<ExecutionTarget>(args[flagValueIndex], ignoreCase: true, out var parsedTarget))
                            {
                                _configuration.Target = parsedTarget;
                                i++;
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex], "Allowed values: None, Proc, Schema, Constraint, All or combinations like Proc,Schema.");
                            }
                            break;

                        default:
                            throw new UnsupportedFlagException(flag);

                    }
                }
            }

            ShowConfiguration(_configuration);
        }

        /// <summary>
        /// Executes the main task runner workflow.
        /// This method serves as the entry point for executing configured migration or script tasks.
        /// </summary>
        public static void Run()
        {
            _executor.SetConfiguration(_configuration);
            PrepareTasks();
            _executor.ExecuteBatches(_globalTaskQueue);
        }

        /// <summary>
        /// Retrieves and batches migration tasks from a given folder containing SQL script files.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing .sql migration scripts.</param>
        /// <param name="sortAscending">
        /// Indicates whether script files should be sorted in ascending order based on file name.
        /// If <c>false</c>, files will be sorted in descending order. Default is <c>true</c>.
        /// </param>
        /// <returns>
        /// A <see cref="Queue{T}"/> of <see cref="Queue{T}"/> batches, where each inner queue contains
        /// a group of <see cref="MigrationTask"/> instances to be processed together.
        /// </returns>
        private static Queue<Queue<MigrationTask>> GetTasks(string folderPath, bool sortAscending = true)
        {
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, "*.sql");
                var tasks = new List<MigrationTask>()
                {
                    Capacity = files.Length
                };

                foreach (var file in files)
                {
                    try
                    {
                        var task = new MigrationTask(new Script(file));
                        if (task.Script.IsSafe &&
                            task.Script.HasCorrectSyntax)
                        {
                            tasks.Add(task);
                        }
                        else
                        {
                            if (!task.Script.HasCorrectSyntax)
                            {
                                task.Status = MigrationTaskStatus.SkippedForWrongSyntax;
                            }

                            if (!task.Script.IsSafe)
                            {
                                task.Status = MigrationTaskStatus.SkippedForSafety;
                            }

                            Logger.Log(task);
                        }
                    }
                    catch (Exception)
                    {
                        if (_configuration.Mode == TaskRunnerMode.Strict)
                        {
                            throw;
                        }
                    }
                }

                var batches = BatchProcessor<MigrationTask>.Batchify(
                    tasks,
                    _configuration.BatchCount,
                    fileNameSelector: task => task.Script.Metadata.Name,
                    sortAscending: sortAscending);

                return batches;
            }
            else
            {
                throw new MigrationFolderNotFoundException(folderPath);
            }
        }

        /// <summary>
        /// Prepares and enqueues migration task batches based on the configured migration type,
        /// target execution items, and environment. This method organizes tasks for up, down,
        /// or both migration directions and applies environment-specific filtering.
        /// </summary>
        /// <remarks>
        /// - Down migrations enqueue tasks in descending order (e.g., for rollback).<br/>
        /// - Up migrations enqueue tasks in ascending order (e.g., for schema/applying changes).<br/>
        /// - In development environments, only selected targets are considered for down/up.
        /// </remarks>
        private static void PrepareTasks()
        {
            var baseDirectory = $"{Environment.CurrentDirectory}/Scripts";

            if (_configuration.Migration == MigrationType.Both ||
                _configuration.Migration == MigrationType.Down)
            {
                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Proc))
                {
                    EnqueueBatches($"{baseDirectory}/StoredProcedures/Down", sortAscending: false);
                }

                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Alter))
                {
                    EnqueueBatches($"{baseDirectory}/Alters/Down", sortAscending: false);
                }

                if (_configuration.Target.HasFlag(ExecutionTarget.Schema))
                {
                    EnqueueBatches($"{baseDirectory}/Schemas/Down", sortAscending: false);
                }
            }

            if (_configuration.Migration == MigrationType.Both ||
                _configuration.Migration == MigrationType.Up)
            {
                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Schema))
                {
                    EnqueueBatches($"{baseDirectory}/Schemas/Up");
                }

                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Alter))
                {
                    EnqueueBatches($"{baseDirectory}/Alters/Up");
                }

                if (_configuration.Target.HasFlag(ExecutionTarget.Proc))
                {
                    EnqueueBatches($"{baseDirectory}/StoredProcedures/Up");
                }
            }
        }

        /// <summary>
        /// Retrieves and enqueues batches of migration tasks from the specified directory path
        /// into the global task queue.
        /// </summary>
        /// <param name="path">The directory path from which to load migration tasks.</param>
        /// <param name="sortAscending">
        /// Indicates whether the task files should be sorted in ascending order by filename.
        /// Set to <c>false</c> to sort in descending order. Default is <c>true</c>.
        /// </param>
        private static void EnqueueBatches(string path, bool sortAscending = true)
        {
            var batches = GetTasks(path, sortAscending);

            foreach (var batch in batches)
            {
                _globalTaskQueue.Enqueue(batch);
            }
        }

        /// <summary>
        /// Displays the current task runner configuration settings to the console in a readable format.
        /// </summary>
        /// <param name="configuration">The configuration instance to display.</param>
        private static void ShowConfiguration(TaskRunnerConfiguration configuration)
        {
            Console.WriteLine();
            Console.WriteLine("Task Runner Configuration\n");
            Console.WriteLine($"Environment           : {configuration.Environment}");
            Console.WriteLine($"Migration Type        : {configuration.Migration}");
            Console.WriteLine($"Execution Mode        : {configuration.Mode}");
            Console.WriteLine($"Execute Incrementally : {configuration.ExecuteIncrementally}");
            Console.WriteLine($"Target Types          : {configuration.Target}");
            Console.WriteLine($"Task Timeout (sec)    : {configuration.TaskTimeout}");
            Console.WriteLine($"Max Retry Count       : {configuration.MaxRetryCount}");
            Console.WriteLine();
        }
    }
}
