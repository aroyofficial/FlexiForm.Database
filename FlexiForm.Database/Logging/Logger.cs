using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlexiForm.Database.Logging
{
    /// <summary>
    /// Provides functionality to log <see cref="MigrationTask"/> entries into a single JSON array file during the application's lifetime.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// JSON serialization options used for formatting and enum string conversion.
        /// </summary>
        private static readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Path to the log file used for recording migration tasks.
        /// </summary>
        private static string _logFilePath;

        /// <summary>
        /// Indicates whether the first log entry has been written.
        /// </summary>
        private static bool _firstWrite;

        /// <summary>
        /// An object used to synchronize access to the log file.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// Static constructor to initialize logging resources, including creating the log file and configuring serialization.
        /// </summary>
        static Logger()
        {
            _firstWrite = true;
            _lock = new();
            _jsonOptions = new()
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            CreateLogFile();
        }

        /// <summary>
        /// Appends a serialized <typeparamref name="T"/> object to the JSON log file.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize and log.</typeparam>
        /// <param name="obj">The object instance to log.</param>
        public static void Log<T>(T obj)
        {
            lock (_lock)
            {
                try
                {
                    string json = JsonSerializer.Serialize(obj, _jsonOptions);

                    using (var stream = new FileStream(_logFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    using (var writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        stream.Seek(-1, SeekOrigin.End);

                        if (!_firstWrite)
                        {
                            writer.Write("," + Environment.NewLine);
                        }
                        else
                        {
                            _firstWrite = false;
                        }

                        writer.Write(json);
                        writer.Write(Environment.NewLine + "]");
                    }
                }
                catch (Exception ex)
                {
                    throw new MigrationLogWriteException(ex);
                }
            }
        }

        /// <summary>
        /// Creates a new JSON log file in the application's log directory with a local timestamped filename.
        /// </summary>
        private static void CreateLogFile()
        {
            var logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDir);
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ssZ");
            _logFilePath = Path.Combine(logsDir, $"{timestamp}.log");
            File.WriteAllText(_logFilePath, "[" + Environment.NewLine);
        }
    }
}
