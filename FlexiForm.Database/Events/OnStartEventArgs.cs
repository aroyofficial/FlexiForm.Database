using FlexiForm.Database.Models;

namespace FlexiForm.Database.Events
{
    /// <summary>
    /// Represents the event data used when a migration task starts execution.
    /// Contains context information about the task being initiated.
    /// </summary>
    public class OnStartEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="Migration"/> instance that is starting execution.
        /// </summary>
        public required Migration Task { get; set; }
    }
}
