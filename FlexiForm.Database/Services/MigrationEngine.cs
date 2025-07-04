using FlexiForm.Database.Configurations;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Extensions;
using FlexiForm.Database.Models;
using System.Data;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Provides a static orchestration engine for configuring, preparing, and executing database migration tasks.
    /// Supports command-line configuration, task batching, environment-specific filtering, and safe execution
    /// modes such as strict rollback. The class manages the full lifecycle of script-based migrations.
    /// </summary>
    public class MigrationEngine
    {
        /// <summary>
        /// Singleton instance of the <see cref="MigrationEngine"/> used to coordinate and execute migration tasks.
        /// </summary>
        private static MigrationEngine _instance;

        /// <summary>
        /// Synchronization object used to ensure thread-safe instantiation of the singleton <see cref="MigrationEngine"/>.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// Stores the task runner configuration, typically populated from command-line arguments or external configuration sources.
        /// </summary>
        private MigrationEngineConfiguration _configuration;

        /// <summary>
        /// Responsible for profiling migration tasks by tracking their execution time and memory usage.
        /// </summary>
        private readonly MigrationProfiler _profiler;

        /// <summary>
        /// Global queue containing batches of migration tasks, organized for sequential or prioritized execution.
        /// </summary>
        private readonly Queue<Queue<Migration>> _globalTaskQueue;

        /// <summary>
        /// Logger instance used to record configuration details, task progress, execution results, and performance data.
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// Database connector used to execute queries and commands during the migration process.
        /// </summary>
        private readonly DBConnector _dbConnector;

        /// <summary>
        /// Internal list storing the history of executed migration scripts retrieved from the database.
        /// Used for filtering out previously executed migrations during incremental migration mode.
        /// </summary>
        private List<MigrationLog> _migrationLogs;

        /// <summary>
        /// Occurs just before the migration process begins execution.
        /// Use this event to initialize any required resources or perform pre-processing logic.
        /// </summary>
        public EventHandler<EventArgs> OnStart;

        /// <summary>
        /// Occurs immediately after the migration process has completed execution.
        /// Use this event to dispose resources, log results, or perform post-processing logic.
        /// </summary>
        public EventHandler<EventArgs> OnComplete;

        /// <summary>
        /// Occurs when the migration process encounters an unhandled exception or fails to complete successfully.
        /// Use this event to perform error logging, cleanup, or alerting mechanisms upon failure.
        /// </summary>
        public EventHandler<EventArgs> OnFailed;

        /// <summary>
        /// Static constructor for the <see cref="MigrationEngine"/> class.
        /// Initializes static members such as the synchronization lock and singleton instance.
        /// </summary>
        static MigrationEngine()
        {
            _lock = new object();
            _instance = null;
        }

        /// <summary>
        /// Initializes the static <see cref="MigrationEngine"/> class by creating a default configuration instance.
        /// </summary>
        private MigrationEngine()
        {
            _configuration = new MigrationEngineConfiguration();
            _globalTaskQueue = new Queue<Queue<Migration>>();
            _profiler = MigrationProfiler.GetInstance();
            _logger = Logger.GetInstance();
            _dbConnector = DBConnector.GetInstance();
            _migrationLogs = new List<MigrationLog>();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="MigrationEngine"/> class.
        /// Ensures thread safety using a lock to prevent multiple instances from being created in a multithreaded environment.
        /// </summary>
        /// <returns>The single, shared instance of the <see cref="MigrationEngine"/> class.</returns>
        public static MigrationEngine GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new MigrationEngine();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Parses and applies command-line arguments to configure the task runner settings.
        /// Supports options such as retry count, timeout, migration type, execution mode,
        /// incremental execution, and target script types.
        /// </summary>
        public void Configure(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var flag = args[i];
                    var flagValueIndex = i + 1;

                    switch (flag.ToLower().Trim())
                    {
                        case "--r":
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

                        case "--t":
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

                        case "--type":

                            if (flagValueIndex >= args.Length)
                            {
                                throw new MissingFlagValueException(flag);
                            }

                            if (Enum.TryParse(args[flagValueIndex], ignoreCase: true, out MigrationType migration) &&
                                Enum.IsDefined(typeof(MigrationType), migration))
                            {
                                _configuration.Type = migration;
                                i++;
                            }
                            else
                            {
                                throw new InvalidFlagValueException(flag, args[flagValueIndex]);
                            }

                            break;

                        case "--incremental":
                            _configuration.RunMode = MigrationRunMode.Incremental;
                            break;

                        case "--full":
                            _configuration.RunMode = MigrationRunMode.Full;
                            break;

                        case "--tgt":
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
        }

        /// <summary>
        /// Starts the migration workflow by logging the configuration,
        /// preparing tasks, and executing them in batches.
        /// If the environment is set to development, a detailed execution report is also generated.
        /// </summary>
        public void Start()
        {
            try
            {
                OnStart?.Invoke(this, EventArgs.Empty);
                _logger.LogConfiguration(_configuration);
                SetupInfrastructure();

                if (_configuration.RunMode == MigrationRunMode.Incremental)
                {
                    GetMigrationHistory();
                }
                else if (_configuration.RunMode == MigrationRunMode.Full)
                {
                    ClearMigrationHistory();
                }

                PrepareTasks();
                ExecuteBatches();
                if (_configuration.Environment == HostEnvironment.Development)
                {
                    _logger.GenerateReport();
                }
                OnComplete?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _logger.GenerateReport();
                OnFailed?.Invoke(this, EventArgs.Empty);
                throw;
            }
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
        /// a group of <see cref="Migration"/> instances to be processed together.
        /// </returns>
        private Queue<Queue<Migration>> GetTasks(string folderPath, bool sortAscending = true)
        {
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath, "*.sql");
                var tasks = new List<Migration>()
                {
                    Capacity = files.Length
                };

                foreach (var file in files)
                {
                    try
                    {
                        var task = new Migration(new Script(file));
                        var exclude = _migrationLogs.FirstOrDefault(log =>
                        {
                            return _configuration.RunMode == MigrationRunMode.Incremental &&
                                log.Id == task.Script.Metadata.Id &&
                                log.MigrationType == task.Script.Metadata.MigrationType &&
                                log.Hash == task.Script.Metadata.Hash;
                        }) != null;

                        if (!exclude &&
                            task.Script.IsSafe &&
                            task.Script.HasCorrectSyntax)
                        {
                            tasks.Add(task);
                            _profiler.Register(task);
                        }
                        else if (!exclude)
                        {
                            _profiler.Register(task);

                            if (!task.Script.HasCorrectSyntax)
                            {
                                task.Status = MigrationTaskStatus.SkippedForWrongSyntax;
                            }

                            if (!task.Script.IsSafe)
                            {
                                task.Status = MigrationTaskStatus.SkippedForSafety;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                var batches = BatchProcessor<Migration>.Batchify(
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
        private void PrepareTasks()
        {
            var baseDirectory = $"{Environment.CurrentDirectory}/Scripts";

            if (_configuration.Type == MigrationType.Both ||
                _configuration.Type == MigrationType.Down)
            {
                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Proc))
                {
                    EnqueueBatches($"{baseDirectory}/StoredProcedures/Down", sortAscending: false);
                }

                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Index))
                {
                    EnqueueBatches($"{baseDirectory}/Indexes/Down", sortAscending: false);
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

            if (_configuration.Type == MigrationType.Both ||
                _configuration.Type == MigrationType.Up)
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

                if (_configuration.Environment == HostEnvironment.Development &&
                    _configuration.Target.HasFlag(ExecutionTarget.Index))
                {
                    EnqueueBatches($"{baseDirectory}/Indexes/Up");
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
        private void EnqueueBatches(string path, bool sortAscending = true)
        {
            var batches = GetTasks(path, sortAscending);

            foreach (var batch in batches)
            {
                _globalTaskQueue.Enqueue(batch);
            }
        }

        /// <summary>
        /// Executes all migration task batches from the global task queue. 
        /// Handles task execution, retries, error handling, and transactional control based on configuration.
        /// </summary>
        /// <remarks>
        /// If the runner mode is <c>Strict</c>, all batches are executed within a single global transaction.<br/>
        /// If the runner mode is <c>Relaxed</c> each batch is executed in its own transaction.<br/>
        /// Each task may retry execution based on the configured <c>MaxRetryCount</c>.<br/>
        /// Tasks update their status throughout execution (e.g., Picked, Executing, Retrying, Failed, Completed).
        /// </remarks>
        private void ExecuteBatches()
        {
            try
            {
                _dbConnector.OpenConnection();

                while (_globalTaskQueue.Count > 0)
                {
                    var batch = _globalTaskQueue.Dequeue();

                    foreach (var task in batch)
                    {
                        task.Status = MigrationTaskStatus.Picked;

                        while (task.Status != MigrationTaskStatus.Completed &&
                            task.RetryCount < _configuration.MaxRetryCount)
                        {
                            var transaction = _dbConnector.BeginTransaction();
                                    
                            try
                            {
                                task.Status = MigrationTaskStatus.Executing;
                                var reader = task.Script.OpenReader();
                                var sql = reader.ReadToEndAsync().GetAwaiter().GetResult();

                                if (!string.IsNullOrWhiteSpace(sql))
                                {
                                    var command = new DBCommand()
                                    {
                                        Sql = sql,
                                        Transaction = transaction,
                                        Timeout = _configuration.TaskTimeout
                                    };
                                    _dbConnector.Execute(command);
                                    task.Status = MigrationTaskStatus.Completed;
                                }

                                if (transaction.IsUsable())
                                {
                                    transaction.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (transaction.IsUsable())
                                {
                                    transaction.Rollback();
                                }

                                task.Status = task.RetryCount < _configuration.MaxRetryCount ?
                                    MigrationTaskStatus.Retrying : MigrationTaskStatus.Failed;
                                task.Error = ex.Message;
                                task.RetryCount++;
                            }
                            finally
                            {
                                task.Script.CloseReader();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _dbConnector.CloseConnection();
            }
        }

        /// <summary>
        /// Sets up the initial infrastructure required for migration by executing all SQL scripts 
        /// located in the <c>Scripts/Infrastructure</c> directory.
        /// </summary>
        private void SetupInfrastructure()
        {
            var directory = $"{Environment.CurrentDirectory}/Scripts/Infrastructure";

            if (Directory.Exists(directory))
            {
                var sqlFiles = Directory.EnumerateFiles(directory, "*.sql", SearchOption.TopDirectoryOnly);

                foreach (var file in sqlFiles)
                {
                    var transaction = _dbConnector.BeginTransaction();

                    try
                    {
                        using (var reader = new StreamReader(file))
                        {
                            var content = reader.ReadToEndAsync().GetAwaiter().GetResult();
                            var command = new DBCommand()
                            {
                                Sql = content,
                                Transaction = transaction,
                                Timeout = _configuration.TaskTimeout
                            };
                            _dbConnector.Execute(command);
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        throw new MigrationInfrastructureException(ex);
                    }
                }
            }
            else
            {
                var ex = new DirectoryNotFoundException($"{directory}");
                throw new MigrationInfrastructureException(ex);
            }
        }

        /// <summary>
        /// Retrieves the migration history from the database by executing the 
        /// stored procedure <c>usp_GetMigrationHistory</c>.
        /// The result is queried using a new database connection and mapped to a list of <see cref="MigrationLog"/>.
        /// </summary>
        private void GetMigrationHistory()
        {
            var command = new DBCommand()
            {
                Sql = "usp_GetMigrationHistory",
                CommandType = CommandType.StoredProcedure,
                UseNewConnection = true
            };

            _migrationLogs = _dbConnector.Query<MigrationLog>(command).ToList();
        }

        /// <summary>
        /// Clears the migration history from the database by executing the 
        /// stored procedure <c>usp_ClearMigrationHistory</c>.
        /// A new database connection is used for this operation.
        /// </summary>
        private void ClearMigrationHistory()
        {
            var command = new DBCommand()
            {
                Sql = "usp_ClearMigrationHistory",
                CommandType = CommandType.StoredProcedure,
                UseNewConnection = true
            };

            _dbConnector.Execute(command);
        }
    }
}
