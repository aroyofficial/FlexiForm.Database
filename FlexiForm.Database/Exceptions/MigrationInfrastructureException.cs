using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the migration infrastructure setup fails.
    /// </summary>
    public class MigrationInfrastructureException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationInfrastructureException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MigrationInfrastructureException(Exception innerException)
            : base(ErrorCode.InfrastructureSetupFailure, "Failed to setup the migration infrastructure.", innerException)
        {
        }
    }
}
