using FlexiForm.Database.Configurations;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Models;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Singleton logger service responsible for capturing, displaying, and exporting logs
    /// related to migration task execution and configuration details.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="Logger"/> class.
        /// </summary>
        private static Logger _instance;

        /// <summary>
        /// A synchronization object used to ensure thread-safe access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// A list used to store log entries for all migration tasks during execution.
        /// </summary>
        private readonly List<MigrationLog> _logs;

        /// <summary>
        /// Stores the task runner configuration used during execution.
        /// </summary>
        private MigrationEngineConfiguration _configuration;

        /// <summary>
        /// Indicates whether the migration report has already been generated during the current execution cycle.
        /// Prevents duplicate report generation when <see cref="GenerateReport"/> is called multiple times.
        /// </summary>
        private bool _isReportGenerated = false;

        /// <summary>
        /// Initializes static members of the <see cref="Logger"/> class,
        /// including the synchronization lock and instance reference.
        /// </summary>
        static Logger()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private Logger()
        {
            _logs = new List<MigrationLog>();
            _configuration = new MigrationEngineConfiguration();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <returns>The single <see cref="Logger"/> instance.</returns>
        public static Logger GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Records a task summary log for a completed, failed, or skipped migration task.
        /// Also prints a formatted summary line to the console with status, duration, and memory usage.
        /// </summary>
        /// <param name="e">The <see cref="MigrationLog"/> containing the task execution summary.</param>
        public void Log(MigrationLog e)
        {
            _logs.Add(e);
            var footer = $"| Status: {e.Status} | Execution Time: {e.ExecutionProfile.Duration} ms | Memory Taken: {e.ExecutionProfile.MemoryUsage} bytes |";
            Console.WriteLine("+" + new string('-', footer.Length - 2) + "+");
            Console.WriteLine(footer);
            Console.WriteLine("+" + new string('-', footer.Length - 2) + "+");
            Console.WriteLine();
        }

        /// <summary>
        /// Outputs the contents and metadata of the currently executing migration task to the console.
        /// Useful for debugging and understanding script flow during runtime.
        /// </summary>
        /// <param name="migration">The <see cref="Migration"/> to display in the console.</param>
        public void LogMigration(Migration migration)
        {
            var header = $"| {migration.Script.Metadata.Id}: {migration.Script.Metadata.Name} |";
            var reader = migration.Script.OpenReader();
            var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
            migration.Script.CloseReader();
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
        /// Logs the provided task runner configuration to the console and saves it internally
        /// for use in generating detailed reports.
        /// </summary>
        /// <param name="configuration">
        /// The <see cref="MigrationEngineConfiguration"/> object containing execution environment,
        /// mode, type, timeout, retry, and migration scope settings.
        /// </param>
        public void LogConfiguration(MigrationEngineConfiguration configuration)
        {
            _configuration = configuration;
            var runMode = configuration.RunMode == MigrationRunMode.Incremental ? "Incremental" : "Full";
            Console.WriteLine();
            Console.WriteLine("Migration Configuration\n");
            Console.WriteLine($"Environment        : {configuration.Environment}");
            Console.WriteLine($"Migration Type     : {configuration.Type}");
            Console.WriteLine($"Run Mode           : {runMode}");
            Console.WriteLine($"Target Type(s)     : {configuration.Target}");
            Console.WriteLine($"Task Timeout (sec) : {configuration.TaskTimeout}");
            Console.WriteLine($"Max Retry Count    : {configuration.MaxRetryCount}");
            Console.WriteLine();
        }

        /// <summary>
        /// Generates a JSON report summarizing the task execution run.
        /// The report includes task configuration, total duration, completion/failure statistics,
        /// and detailed logs of each task. The report is saved to a timestamped file under the "logs" directory.
        /// </summary>
        public void GenerateReport()
        {
            if (!_isReportGenerated)
            {
                var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logsDir);
                var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ssZ");
                var logFilePath = Path.Combine(logsDir, $"{timestamp}.log");
                var total = _logs.Count;
                var completed = _logs.Count(t => t.Status == MigrationTaskStatus.Completed);
                var failed = _logs.Count(t => t.Status == MigrationTaskStatus.Failed);
                var skippedForSafety = _logs.Count(t => t.Status == MigrationTaskStatus.SkippedForSafety);
                var skippedForWrongSyntax = _logs.Count(t => t.Status == MigrationTaskStatus.SkippedForWrongSyntax);

                Console.WriteLine("Task Summary");
                Console.WriteLine();
                Console.WriteLine($"Total tasks     : {total, 3}");
                Console.WriteLine($"Completed tasks : {completed, 3}");
                Console.WriteLine($"Failed tasks    : {failed,3}");
                Console.WriteLine($"Skipped tasks   : {(skippedForSafety + skippedForWrongSyntax), 3}");
                Console.WriteLine();

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
                        Total = total,
                        Completed = completed,
                        Failed = failed,
                        SkippedForSafety = skippedForSafety,
                        SkippedForWrongSyntax = skippedForWrongSyntax
                    },
                    Tasks = _logs
                });

                Console.WriteLine($"Migration report is now available at {logFilePath}");
                _isReportGenerated = true;
            }
        }

        /// <summary>
        /// Writes a custom plain text message to the console log.
        /// </summary>
        /// <param name="message">The message to display in the console output.</param>
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
