using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the audit metadata (e.g., Created At, Updated At)
    /// is malformed or not in the expected format within a script or migration file.
    /// </summary>
    public class MalformedAuditEntryException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MalformedAuditEntryException"/> class
        /// with a predefined error code and message indicating a malformed audit metadata line.
        /// </summary>
        public MalformedAuditEntryException()
            : base(ErrorCode.MalformedAudit, "Audit metadata line is malformed or has incorrect format.")
        {
        }
    }
}
