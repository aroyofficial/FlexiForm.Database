using FlexiForm.Database.Configurations;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Models;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Singleton logger responsible for capturing, displaying, and exporting logs related to migration task execution.
    /// </summary>
    public class TaskLogger
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="TaskLogger"/> class.
        /// </summary>
        private static TaskLogger _instance;

        /// <summary>
        /// A synchronization object used to ensure thread-safe access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// A list used to store log entries for all migration tasks.
        /// </summary>
        private readonly List<TaskLog> _logs;

        /// <summary>
        /// Stores the configuration settings used during task execution and logging.
        /// </summary>
        private TaskRunnerConfiguration _configuration;

        /// <summary>
        /// Initializes static members of the <see cref="TaskLogger"/> class,
        /// setting up the synchronization lock and instance reference.
        /// </summary>
        static TaskLogger()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLogger"/> class.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private TaskLogger()
        {
            _logs = new List<TaskLog>();
            _configuration = new TaskRunnerConfiguration();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="TaskLogger"/> class.
        /// </summary>
        /// <returns>The single <see cref="TaskLogger"/> instance.</returns>
        public static TaskLogger GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TaskLogger();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Records a summary log entry for a completed or attempted migration task.
        /// </summary>
        /// <param name="e">The <see cref="TaskLog"/> containing the task execution summary.</param>
        public void Log(TaskLog e)
        {
            _logs.Add(e);
            var footer = $"| Status: {e.Status.ToString()} | Execution Time: {e.ExecutionProfile.Duration} ms | Memory Taken: {e.ExecutionProfile.MemoryUsage} bytes |";
            Console.WriteLine("+" + new string('-', footer.Length - 2) + "+");
            Console.WriteLine(footer);
            Console.WriteLine("+" + new string('-', footer.Length - 2) + "+");
            Console.WriteLine();
        }

        /// <summary>
        /// Outputs the details of the currently executing migration task, including the SQL script content,
        /// to the console in a formatted and readable layout.
        /// </summary>
        /// <param name="task">The <see cref="MigrationTask"/> to display in the console.</param>
        public void LogCurrentExecution(MigrationTask task)
        {
            var header = $"| {task.Script.Metadata.Id}: {task.Script.Metadata.Name} |";
            var reader = task.OpenReader();
            var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
            task.CloseReader();
            Console.WriteLine();
            Console.WriteLine("+" + new string('-', header.Length - 2) + "+");
            Console.WriteLine(header);
            Console.WriteLine("+" + new string('-', header.Length - 2) + "+");
            Console.WriteLine();
            Console.WriteLine(body);
            if (!body.EndsWith("\n"))
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Logs the specified task runner configuration to the console in a formatted manner,
        /// and stores it for future reference.
        /// </summary>
        /// <param name="configuration">
        /// The <see cref="TaskRunnerConfiguration"/> object containing environment, mode,
        /// migration type, target types, and retry configuration details.
        /// </param>
        public void LogConfiguration(TaskRunnerConfiguration configuration)
        {
            _configuration = configuration;
            Console.WriteLine();
            Console.WriteLine("Migration Configuration\n");
            Console.WriteLine($"Environment           : {configuration.Environment}");
            Console.WriteLine($"Migration Type        : {configuration.Migration}");
            Console.WriteLine($"Execution Mode        : {configuration.Mode}");
            Console.WriteLine($"Execute Incrementally : {configuration.ExecuteIncrementally}");
            Console.WriteLine($"Target Types          : {configuration.Target}");
            Console.WriteLine($"Task Timeout (sec)    : {configuration.TaskTimeout}");
            Console.WriteLine($"Max Retry Count       : {configuration.MaxRetryCount}");
            Console.WriteLine();
        }

        /// <summary>
        /// Generates a JSON-formatted report summarizing the execution of migration tasks.
        /// The report includes:
        /// <list type="bullet">
        /// <item><description>Total execution time in milliseconds.</description></item>
        /// <item><description>Count of tasks completed, failed, or skipped.</description></item>
        /// <item><description>Detailed logs for all tasks.</description></item>
        /// </list>
        /// The report is saved in the application's <c>logs</c> directory with a timestamped filename.
        /// </summary>
        public void GenerateReport()
        {
            var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDir);
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ssZ");
            var logFilePath = Path.Combine(logsDir, $"{timestamp}.log");
            using var fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var streamWriter = new StreamWriter(fileStream);
            using var jsonWriter = new JsonTextWriter(streamWriter)
            {
                Formatting = Formatting.Indented
            };

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, new
            {
                Configuration = _configuration,
                Summary = new
                {
                    TotalTimeTakenInMs = _logs.Sum(t => t.ExecutionProfile.Duration),
                    Completed = _logs.Count(t => t.Status == MigrationTaskStatus.Completed),
                    Failed = _logs.Count(t => t.Status == MigrationTaskStatus.Failed),
                    SkippedForSafety = _logs.Count(t => t.Status == MigrationTaskStatus.SkippedForSafety),
                    SkippedForWrongSyntax = _logs.Count(t => t.Status == MigrationTaskStatus.SkippedForWrongSyntax)
                },
                Tasks = _logs
            });

            Console.WriteLine($"Migration report is now available at {logFilePath}");
        }

        /// <summary>
        /// Writes a simple custom message to the console.
        /// </summary>
        /// <param name="message">The text message to display in the console output.</param>
        public void LogMessage(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}
