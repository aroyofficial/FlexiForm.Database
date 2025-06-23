using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Exception thrown when a script object is null or considered invalid.
    /// Typically indicates a programming or validation error before processing scripts.
    /// </summary>
    public class InvalidScriptException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidScriptException"/> class
        /// with a predefined message indicating that the script object is null.
        /// </summary>
        public InvalidScriptException()
            : base(ErrorCode.InvalidScript, "Script object cannot be null") { }
    }
}
