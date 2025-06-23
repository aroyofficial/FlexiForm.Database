namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a collection of detected threat insights within a SQL script.
    /// </summary>
    public class Threat
    {
        /// <summary>
        /// Gets or sets the collection of threat information entries identified during scanning.
        /// Each entry provides details such as the threat type and the line where it occurred.
        /// </summary>
        public IEnumerable<ThreatInfo> Insights { get; set; }
    }
}
