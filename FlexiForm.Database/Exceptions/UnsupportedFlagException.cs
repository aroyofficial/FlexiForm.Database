using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an unsupported or unrecognized
    /// command-line flag is encountered during configuration or execution.
    /// </summary>
    public class UnsupportedFlagException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedFlagException"/> class
        /// with the specified flag name and a predefined error code.
        /// </summary>
        /// <param name="flag">The name of the unsupported flag.</param>
        public UnsupportedFlagException(string flag)
            : base(ErrorCode.UnsupportedFlag, $"Unsupported flag: {flag}") { }
    }
}
