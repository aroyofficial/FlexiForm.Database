using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a required value for a command-line flag is missing.
    /// </summary>
    public class MissingFlagValueException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingFlagValueException"/> class
        /// with a specified flag name and a predefined error code.
        /// </summary>
        /// <param name="flag">The name of the command-line flag that is missing a required value.</param>
        public MissingFlagValueException(string flag)
            : base(ErrorCode.MissingFlagValue, $"Missing value for flag: {flag}") { }
    }
}
