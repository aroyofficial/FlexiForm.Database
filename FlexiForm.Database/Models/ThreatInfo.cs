using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents information about a detected threat within a SQL script,
    /// including the line number where it occurred and the type of threat identified.
    /// </summary>
    public class ThreatInfo
    {
        /// <summary>
        /// The line number in the SQL script where the threat pattern was detected.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// The type of threat detected, categorized by known SQL threat patterns.
        /// </summary>
        public ThreatType Type { get; set; }
    }
}
