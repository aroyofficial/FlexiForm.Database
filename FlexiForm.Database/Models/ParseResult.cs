using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents the result of parsing a SQL script, including success status and any errors encountered.
    /// </summary>
    public class ParseResult
    {
        /// <summary>
        /// Gets a value indicating whether the parsing was successful.
        /// Returns true if there are no errors, otherwise false.
        /// </summary>
        public bool Success => Errors == null || !Errors.Any();

        /// <summary>
        /// Gets or sets the collection of parsing errors encountered during SQL script parsing.
        /// </summary>
        public IEnumerable<ParseError> Errors { get; set; }
    }
}
