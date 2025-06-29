using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Services;

namespace FlexiForm.Database
{
    /// <summary>
    /// Entry point for the FlexiForm database task runner application.
    /// This class is responsible for initializing services, handling lifecycle events,
    /// configuring the environment, and executing the migration process.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Static instance of the <see cref="MigrationEngine"/> used to coordinate
        /// the execution of all configured migration tasks in the system.
        /// </summary>
        private static readonly MigrationEngine _engine;

        /// <summary>
        /// Static instance of the <see cref="Logger"/> responsible for logging 
        /// configuration settings, execution progress, exceptions, and summaries.
        /// </summary>
        private static readonly Logger _logger;

        /// <summary>
        /// Static instance of the <see cref="GlobalProfiler"/> that manages 
        /// profiling tasks related to resource usage and performance monitoring.
        /// </summary>
        private static readonly GlobalProfiler _profiler;

        /// <summary>
        /// Static constructor that initializes singleton service instances used throughout
        /// the application lifecycle, including the migration engine, logger, and profiler.
        /// </summary>
        static Program()
        {
            _engine = MigrationEngine.GetInstance();
            _logger = Logger.GetInstance();
            _profiler = GlobalProfiler.GetInstance();
        }

        /// <summary>
        /// The main method and entry point of the application.
        /// It parses command-line arguments, configures the migration engine,
        /// and initiates the execution of migration tasks.
        /// Errors are caught and logged appropriately to help in diagnostics.
        /// </summary>
        /// <param name="args">Command-line arguments that control the behavior of the migration task runner.</param>
        public static void Main(string[] args)
        {
            try
            {
                _engine.Configure(args);
                _engine.Start();
            }
            catch (Exception ex)
            {
                _logger.LogMessage("Oops! Something went wrong...");

                if (ex is BaseException baseEx)
                {
                    _logger.LogMessage(baseEx.Message);
                }
                else
                {
                    _logger.LogMessage(ex.Message);
                }
            }
        }
    }
}
