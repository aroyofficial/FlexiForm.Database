namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents metadata related to a database operation, capturing the author and the time of change.
    /// Useful for auditing created or modified entries.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// The name or identifier of the user who performed the operation.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The UTC timestamp when the operation was performed.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
