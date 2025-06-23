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
        /// A script for defining constraints such as primary keys, foreign keys, or checks.
        /// </summary>
        Constraint
    }
}
