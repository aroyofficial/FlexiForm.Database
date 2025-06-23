using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the script name is missing or malformed
    /// in the script header.
    /// </summary>
    public class ScriptNameMissingException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptNameMissingException"/> class
        /// with a predefined error code and a message indicating a missing or invalid script name.
        /// </summary>
        public ScriptNameMissingException()
            : base(ErrorCode.ScriptNameMissing, "Script Name is missing or malformed.")
        {
        }
    }
}
