using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the 'Created At' metadata is missing or malformed
    /// in a migration or script file.
    /// </summary>
    public class CreatedAtMissingException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatedAtMissingException"/> class
        /// with a predefined error code and message indicating the absence or invalidity of 'Created At' metadata.
        /// </summary>
        public CreatedAtMissingException()
            : base(ErrorCode.CreatedAtMissing, "Created At metadata is missing or malformed.")
        {
        }
    }
}
