namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Represents a global profiler responsible for managing the lifecycle of shared resources
    /// (such as database connections) during the migration workflow.
    /// This class registers event handlers to the <see cref="MigrationEngine"/> to ensure
    /// that resources are properly acquired before execution and released after completion.
    /// </summary>
    public class GlobalProfiler
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="GlobalProfiler"/> class.
        /// Ensures a single global resource manager is used throughout the application.
        /// </summary>
        private static GlobalProfiler _instance;

        /// <summary>
        /// A thread-safe synchronization object used to control access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// Reference to the singleton instance of the <see cref="MigrationEngine"/> used for event registration.
        /// </summary>
        private readonly MigrationEngine _engine;

        /// <summary>
        /// Reference to the singleton instance of the <see cref="DBConnector"/> used for database connection control.
        /// </summary>
        private readonly DBConnector _dbConnector;

        /// <summary>
        /// Initializes static members of the <see cref="GlobalProfiler"/> class.
        /// Sets up the synchronization lock and prepares the singleton instance.
        /// </summary>
        static GlobalProfiler()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalProfiler"/> class.
        /// Automatically wires up resource lifecycle events with the <see cref="MigrationEngine"/>.
        /// </summary>
        private GlobalProfiler()
        {
            _engine = MigrationEngine.GetInstance();
            _dbConnector = DBConnector.GetInstance();
            Run();
        }

        /// <summary>
        /// Retrieves the singleton instance of the <see cref="GlobalProfiler"/> class.
        /// Ensures thread-safe, lazy initialization of the global profiler.
        /// </summary>
        /// <returns>The single instance of the <see cref="GlobalProfiler"/>.</returns>
        public static GlobalProfiler GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new GlobalProfiler();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Subscribes internal resource lifecycle handlers to the <see cref="MigrationEngine"/> events.
        /// Specifically listens to <c>OnStart</c> and <c>OnComplete</c> events to manage resource access.
        /// </summary>
        private void Run()
        {
            _engine.OnStart += AcquireResource;
            _engine.OnComplete += ReleaseResource;
            _engine.OnFailed += ReleaseResource;
        }

        /// <summary>
        /// Handles the <c>OnStart</c> event of the <see cref="MigrationEngine"/>.
        /// Establishes and opens a database connection before the migration tasks begin execution.
        /// </summary>
        /// <param name="sender">The source of the event, typically the <see cref="MigrationEngine"/> instance.</param>
        /// <param name="e">Event arguments (not used).</param>
        public void AcquireResource(object? sender, EventArgs e)
        {
            _dbConnector.OpenConnection();
        }

        /// <summary>
        /// Handles the <c>OnComplete</c> event of the <see cref="MigrationEngine"/>.
        /// Closes and releases the database connection after the migration tasks are completed.
        /// </summary>
        /// <param name="sender">The source of the event, typically the <see cref="MigrationEngine"/> instance.</param>
        /// <param name="e">Event arguments (not used).</param>
        public void ReleaseResource(object? sender, EventArgs e)
        {
            _dbConnector.CloseConnection();
        }
    }
}
