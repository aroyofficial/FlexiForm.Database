using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Events;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a migration task containing a SQL script and provides access to its execution and profiling details.
    /// </summary>
    public class MigrationTask
    {
        /// <summary>
        /// Holds the reader used to access the script file contents.
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        /// Backing field for the <see cref="Status"/> property that holds the current status of the migration task.
        /// </summary>
        private MigrationTaskStatus _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationTask"/> class with the specified script.
        /// </summary>
        /// <param name="script">The <see cref="Script"/> object to be executed by this task.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="script"/> is null.</exception>
        public MigrationTask(Script script)
        {
            Script = script ?? throw new ArgumentNullException(nameof(script));
            _status = MigrationTaskStatus.New;
        }

        /// <summary>
        /// Occurs when a summary log entry is generated for a migration task after execution.
        /// </summary>
        public event EventHandler<TaskLogSummaryEventArgs> LogSummary;

        /// <summary>
        /// Occurs when performance tracking for a task is initiated.
        /// This event is typically raised before the execution of a task begins.
        /// </summary>
        public event EventHandler<PerformanceTrackingEventArgs> TrackPerformance;

        /// <summary>
        /// Gets the script associated with this migration task.
        /// </summary>
        public Script Script { get; }

        /// <summary>
        /// Gets or sets the retry count for the task.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the error message encountered while executing the task.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the current status of the migration task.
        /// When set, it raises the <see cref="LogSummary"/> and <see cref="TrackPerformance"/> events as appropriate.
        /// </summary>
        public MigrationTaskStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    var oldStatus = _status;
                    var newStatus = value;
                    _status = newStatus;

                    if (newStatus == MigrationTaskStatus.Picked)
                    {
                        OnTrackPerformance();
                    }

                    if (newStatus == MigrationTaskStatus.Completed ||
                        newStatus == MigrationTaskStatus.Failed ||
                        newStatus == MigrationTaskStatus.SkippedForSafety ||
                        newStatus == MigrationTaskStatus.SkippedForWrongSyntax)
                    {
                        OnLogSummary();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the performance profile of the task, including execution duration and memory usage.
        /// </summary>
        public ExecutionProfile ExecutionProfile { get; set; }

        /// <summary>
        /// Opens a stream reader to read the SQL script content from the file system.
        /// </summary>
        /// <returns>A <see cref="StreamReader"/> for reading the contents of the script file.</returns>
        public StreamReader OpenReader()
        {
            if (!IsReaderOpen())
            {
                _reader = new StreamReader(Script.AbsolutePath);
            }
            return _reader;
        }

        /// <summary>
        /// Closes and disposes the stream reader if it has been opened.
        /// </summary>
        public void CloseReader()
        {
            if (IsReaderOpen())
            {
                _reader.Close();
                _reader.Dispose();
            }
        }

        /// <summary>
        /// Raises the <see cref="LogSummary"/> event to notify that task execution has completed or been skipped.
        /// </summary>
        protected virtual void OnLogSummary()
        {
            LogSummary?.Invoke(this, new TaskLogSummaryEventArgs()
            {
                TaskLog = new TaskLog()
                {
                    Id = Script.Metadata.Id,
                    AbsolutePath = Script.AbsolutePath,
                    Status = Status,
                    RetryCount = RetryCount,
                    ExecutionProfile = ExecutionProfile
                }
            });
        }

        /// <summary>
        /// Raises the <see cref="TrackPerformance"/> event to signal the start of performance tracking for the task.
        /// </summary>
        protected virtual void OnTrackPerformance()
        {
            TrackPerformance?.Invoke(this, new PerformanceTrackingEventArgs()
            {
                Task = this
            });
        }

        /// <summary>
        /// Determines whether the internal <see cref="StreamReader"/> is currently open and readable.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the stream reader is not null, its underlying stream exists, and is readable; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReaderOpen()
        {
            return _reader != null &&
                   _reader.BaseStream != null &&
                   _reader.BaseStream.CanRead;
        }
    }
}
