using FlexiForm.Database.Models;

namespace FlexiForm.Database.Events
{
    /// <summary>
    /// Represents the data associated with the completion of a migration task,
    /// including execution status, retry information, and performance metrics.
    /// </summary>
    public class OnCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="Log"/> that contains a summary of the completed migration task,
        /// including details such as status, retries, and execution duration.
        /// </summary>
        public required MigrationLog Log { get; set; }
    }
}
