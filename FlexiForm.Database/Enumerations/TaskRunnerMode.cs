namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Specifies the execution mode for the task runner.
    /// </summary>
    public enum TaskRunnerMode
    {
        /// <summary>
        /// Executes in strict mode: any failure during script execution results in stopping and rolling back all changes.
        /// </summary>
        Strict,

        /// <summary>
        /// Executes in relaxed mode: continues executing remaining scripts even if some scripts fail.
        /// </summary>
        Relaxed
    }
}
