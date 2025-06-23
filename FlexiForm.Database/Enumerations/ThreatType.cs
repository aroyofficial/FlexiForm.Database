namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Represents known categories of SQL-based threats identified during script scanning.
    /// </summary>
    public enum ThreatType
    {
        /// <summary>
        /// Indicates that no threat was detected or the input does not match any known threat pattern.
        /// </summary>
        None,

        /// <summary>
        /// Dangerous command that drops an entire database.
        /// </summary>
        DropDatabase,

        /// <summary>
        /// Execution of system-level commands via xp_cmdshell.
        /// </summary>
        XpCmdShellExecution,

        /// <summary>
        /// Classic SQL injection using UNION SELECT.
        /// </summary>
        UnionSelectInjection,

        /// <summary>
        /// Time-based attack using WAITFOR DELAY.
        /// </summary>
        TimeDelayAttack,

        /// <summary>
        /// Boolean-based SQL injection (e.g., OR 1 = 1).
        /// </summary>
        BooleanInjection,

        /// <summary>
        /// Use of hexadecimal-encoded payloads or literals.
        /// </summary>
        HexLiteralPayload
    }
}
