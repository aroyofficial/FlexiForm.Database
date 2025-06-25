using Dapper;
using FlexiForm.Database.Configurations;
using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Provides a singleton instance of the <see cref="TaskExecutor"/> class,
    /// responsible for managing and executing configured database tasks.
    /// </summary>
    public class TaskExecutor
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="TaskExecutor"/>.
        /// </summary>
        private static TaskExecutor? _instance;

        /// <summary>
        /// An object used to synchronize access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// Holds the configuration used to determine execution behavior.
        /// </summary>
        private TaskRunnerConfiguration _configuration;

        /// <summary>
        /// The database connection used for executing migration tasks.
        /// </summary>
        private readonly IDbConnection _connection;

        /// <summary>
        /// Initializes static members of the <see cref="TaskExecutor"/> class.
        /// Sets up the synchronization lock and initializes the singleton instance to null.
        /// </summary>
        static TaskExecutor()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutor"/> class.
        /// The constructor is private to enforce the Singleton pattern.
        /// </summary>
        private TaskExecutor()
        {
            _configuration = new TaskRunnerConfiguration();
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString__PrimaryDB", EnvironmentVariableTarget.Machine);
            _connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="TaskExecutor"/> class.
        /// If the instance does not exist, it is created in a thread-safe manner.
        /// </summary>
        /// <returns>The singleton instance of <see cref="TaskExecutor"/>.</returns>
        public static TaskExecutor GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new TaskExecutor();
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// Sets the task runner configuration used by the singleton instance.
        /// </summary>
        /// <param name="configuration">The <see cref="TaskRunnerConfiguration"/> to use.</param>
        public void SetConfiguration(TaskRunnerConfiguration configuration)
        {
            if (_instance != null)
            {
                _instance._configuration = configuration;
            }
        }

        /// <summary>
        /// Executes batches of migration tasks. In strict mode, all batches are wrapped
        /// in a single transaction and will be rolled back if any task fails.
        /// In relaxed mode, each batch is executed in its own transaction and failures do not stop execution.
        /// </summary>
        /// <param name="taskBatches">A queue of task batches, where each batch is a queue of <see cref="MigrationTask"/> instances.</param>
        /// <exception cref="MigrationFailedInStrictModeException">
        /// Thrown when a task fails in strict mode, causing a rollback of all previously executed tasks.
        /// </exception>
        public void ExecuteBatches(Queue<Queue<MigrationTask>> taskBatches)
        {
            try
            {
                _connection.Open();
                var isStrict = _configuration.Mode == TaskRunnerMode.Strict;

                using (var globalTransaction = isStrict ? _connection.BeginTransaction() : null)
                {
                    try
                    {
                        while (taskBatches.Count > 0)
                        {
                            var batch = taskBatches.Dequeue();
                            var batchLevelTransaction = isStrict ? globalTransaction : _connection.BeginTransaction();

                            try
                            {
                                foreach (var task in batch)
                                {
                                    task.Status = MigrationTaskStatus.Picked;

                                    while (task.Status != MigrationTaskStatus.Completed &&
                                        task.RetryCount < _configuration.MaxRetryCount)
                                    {
                                        try
                                        {
                                            task.Status = MigrationTaskStatus.Executing;
                                            var reader = task.OpenReader();
                                            var sql = reader.ReadToEndAsync().GetAwaiter().GetResult();

                                            if (!string.IsNullOrWhiteSpace(sql))
                                            {
                                                _connection.ExecuteAsync(sql, transaction: batchLevelTransaction, commandTimeout: _configuration.TaskTimeout)
                                                    .GetAwaiter().GetResult();
                                                task.Status = MigrationTaskStatus.Completed;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (task.RetryCount < _configuration.MaxRetryCount)
                                            {
                                                task.Status = MigrationTaskStatus.Retrying;
                                            }
                                            else
                                            {
                                                task.Status = MigrationTaskStatus.Failed;
                                            }
                                            task.Error = ex.Message;
                                            task.RetryCount++;
                                        }

                                        task.CloseReader();
                                    }
                                }

                                if (!isStrict)
                                {
                                    batchLevelTransaction?.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                batchLevelTransaction?.Rollback();

                                if (isStrict)
                                {
                                    throw new MigrationFailedInStrictModeException();
                                }
                            }
                        }

                        if (isStrict)
                        {
                            globalTransaction?.Commit();
                        }
                    }
                    catch
                    {
                        if (isStrict)
                        {
                            globalTransaction?.Rollback();
                        }

                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
