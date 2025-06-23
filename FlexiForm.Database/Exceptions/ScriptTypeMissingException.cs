using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the script type is missing or invalid
    /// in the script metadata or header.
    /// </summary>
    public class ScriptTypeMissingException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptTypeMissingException"/> class
        /// with a predefined error code and a message indicating a missing or invalid script type.
        /// </summary>
        public ScriptTypeMissingException()
            : base(ErrorCode.ScriptTypeMissing, "Script Type is missing or invalid.")
        {
        }
    }
}
