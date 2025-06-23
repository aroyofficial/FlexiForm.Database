namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Represents the various states a migration task can be in during its lifecycle.
    /// </summary>
    public enum MigrationTaskStatus
    {
        /// <summary>
        /// The task is newly created and has not yet started execution.
        /// </summary>
        New,

        /// <summary>
        /// The task is currently being executed.
        /// </summary>
        Executing,

        /// <summary>
        /// The task has completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The task has failed during execution.
        /// </summary>
        Failed,

        /// <summary>
        /// The task is being retried after a previous failure.
        /// </summary>
        Retrying,

        /// <summary>
        /// The task was skipped due to invalid or incorrect SQL syntax detected during parsing.
        /// </summary>
        SkippedForWrongSyntax,

        /// <summary>
        /// The task was skipped because it was deemed unsafe to execute based on predefined safety rules or checks.
        /// </summary>
        SkippedForSafety
    }
}
