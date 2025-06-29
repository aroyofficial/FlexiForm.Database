namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Defines the strategy to be used during a migration run.
    /// </summary>
    public enum MigrationRunMode
    {
        /// <summary>
        /// Performs an incremental migration by processing only the data modified since the last run.
        /// </summary>
        Incremental,

        /// <summary>
        /// Performs a full migration by processing all data, regardless of previous runs.
        /// </summary>
        Full
    }
}
