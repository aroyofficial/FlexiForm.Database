using System.Data;

namespace FlexiForm.Database.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDbTransaction"/>.
    /// </summary>
    public static class TransactionExtensions
    {
        /// <summary>
        /// Determines whether the specified <see cref="IDbTransaction"/> is currently usable.
        /// </summary>
        /// <param name="transaction">The database transaction to check.</param>
        /// <returns>
        /// <c>true</c> if the transaction is not null, its associated connection is not null,
        /// and the connection state is <see cref="ConnectionState.Open"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUsable(this IDbTransaction transaction)
        {
            return transaction != null &&
                   transaction.Connection != null &&
                   transaction.Connection.State == ConnectionState.Open;
        }
    }
}
