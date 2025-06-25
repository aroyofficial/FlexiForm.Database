using FlexiForm.Database.Models;

namespace FlexiForm.Database.Events
{
    /// <summary>
    /// Provides data for an event that tracks the performance of a migration task or operation.
    /// </summary>
    public class PerformanceTrackingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the migration task whose performance is being tracked.
        /// </summary>
        public required MigrationTask Task { get; set; }
    }
}
