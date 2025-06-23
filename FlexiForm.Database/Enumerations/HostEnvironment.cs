namespace FlexiForm.Database.Enumerations
{
    /// <summary>
    /// Represents the various hosting environments in which the application can run.
    /// Useful for configuring environment-specific behavior such as logging, database connections, and feature toggles.
    /// </summary>
    public enum HostEnvironment
    {
        /// <summary>
        /// The environment could not be determined or was not specified.
        /// Typically used as a fallback or default value.
        /// </summary>
        Unknown,

        /// <summary>
        /// The development environment, typically used for local development and debugging.
        /// </summary>
        Development,

        /// <summary>
        /// The testing environment, typically used for running automated tests or QA validation.
        /// </summary>
        Testing,

        /// <summary>
        /// The staging environment, which mimics production for final testing before deployment.
        /// </summary>
        Staging,

        /// <summary>
        /// The live production environment where the application is actively used by end users.
        /// </summary>
        Production
    }
}
