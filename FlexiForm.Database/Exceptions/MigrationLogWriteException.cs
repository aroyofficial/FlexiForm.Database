using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the application fails to write to the migration log file.
    /// </summary>
    public class MigrationLogWriteException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationLogWriteException"/> class with the specified inner exception.
        /// </summary>
        /// <param name="ex">The exception that caused the failure to write to the migration log file.</param>
        public MigrationLogWriteException(Exception ex)
            : base(ErrorCode.MigrationLogWriteFailure, "Failed to append migration task to log file.", ex)
        {
        }
    }
}
