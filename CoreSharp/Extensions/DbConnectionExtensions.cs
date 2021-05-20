using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    //TODO: Finish unit tests for DbConnection. 
    /// <summary>
    /// DbConnection extensions. 
    /// </summary>
    public static partial class DbConnectionExtensions
    {
        /// <summary>
        /// Return a new instance of the provider's class that implements the DbDataAdapter class.
        /// </summary>
        public static DbDataAdapter CreateDataAdapter(this DbConnection connection)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            var factory = DbProviderFactories.GetFactory(connection);
            return factory.CreateDataAdapter();
        }

        /// <summary>
        /// Return a new instance of the provider's class that implements the DbParameter class. 
        /// </summary>
        public static DbParameter CreateParameter(this DbConnection connection, string parameterName, object parameterValue)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            var factory = DbProviderFactories.GetFactory(connection);
            return factory.CreateParameter(parameterName, parameterValue);
        }

        /// <summary>
        /// Try to open the connection and begin a new transaction on it. 
        /// </summary>
        public static DbTransaction OpenTransaction(this DbConnection connection)
        {
            return connection.OpenTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Try to open the connection and begin a new transaction on it. 
        /// </summary>
        public static DbTransaction OpenTransaction(this DbConnection connection, IsolationLevel isolationLevel)
        {
            return connection
                .OpenTransactionAsync(isolationLevel)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Try to open the connection and begin a new transaction on it. 
        /// </summary>
        public static async Task<DbTransaction> OpenTransactionAsync(this DbConnection connection)
        {
            return await connection.OpenTransactionAsync(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Try to open the connection and begin a new transaction on it. 
        /// </summary>
        public static async Task<DbTransaction> OpenTransactionAsync(this DbConnection connection, IsolationLevel isolationLevel)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            if (!connection.IsOpen())
                await connection.OpenAsync();

            return connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Check if connection is open. 
        /// </summary>
        public static bool IsOpen(this DbConnection connection)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            return connection.State.HasFlag(ConnectionState.Open);
        }

        /// <summary>
        /// Check if connection is available. 
        /// </summary>
        public static bool IsAvailable(this DbConnection connection)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            return connection
                    .IsAvailableAsync()
                    .GetAwaiter()
                    .GetResult();
        }

        /// <summary>
        /// Check if connection is available. 
        /// </summary>
        public static async Task<bool> IsAvailableAsync(this DbConnection connection)
        {
            connection = connection ?? throw new ArgumentNullException(nameof(connection));

            try
            {
                await connection.OpenAsync();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
