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
        /// The main entry point of the application.
        /// Parses command-line arguments and triggers the task runner execution.
        /// </summary>
        /// <param name="args">Command-line arguments used to configure the task runner.</param>
        public static void Main(string[] args)
        {
            try
            {
                TaskRunner.Configure(args);
                TaskRunner.Run();
                PrintMessage("[Success] All migration tasks completed successfully.");
            }
            catch (Exception ex)
            {
                if (ex is BaseException baseEx)
                {
                    PrintMessage(baseEx.Message);
                }
                else
                {
                    PrintMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// Prints the specified message to the console, prefixed with the current local timestamp.
        /// </summary>
        /// <param name="message">The message content to display in the console output.</param>
        private static void PrintMessage(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]\n{message}");
        }
    }
}
