using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Extensions;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a database migration script with metadata.
    /// </summary>
    public class Script
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class using the specified absolute file path.
        /// </summary>
        /// <param name="absolutePath">The full file system path to the SQL script.</param>
        public Script(string absolutePath)
        {
            AbsolutePath = absolutePath;
            Validate();
            Metadata = this.ParseHeader();
            ParseResult = this.Parse();
            Threat = this.Scan();
        }

        /// <summary>
        /// Gets the metadata extracted from the script header.
        /// </summary>
        public ScriptMetadata Metadata { get; }

        /// <summary>
        /// Gets or sets the absolute file path where the script is stored.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Gets a value indicating whether the script is considered safe.
        /// </summary>
        public bool IsSafe => Threat == null;

        /// <summary>
        /// Gets the threat identified during the scan, if any.
        /// </summary>
        public Threat? Threat { get; }

        /// <summary>
        /// Gets a value indicating whether the script has correct SQL syntax.
        /// </summary>
        public bool HasCorrectSyntax => ParseResult == null || ParseResult.Success;

        /// <summary>
        /// Gets the result of parsing the script, including any syntax errors.
        /// </summary>
        public ParseResult ParseResult { get; }

        /// <summary>
        /// Validates the <see cref="AbsolutePath"/> property to ensure it points to an existing file.
        /// </summary>
        private void Validate()
        {
            if (File.Exists(this.AbsolutePath))
            {
                var attributes = File.GetAttributes(this.AbsolutePath);
                if (attributes.HasFlag(FileAttributes.Directory))
                {
                    throw new ScriptPathIsDirectoryException(this.AbsolutePath);
                }
            }
            else
            {
                throw new ScriptFileNotFoundException(this.AbsolutePath);
            }
        }
    }
}
