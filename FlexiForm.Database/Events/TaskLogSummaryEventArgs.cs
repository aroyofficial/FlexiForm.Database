using FlexiForm.Database.Models;

namespace FlexiForm.Database.Events
{
    /// <summary>
    /// Provides data for a logging event related to a migration task,
    /// including status, retry count, and execution profile information.
    /// </summary>
    public class TaskLogSummaryEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the log entry that summarizes the execution details of the migration task.
        /// </summary>
        public required TaskLog TaskLog { get; set; }
    }
}
