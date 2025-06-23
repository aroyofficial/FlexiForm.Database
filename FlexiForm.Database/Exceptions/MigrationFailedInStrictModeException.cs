using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a migration fails while running in strict mode,
    /// resulting in a rollback of all previous changes.
    /// </summary>
    public class MigrationFailedInStrictModeException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationFailedInStrictModeException"/> class
        /// with a predefined error code and message indicating a strict mode failure and rollback.
        /// </summary>
        public MigrationFailedInStrictModeException()
            : base(ErrorCode.MigrationFailed, "Migration failed in strict mode. Rolled back everything.")
        {
        }
    }
}
