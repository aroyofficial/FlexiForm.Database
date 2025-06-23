using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the migration type is missing
    /// or invalid in the script metadata.
    /// </summary>
    public class MigrationTypeMissingException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationTypeMissingException"/> class
        /// with a predefined error code and message indicating the absence or invalidity of the migration type.
        /// </summary>
        public MigrationTypeMissingException()
            : base(ErrorCode.MigrationTypeMissing, "Migration Type is missing or invalid.")
        {
        }
    }
}
