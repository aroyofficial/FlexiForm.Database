namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Represents the types of items that can be executed by the task runner.
    /// Supports bitwise combinations to include multiple targets.
    /// </summary>
    [Flags]
    public enum ExecutionTarget
    {
        /// <summary>
        /// No items selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include stored procedures.
        /// </summary>
        Proc = 1 << 0,

        /// <summary>
        /// Include schemas.
        /// </summary>
        Schema = 1 << 1,

        /// <summary>
        /// Include alter scripts.
        /// </summary>
        Alter = 1 << 2,

        /// <summary>
        /// Include index scripts.
        /// </summary>
        Index = 1 << 3,

        /// <summary>
        /// Include all targets.
        /// </summary>
        All = Proc | Schema | Alter | Index
    }
}
