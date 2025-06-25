using FlexiForm.Database.Events;
using FlexiForm.Database.Models;
using System.Diagnostics;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Singleton service responsible for profiling migration tasks by measuring performance metrics such as
    /// execution duration and memory usage, and logging task summaries.
    /// </summary>
    public class TaskProfiler
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="TaskProfiler"/> class.
        /// </summary>
        private static TaskProfiler _instance;

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
        /// Reference to the singleton <see cref="TaskLogger"/> used for logging task details.
        /// </summary>
        private readonly TaskLogger _logger;

        /// <summary>
        /// Initializes static members of the <see cref="TaskProfiler"/> class,
        /// including the synchronization lock and instance holder.
        /// </summary>
        static TaskProfiler()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProfiler"/> class.
        /// This constructor is private to enforce the singleton pattern.
        /// </summary>
        private TaskProfiler()
        {
            _logger = TaskLogger.GetInstance();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="TaskProfiler"/> class.
        /// </summary>
        /// <returns>The singleton <see cref="TaskProfiler"/> instance.</returns>
        public static TaskProfiler GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TaskProfiler();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Subscribes to task events for tracking performance and logging summaries.
        /// </summary>
        /// <param name="task">The <see cref="MigrationTask"/> to monitor for performance and logging events.</param>
        public void Register(MigrationTask task)
        {
            task.LogSummary += OnLogSummary;
            task.TrackPerformance += OnTrackPerformance;
        }

        /// <summary>
        /// Handles the <see cref="MigrationTask.LogSummary"/> event by stopping the stopwatch,
        /// computing execution metrics, and logging the task summary.
        /// </summary>
        /// <param name="sender">The source of the event, typically the migration task.</param>
        /// <param name="e">The event arguments containing the <see cref="TaskLog"/> to be logged.</param>
        private void OnLogSummary(object? sender, TaskLogSummaryEventArgs e)
        {
            _stopwatch.Stop();
            e.TaskLog.ExecutionProfile = new ExecutionProfile
            {
                Duration = _stopwatch.ElapsedMilliseconds,
                MemoryUsage = GC.GetTotalMemory(true) - _usedMemory
            };
            _logger.Log(e.TaskLog);
        }

        /// <summary>
        /// Handles the <see cref="MigrationTask.TrackPerformance"/> event by starting a stopwatch
        /// and capturing the current memory usage for performance tracking.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the task whose performance is being tracked.</param>
        private void OnTrackPerformance(object? sender, PerformanceTrackingEventArgs e)
        {
            _logger.LogCurrentExecution(e.Task);
            _stopwatch = Stopwatch.StartNew();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _usedMemory = GC.GetTotalMemory(true);
        }
    }
}
