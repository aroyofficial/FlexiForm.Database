using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a command-line flag is assigned an invalid value.
    /// </summary>
    public class InvalidFlagValueException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFlagValueException"/> class
        /// with the specified flag name, invalid value, and an optional additional note.
        /// </summary>
        /// <param name="flag">The name of the flag that received an invalid value.</param>
        /// <param name="value">The invalid value provided for the flag.</param>
        /// <param name="additionalNote">An optional note providing further context or guidance.</param>
        public InvalidFlagValueException(string flag, string value, string additionalNote = "")
            : base(ErrorCode.InvalidFlagValue, $"Invalid value '{value}' for flag: {flag}. {additionalNote}") { }
    }
}
