using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents the abstract base class for all custom exceptions in the FlexiForm database layer.
    /// Contains a standardized <see cref="ErrorCode"/> and message to support consistent error handling.
    /// </summary>
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// Gets or sets the application-specific error code associated with this exception.
        /// </summary>
        protected ErrorCode Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class with the specified error code and message.
        /// </summary>
        /// <param name="code">The error code that identifies the nature of the exception.</param>
        /// <param name="message">The human-readable message describing the exception.</param>
        protected BaseException(ErrorCode code, string message)
            : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// Gets the formatted exception message that includes the associated <see cref="Code"/>.
        /// </summary>
        /// <value>
        /// A string in the format <c>[ERROR{Code}] base.Message</c>, where <c>Code</c>
        /// represents the application-specific error code and <c>base.Message</c>
        /// is the original exception message.
        /// </value>
        public new string Message => $"[ERROR{Code}] {base.Message}";

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class with a specified error code,
        /// error message, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="code">The <see cref="ErrorCode"/> that identifies the type of error.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        protected BaseException(ErrorCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }
    }
}
