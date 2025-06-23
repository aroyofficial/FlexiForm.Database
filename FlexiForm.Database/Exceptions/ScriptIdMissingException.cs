using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the Script ID is missing or malformed
    /// in a migration or script header.
    /// </summary>
    public class ScriptIdMissingException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptIdMissingException"/> class
        /// with a predefined error code and message indicating a missing or invalid Script ID.
        /// </summary>
        public ScriptIdMissingException()
            : base(ErrorCode.ScriptIdMissing, "Script ID is missing or malformed")
        {
        }
    }
}
