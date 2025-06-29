using FlexiForm.Database.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlexiForm.Database.Configurations
{
    /// <summary>
    /// Represents the set of configurable options used by the migration engine to control task execution,
    /// error handling strategy, environment detection, and target selection.
    /// </summary>
    public class MigrationEngineConfiguration
    {
        /// <summary>
        /// Gets or sets the type of migration to perform.
        /// Default is <see cref="MigrationType.Both"/>, which includes both infrastructure and application-level migrations.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationType Type { get; set; } = MigrationType.Both;

        /// <summary>
        /// Gets the current host environment based on the 'ASPNETCORE_ENVIRONMENT' environment variable.
        /// Returns a value from the <see cref="HostEnvironment"/> enumeration.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public HostEnvironment Environment => DetectEnvironment();

        /// <summary>
        /// Gets or sets the timeout duration (in seconds) for individual migration tasks.
        /// Default is <c>1</c> second.
        /// </summary>
        public int TaskTimeout { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for a failed migration task.
        /// Default is <c>2</c> retries.
        /// </summary>
        public int MaxRetryCount { get; set; } = 2;

        /// <summary>
        /// Gets or sets the strategy used for executing migration tasks.
        /// <see cref="MigrationStrategy.Strict"/> rolls back all changes on failure.
        /// <see cref="MigrationStrategy.Relaxed"/> continues execution even if some scripts fail.
        /// Default is <see cref="MigrationStrategy.Strict"/>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationStrategy Strategy { get; set; } = MigrationStrategy.Strict;

        /// <summary>
        /// Gets or sets the run mode for the migration process.
        /// Determines whether to run all scripts (<see cref="MigrationRunMode.Full"/>) or only new/updated ones (<see cref="MigrationRunMode.Incremental"/>).
        /// Default is <see cref="MigrationRunMode.Incremental"/>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationRunMode RunMode { get; set; } = MigrationRunMode.Incremental;

        /// <summary>
        /// Gets or sets the database object types (e.g., procedures, schemas, constraints) to include in the migration.
        /// Default is <see cref="ExecutionTarget.All"/>, meaning all supported types will be considered.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionTarget Target { get; set; } = ExecutionTarget.All;

        /// <summary>
        /// Gets the number of scripts to be executed per batch.
        /// This is a constant value set to <c>5</c>.
        /// </summary>
        public int BatchCount => 5;

        /// <summary>
        /// Detects the current application environment by reading the 'ASPNETCORE_ENVIRONMENT' system environment variable.
        /// Returns <see cref="HostEnvironment.Unknown"/> if the environment value is not recognized or not set.
        /// </summary>
        /// <returns>The resolved <see cref="HostEnvironment"/> value.</returns>
        private static HostEnvironment DetectEnvironment()
        {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return env?.ToLower() switch
            {
                "development" => HostEnvironment.Development,
                "testing" => HostEnvironment.Testing,
                "staging" => HostEnvironment.Staging,
                "production" => HostEnvironment.Production,
                _ => HostEnvironment.Unknown
            };
        }
    }
}
