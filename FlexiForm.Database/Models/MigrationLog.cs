using FlexiForm.Database.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a detailed log entry for a migration task.
    /// This includes identifying metadata, execution status, retry attempts, profiling data,
    /// and a timestamp indicating when the task ran.
    /// </summary>
    public class MigrationLog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the migration task.
        /// Typically derived from the script's metadata or version control.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the display name or label associated with the migration task.
        /// Usually corresponds to the script filename or a human-readable description.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hash of the migration script content.
        /// This is used to verify the integrity and uniqueness of the script.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the absolute file path of the SQL script executed during the migration.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Gets or sets the final status of the migration task after execution.
        /// Indicates whether the task succeeded, failed, or was skipped.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationTaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the number of retry attempts made to execute the task due to prior failures.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the performance metrics related to task execution,
        /// such as duration and memory consumption.
        /// </summary>
        public ExecutionProfile ExecutionProfile { get; set; }

        /// <summary>
        /// Gets or sets the type of the migration, indicating whether it is a up or down migration.
        /// </summary>
        public MigrationType MigrationType { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp indicating when the task started execution.
        /// Useful for auditing and performance analysis.
        /// </summary>
        public DateTime? RanAt { get; set; }
    }
}
