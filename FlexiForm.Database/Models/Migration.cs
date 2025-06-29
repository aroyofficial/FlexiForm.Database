using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Events;
using System.Security.Cryptography;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a single migration task, encapsulating the associated SQL script and 
    /// providing mechanisms to manage execution, track status, and emit lifecycle events.
    /// </summary>
    public class Migration
    {
        /// <summary>
        /// Backing field for the <see cref="Status"/> property, indicating the current execution state of the task.
        /// </summary>
        private MigrationTaskStatus _status;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration"/> class with the specified script.
        /// </summary>
        /// <param name="script">The SQL script to be executed as part of this migration task.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="script"/> is <c>null</c>.</exception>
        public Migration(Script script)
        {
            Script = script ?? throw new ArgumentNullException(nameof(script));
            _status = MigrationTaskStatus.New;
            Script.Metadata.Hash = GetHash();
        }

        /// <summary>
        /// Occurs when the task has completed execution, whether successfully or not.
        /// Provides execution summary details via the <see cref="OnCompletedEventArgs"/>.
        /// </summary>
        public event EventHandler<OnCompletedEventArgs> OnCompleted;

        /// <summary>
        /// Occurs just before the task begins execution.
        /// Provides task context to subscribers via <see cref="OnStartEventArgs"/>.
        /// </summary>
        public event EventHandler<OnStartEventArgs> OnStart;

        /// <summary>
        /// Gets the script that defines the migration operation.
        /// </summary>
        public Script Script { get; }

        /// <summary>
        /// Gets or sets the number of times the task has been retried due to failure.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the error message, if any, encountered during task execution.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the current status of the migration task.
        /// Setting this property may trigger lifecycle events based on status transitions.
        /// </summary>
        public MigrationTaskStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    var newStatus = value;
                    _status = newStatus;

                    if (newStatus == MigrationTaskStatus.Picked)
                        InvokeOnStart();

                    if (newStatus == MigrationTaskStatus.Executing)
                        RanAt = DateTime.UtcNow;

                    if (newStatus == MigrationTaskStatus.Completed ||
                        newStatus == MigrationTaskStatus.Failed ||
                        newStatus == MigrationTaskStatus.SkippedForSafety ||
                        newStatus == MigrationTaskStatus.SkippedForWrongSyntax)
                    {
                        InvokeOnCompleted();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets profiling information for the task, such as execution duration and memory usage.
        /// </summary>
        public ExecutionProfile ExecutionProfile { get; set; }

        /// <summary>
        /// Gets the timestamp indicating when the task started execution.
        /// Set internally when the status changes to <see cref="MigrationTaskStatus.Executing"/>.
        /// </summary>
        public DateTime? RanAt { get; private set; }

        /// <summary>
        /// Raises the <see cref="OnCompleted"/> event to signal that the task has finished.
        /// Constructs a <see cref="MigrationLog"/> with relevant execution data for subscribers.
        /// </summary>
        protected virtual void InvokeOnCompleted()
        {
            OnCompleted?.Invoke(this, new OnCompletedEventArgs()
            {
                Log = new MigrationLog()
                {
                    Id = Script.Metadata.Id,
                    Name = Script.Metadata.Name,
                    Hash = Script.Metadata.Hash,
                    AbsolutePath = Script.AbsolutePath,
                    Status = Status,
                    MigrationType = Script.Metadata.MigrationType,
                    RetryCount = RetryCount,
                    ExecutionProfile = ExecutionProfile,
                    RanAt = RanAt
                }
            });
        }

        /// <summary>
        /// Raises the <see cref="OnStart"/> event to indicate that the task is starting.
        /// Provides the current task instance to any subscribed event handlers.
        /// </summary>
        protected virtual void InvokeOnStart()
        {
            OnStart?.Invoke(this, new OnStartEventArgs()
            {
                Task = this
            });
        }

        /// <summary>
        /// Computes and returns the SHA256 hash of the migration script file associated with this task.
        /// The hash is generated from the contents of the file located at <see cref="Script.AbsolutePath"/>.
        /// </summary>
        /// <returns>
        /// A lowercase hexadecimal string representing the SHA256 hash of the script file contents.
        /// </returns>
        private string GetHash()
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(Script.AbsolutePath))
            {
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
