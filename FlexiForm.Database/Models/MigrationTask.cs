using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Models
{
    /// <summary>
    /// Represents a migration task containing a SQL script and provides access to its content.
    /// </summary>
    public class MigrationTask
    {
        /// <summary>
        /// Holds the reader used to access the script file contents.
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationTask"/> class with the specified script.
        /// </summary>
        /// <param name="script">The <see cref="Script"/> object to be executed by this task.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="script"/> is null.</exception>
        public MigrationTask(Script script)
        {
            Script = script ?? throw new ArgumentNullException(nameof(script));
            Errors = new List<string>();
        }

        /// <summary>
        /// Gets the script associated with this migration task.
        /// </summary>
        public Script Script { get; }

        /// <summary>
        /// Gets or sets the retry count for the task.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets the errors encountered while executing task.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the current status of the migration task.
        /// Default value is <see cref="MigrationTaskStatus.New"/>.
        /// </summary>
        public MigrationTaskStatus Status { get; set; } = MigrationTaskStatus.New;

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
