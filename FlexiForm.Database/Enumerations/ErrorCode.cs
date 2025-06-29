namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Defines application-specific error codes used for identifying and categorizing exceptions
    /// within the FlexiForm database system.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// An unspecified or unknown error occurred.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The script object provided was null or invalid.
        /// </summary>
        InvalidScript = 1001,

        /// <summary>
        /// The specified script file was not found on disk.
        /// </summary>
        ScriptFileNotFound = 1002,

        /// <summary>
        /// The given script path points to a directory, not a file.
        /// </summary>
        ScriptPathIsDirectory = 1003,

        /// <summary>
        /// The script header is missing a required Script ID.
        /// </summary>
        ScriptIdMissing = 1004,

        /// <summary>
        /// The script header is missing a valid Script Type.
        /// </summary>
        ScriptTypeMissing = 1005,

        /// <summary>
        /// The script header is missing a valid Name field.
        /// </summary>
        ScriptNameMissing = 1006,

        /// <summary>
        /// The script header is missing a valid Migration Type.
        /// </summary>
        MigrationTypeMissing = 1007,

        /// <summary>
        /// The script header is missing a valid Created At timestamp.
        /// </summary>
        CreatedAtMissing = 1008,

        /// <summary>
        /// The audit metadata (Created At or Updated At) is malformed or has an invalid format.
        /// </summary>
        MalformedAudit = 1009,

        /// <summary>
        /// A required value for a command-line flag is missing.
        /// </summary>
        MissingFlagValue = 1010,

        /// <summary>
        /// The value provided for a command-line flag is invalid or out of expected range.
        /// </summary>
        InvalidFlagValue = 1011,

        /// <summary>
        /// An unrecognized or unsupported command-line flag was provided.
        /// </summary>
        UnsupportedFlag = 1012,

        /// <summary>
        /// The specified folder for scripts could not be found.
        /// </summary>
        ScriptFolderNotFound = 1013,

        /// <summary>
        /// A migration task failed, potentially requiring a rollback of all previous changes in strict mode.
        /// </summary>
        MigrationFailed = 1014,

        /// <summary>
        /// An error occurred while writing to the migration log file.
        /// </summary>
        MigrationLogWriteFailure = 1015,
        InfrastructureSetupFailure = 1016
    }
}
