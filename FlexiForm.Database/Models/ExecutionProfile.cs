namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Captures performance metrics for a migration task, including execution duration and memory usage.
    /// </summary>
    public class ExecutionProfile
    {
        /// <summary>
        /// Gets or sets the total execution time in milliseconds.
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the memory usage in bytes during execution.
        /// </summary>
        public long MemoryUsage { get; set; }
    }
}
