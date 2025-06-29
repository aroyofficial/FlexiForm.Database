using Dapper;
using System.Data;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a database command with SQL text, parameters, transaction context, timeout setting, and command type.
    /// </summary>
    public class DBCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBCommand"/> class with default values.
        /// </summary>
        public DBCommand()
        {
            Sql = string.Empty;
            Parameters = new DynamicParameters();
            Transaction = null;
            Timeout = null;
            CommandType = CommandType.Text;
        }

        /// <summary>
        /// Gets or sets the SQL query or command text to be executed.
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// Gets or sets the parameters to be passed with the SQL command.
        /// </summary>
        public DynamicParameters Parameters { get; set; }

        /// <summary>
        /// Gets or sets the database transaction associated with the command, if any.
        /// </summary>
        public IDbTransaction? Transaction { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds. If null, the default timeout is used.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the type of the database command (e.g., Text, StoredProcedure).
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a new database connection should be used
        /// for the current operation instead of reusing an existing one.
        /// </summary>
        /// <value>
        /// <c>true</c> if a new connection should be established; otherwise, <c>false</c>.
        /// </value>
        public bool UseNewConnection { get; set; }
    }
}
