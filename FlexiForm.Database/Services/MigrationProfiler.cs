using Dapper;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Events;
using FlexiForm.Database.Models;
using System.Data;
using System.Diagnostics;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Singleton service responsible for profiling migration tasks by measuring performance metrics such as
    /// execution duration and memory usage, and logging task summaries.
    /// </summary>
    public class MigrationProfiler
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="MigrationProfiler"/> class.
        /// </summary>
        private static MigrationProfiler _instance;

        /// <summary>
        /// A synchronization object used to ensure thread-safe access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// Stopwatch used to measure task execution duration.
        /// </summary>
        private static Stopwatch _stopwatch;

        /// <summary>
        /// Stores the memory usage at the beginning of performance tracking.
        /// </summary>
        private static long _usedMemory;

        /// <summary>
        /// Reference to the singleton <see cref="Logger"/> used for logging task details.
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// Provides access to the database for executing SQL commands and managing transactions during migration operations.
        /// </summary>
        private readonly DBConnector _dbConnector;

        /// <summary>
        /// Initializes static members of the <see cref="MigrationProfiler"/> class,
        /// including the synchronization lock and instance holder.
        /// </summary>
        static MigrationProfiler()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationProfiler"/> class.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private MigrationProfiler()
        {
            _dbConnector = DBConnector.GetInstance();
            _logger = Logger.GetInstance();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="MigrationProfiler"/> class.
        /// </summary>
        /// <returns>The singleton <see cref="MigrationProfiler"/> instance.</returns>
        public static MigrationProfiler GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new MigrationProfiler();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Subscribes to task events for tracking performance and logging summaries.
        /// </summary>
        /// <param name="task">The <see cref="Migration"/> to monitor for performance and logging events.</param>
        public void Register(Migration task)
        {
            task.OnStart += TrackPerformance;
            task.OnCompleted += Archive;
        }

        /// <summary>
        /// Handles the <see cref="Migration.OnCompleted"/> event by stopping the stopwatch,
        /// computing execution metrics, and logging the task summary.
        /// </summary>
        /// <param name="sender">The source of the event, typically the migration task.</param>
        /// <param name="e">The event arguments containing the <see cref="MigrationLog"/> to be logged.</param>
        private void Archive(object? sender, OnCompletedEventArgs e)
        {
            _stopwatch.Stop();
            if (e.Log.Status == MigrationTaskStatus.Completed)
            {
                AddLog(e.Log);
            }
            e.Log.ExecutionProfile = new ExecutionProfile
            {
                Duration = _stopwatch.ElapsedMilliseconds,
                MemoryUsage = GC.GetTotalMemory(true) - _usedMemory
            };
            _logger.Log(e.Log);
        }

        /// <summary>
        /// Handles the <see cref="Migration.OnStart"/> event by starting a stopwatch
        /// and capturing the current memory usage for performance tracking.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the task whose performance is being tracked.</param>
        private void TrackPerformance(object? sender, OnStartEventArgs e)
        {
            _logger.LogMigration(e.Task);
            _stopwatch = Stopwatch.StartNew();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _usedMemory = GC.GetTotalMemory(true);
        }

        /// <summary>
        /// Adds a migration log entry to the database by executing the <c>usp_AddMigrationHistory</c> stored procedure.
        /// </summary>
        /// <param name="log">The <see cref="MigrationLog"/> object containing details such as ID, script name, hash, and status.</param>
        private void AddLog(MigrationLog log)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", log.Id);
            parameters.Add("@scriptname", log.Name);
            parameters.Add("@scripthash", log.Hash);
            parameters.Add("@type", Convert.ToByte(log.MigrationType));

            var command = new DBCommand()
            {
                Sql = "usp_AddMigrationHistory",
                Parameters = parameters,
                CommandType = CommandType.StoredProcedure,
                UseNewConnection = true
            };

            _dbConnector.Execute(command);
        }
    }
}
