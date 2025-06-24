namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Represents the various types of SQL scripts used within the FlexiForm database.
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// Indicates an undefined or unclassified script type.
        /// </summary>
        Unknown,

        /// <summary>
        /// A script that defines or modifies database schemas (tables, columns, etc.).
        /// </summary>
        Schema,

        /// <summary>
        /// A stored procedure definition script.
        /// </summary>
        Proc,

        /// <summary>
        /// Represents a script used to apply non-destructive changes to the database schema,
        /// such as altering columns, adding or removing constraints (e.g., primary keys, 
        /// foreign keys, check constraints), or modifying indexes.
        /// </summary>
        Alter
    }
}
