using FlexiForm.Database.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a log entry for a migration task, capturing key execution details such as
    /// status, retry attempts, execution time, and memory usage.
    /// </summary>
    public class TaskLog
    {
        /// <summary>
        /// Gets or sets the unique identifier associated with the tracked task or operation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the absolute path associated with the migration task.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Gets or sets the current status of the migration task.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationTaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the number of retry attempts made for the migration task.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the execution profile containing details of the task's execution,
        /// such as duration and memory usage.
        /// </summary>
        public ExecutionProfile ExecutionProfile { get; set; }
    }
}
