namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Defines the strategy for executing database migration scripts.
    /// Determines how failures are handled during batch execution.
    /// </summary>
    public enum MigrationStrategy
    {
        /// <summary>
        /// Executes all scripts within a single transaction.
        /// If any script fails, the entire batch is rolled back and execution is halted.
        /// </summary>
        Strict,

        /// <summary>
        /// Executes each script independently.
        /// If a script fails, execution continues with the remaining scripts; failed scripts are logged.
        /// </summary>
        Relaxed
    }
}
