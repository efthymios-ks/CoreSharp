using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    //TODO: Finish unit tests for DbConnection. 
    /// <summary>
    /// <see cref="DbConnection"/> extensions.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <inheritdoc cref="DbProviderFactory.CreateDataAdapter"/>
        public static DbDataAdapter CreateDataAdapter(this DbConnection connection)
        {
            _ = connection ?? throw new ArgumentNullException(nameof(connection));

            var factory = DbProviderFactories.GetFactory(connection);
            return factory?.CreateDataAdapter();
        }

        /// <inheritdoc cref="DbProviderFactoryExtensions.CreateParameter(DbProviderFactory, string, object)" />
        public static DbParameter CreateParameter(this DbConnection connection, string parameterName, object parameterValue)
        {
            _ = connection ?? throw new ArgumentNullException(nameof(connection));

            var factory = DbProviderFactories.GetFactory(connection);
            return factory.CreateParameter(parameterName, parameterValue);
        }

        /// <inheritdoc cref="OpenTransactionAsync(DbConnection, IsolationLevel)" />
        public static DbTransaction OpenTransaction(this DbConnection connection)
            => connection.OpenTransaction(IsolationLevel.ReadCommitted);

        /// <inheritdoc cref="OpenTransactionAsync(DbConnection, IsolationLevel)" />
        public static DbTransaction OpenTransaction(this DbConnection connection, IsolationLevel isolationLevel)
            => connection
                .OpenTransactionAsync(isolationLevel)
                .GetAwaiter()
                .GetResult();

        /// <inheritdoc cref="OpenTransactionAsync(DbConnection, IsolationLevel)" />
        public static async Task<DbTransaction> OpenTransactionAsync(this DbConnection connection)
            => await connection.OpenTransactionAsync(IsolationLevel.ReadCommitted);

        /// <summary>
        /// Try to open connection and begin a new transaction on it.
        /// </summary>
        public static async Task<DbTransaction> OpenTransactionAsync(this DbConnection connection, IsolationLevel isolationLevel)
        {
            _ = connection ?? throw new ArgumentNullException(nameof(connection));

            if (!connection.IsOpen())
                await connection.OpenAsync();

            return await connection.BeginTransactionAsync(isolationLevel);
        }

        /// <summary>
        /// Check <see cref="DbConnection.State"/> for <see cref="ConnectionState.Open"/>.
        /// </summary>
        public static bool IsOpen(this DbConnection connection)
        {
            _ = connection ?? throw new ArgumentNullException(nameof(connection));

            return connection.State.HasFlag(ConnectionState.Open);
        }

        /// <inheritdoc cref="IsAvailableAsync(DbConnection, CancellationToken)" />
        public static bool IsAvailable(this DbConnection connection)
            => connection
                    .IsAvailableAsync()
                    .GetAwaiter()
                    .GetResult();

        /// <summary>
        /// Chain calls <see cref="DbConnection.OpenAsync(System.Threading.CancellationToken)"/> and <see cref="DbConnection.CloseAsync"/>.
        /// </summary>
        public static async Task<bool> IsAvailableAsync(this DbConnection connection, CancellationToken cancellationToken = default)
        {
            _ = connection ?? throw new ArgumentNullException(nameof(connection));

            try
            {
                await connection.OpenAsync(cancellationToken);
                await connection.CloseAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
