using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Services;

namespace FlexiForm.Database
{
    /// <summary>
    /// Entry point for the FlexiForm database task runner application.
    /// Initializes configuration from command-line arguments and executes the task runner.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Static instance of the task runner responsible for managing and executing migration tasks.
        /// </summary>
        private static readonly TaskRunner _runner;

        /// <summary>
        /// Static instance of the task logger responsible for logging configuration, task progress, and results.
        /// </summary>
        private static readonly TaskLogger _logger;

        /// <summary>
        /// Static constructor that initializes the task runner and logger instances.
        /// </summary>
        static Program()
        {
            _runner = TaskRunner.GetInstance();
            _logger = TaskLogger.GetInstance();
        }

        /// <summary>
        /// The main entry point of the application.
        /// Parses command-line arguments and triggers the task runner execution.
        /// </summary>
        /// <param name="args">Command-line arguments used to configure the task runner.</param>
        public static void Main(string[] args)
        {
            try
            {
                _runner.Configure(args);
                _runner.Run();
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
