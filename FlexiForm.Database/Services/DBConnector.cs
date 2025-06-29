using Dapper;
using FlexiForm.Database.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlexiForm.Database.Services
{
    /// <summary>
    /// Provides a thread-safe singleton service for managing and executing SQL database operations using Dapper.
    /// </summary>
    public class DBConnector
    {
        /// <summary>
        /// Holds the singleton instance of the <see cref="DBConnector"/>.
        /// </summary>
        private static DBConnector? _instance;

        /// <summary>
        /// Synchronization object used for thread-safe access to the singleton instance.
        /// </summary>
        private static readonly object _lock;

        /// <summary>
        /// The database connection string retrieved from environment variables.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// The persistent database connection used for command execution.
        /// </summary>
        private readonly IDbConnection _connection;

        /// <summary>
        /// Initializes static members of the <see cref="DBConnector"/> class.
        /// </summary>
        static DBConnector()
        {
            _lock = new();
            _instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnector"/> class using the environment-provided connection string.
        /// </summary>
        private DBConnector()
        {
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString__PrimaryDB", EnvironmentVariableTarget.Machine);
            _connection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Retrieves the singleton instance of the <see cref="DBConnector"/> class.
        /// </summary>
        /// <returns>The singleton instance of <see cref="DBConnector"/>.</returns>
        public static DBConnector GetInstance()
        {
            lock (_lock)
            {
                return _instance ??= new DBConnector();
            }
        }

        /// <summary>
        /// Executes a non-query SQL command using the specified <see cref="DBCommand"/>.
        /// </summary>
        /// <param name="command">The command to be executed against the database.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> is null.</exception>
        public void Execute(DBCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null.");
            }

            if (command.UseNewConnection)
            {
                var connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();
                    connection.ExecuteAsync(
                        command.Sql,
                        param: command.Parameters,
                        commandType: command.CommandType,
                        transaction: command.Transaction,
                        commandTimeout: command.Timeout)
                        .GetAwaiter()
                        .GetResult();
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
            else
            {
                OpenConnection();
                _connection.ExecuteAsync(
                    command.Sql,
                    param: command.Parameters,
                    commandType: command.CommandType,
                    transaction: command.Transaction,
                    commandTimeout: command.Timeout)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        /// <summary>
        /// Executes a SQL query and returns the result set mapped to a collection of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the result set should be mapped.</typeparam>
        /// <param name="command">The command defining the query to be executed.</param>
        /// <returns>A collection of <typeparamref name="T"/> representing the query result.</returns>
        public IEnumerable<T> Query<T>(DBCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Command cannot be null.");
            }


            if (command.UseNewConnection)
            {
                var connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();
                    return connection.QueryAsync<T>(
                        command.Sql,
                        param: command.Parameters,
                        commandType: command.CommandType,
                        transaction: command.Transaction,
                        commandTimeout: command.Timeout)
                        .GetAwaiter()
                        .GetResult();
                }
                catch
                {
                    connection.Close();
                    throw;
                }
            }
            else
            {
                OpenConnection();
                return _connection.QueryAsync<T>(
                    command.Sql,
                    param: command.Parameters,
                    commandType: command.CommandType,
                    transaction: command.Transaction,
                    commandTimeout: command.Timeout)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        /// <summary>
        /// Begins and returns a new database transaction on the existing connection.
        /// </summary>
        /// <returns>An <see cref="IDbTransaction"/> representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            OpenConnection();
            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Opens the database connection if it is not already open.
        /// </summary>
        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// Closes the database connection if it is currently open.
        /// </summary>
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
