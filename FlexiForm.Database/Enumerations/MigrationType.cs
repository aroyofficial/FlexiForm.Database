namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Specifies the type of database migration represented by a script.
    /// </summary>
    public enum MigrationType
    {
        /// <summary>
        /// Indicates that no specific migration direction is defined.
        /// </summary>
        None,

        /// <summary>
        /// Represents a forward migration typically used to apply schema or data changes.
        /// </summary>
        Up,

        /// <summary>
        /// Represents a rollback migration used to undo changes introduced in an Up migration.
        /// </summary>
        Down,

        /// <summary>
        /// Indicates that the script contains both Up and Down migration logic.
        /// </summary>
        Both
    }
}
