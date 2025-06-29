using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents metadata information for a database migration script.
    /// </summary>
    public class ScriptMetadata
    {
        /// <summary>
        /// Backing field for storing the audit history.
        /// </summary>
        private List<Audit> _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptMetadata"/> class with default values.
        /// </summary>
        public ScriptMetadata()
        {
            _history = new List<Audit>();
        }

        /// <summary>
        /// Gets or sets the unique identifier of the script.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name or title of the script.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the audit history associated with the script.
        /// The history is maintained in ascending order based on <c>TimeStamp</c>.
        /// </summary>
        public IEnumerable<Audit> History => _history;

        /// <summary>
        /// Gets or sets the type of the script (e.g., schema, data, etc.).
        /// </summary>
        public ScriptType Type { get; set; }

        /// <summary>
        /// Gets or sets the type of migration the script performs (e.g., baseline, incremental, etc.).
        /// </summary>
        public MigrationType MigrationType { get; set; }

        /// <summary>
        /// Gets or sets the hash value representing the unique fingerprint of the migration script.
        /// Typically used to verify script integrity or detect changes.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Adds a new <see cref="Audit"/> entry to the history and ensures the list is sorted by <c>TimeStamp</c> in ascending order.
        /// </summary>
        /// <param name="audit">The audit entry to be added.</param>
        public void AddAudit(Audit audit)
        {
            _history.Add(audit);
            _history = _history.OrderBy(a => a.TimeStamp).ToList();
        }
    }
}
