using FlexiForm.Database.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlexiForm.Database.Configurations
{
    /// <summary>
    /// Represents the configuration settings for the task runner.
    /// </summary>
    public class TaskRunnerConfiguration
    {
        /// <summary>
        /// Gets or sets the type of migration to run.
        /// Default is <see cref="MigrationType.Both"/>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public MigrationType Migration { get; set; } = MigrationType.Both;

        /// <summary>
        /// Gets the current host environment by detecting the value of the 'ASPNETCORE_ENVIRONMENT' system variable.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public HostEnvironment Environment => DetectEnvironment();

        /// <summary>
        /// Gets or sets the timeout for a task in seconds.
        /// Default is 1 second.
        /// </summary>
        public int TaskTimeout { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for a failed task.
        /// Default is 2.
        /// </summary>
        public int MaxRetryCount { get; set; } = 2;

        /// <summary>
        /// Gets or sets the task runner execution mode.
        /// Determines whether the runner should stop and rollback on errors (<see cref="TaskRunnerMode.Strict"/>)
        /// or continue execution despite errors (<see cref="TaskRunnerMode.Relaxed"/>).
        /// Default is <see cref="TaskRunnerMode.Strict"/>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TaskRunnerMode Mode { get; set; } = TaskRunnerMode.Strict;

        /// <summary>
        /// Gets or sets a value indicating whether the migration should be executed incrementally.
        /// When set to <c>true</c>, only new or modified scripts are executed.
        /// When set to <c>false</c>, a full migration is performed, re-executing all scripts.
        /// Default is <c>false</c>.
        /// </summary>
        public bool ExecuteIncrementally { get; set; } = false;

        /// <summary>
        /// Gets or sets the execution target, which specifies the types of database items
        /// (e.g., stored procedures, schemas, constraints) to be included during the task execution.
        /// Defaults to <see cref="ExecutionTarget.All"/>, meaning all supported types will be executed.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionTarget Target { get; set; } = ExecutionTarget.All;

        /// <summary>
        /// Gets the number of scripts to be executed in a single batch.
        /// This value is fixed at <c>5</c>.
        /// </summary>
        public int BatchCount => 5;

        /// <summary>
        /// Detects the current host environment by reading the 'ASPNETCORE_ENVIRONMENT' environment variable.
        /// Returns <see cref="HostEnvironment.Unknown"/> if the value does not match a known environment.
        /// </summary>
        /// <returns>The detected <see cref="HostEnvironment"/> value.</returns>
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
