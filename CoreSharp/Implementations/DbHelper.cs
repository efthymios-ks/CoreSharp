using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreSharp.Extensions;

namespace CoreSharp.Implementations
{
    /// <summary>
    /// An extension to DbConnection to run quick actions on it.
    /// </summary>
    public class DbHelper : IDisposable
    {
        //Fields 
        private readonly DbConnection connection;
        private readonly DbTransaction transaction;
        private int timeoutSeconds = 0;

        //Properties 
        /// <summary>
        /// Gets or sets the wait time (in seconds) before terminating 
        /// the attempt to execute a command and generating an error.
        /// </summary>
        public int TimeoutSeconds
        {
            get { return timeoutSeconds; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TimeoutSeconds), $"{nameof(TimeoutSeconds)} ({value}) cannot have negative value.");
                timeoutSeconds = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText property is to be interpreted.
        /// </summary>
        public CommandType QueryType { get; set; } = CommandType.Text;

        /// <summary>
        /// Represents a collection of parameters associated with a SqlCommand 
        /// and their respective mappings to columns in a DataSet. 
        /// </summary>
        public ICollection<DbParameter> Parameters { get; private set; } = new HashSet<DbParameter>();

        //Constructors 
        public DbHelper(DbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public DbHelper(DbTransaction transaction)
        {
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            connection = transaction?.Connection ?? throw new ArgumentException($"{nameof(transaction)}.{nameof(transaction.Connection)} cannot be null.", nameof(transaction));
        }

        //Methods 
        /// <summary>
        /// Performs application-defined tasks associated with 
        /// freeing, releasing, or resetting unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds a SqlParameter to the SqlParameterCollection.
        /// </summary>
        public void AddParameter(string name, object value)
        {
            var parameter = connection.CreateParameter(name, value);
            Parameters.Add(parameter);
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and 
        /// returns the number of rows affected.
        /// </summary>
        public int ExecuteNonQuery(string query)
        {
            Task<int> action() => ExecuteNonQueryAsync(query);
            return Task.Run(action).GetAwaiter().GetResult();

        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and 
        /// returns the number of rows affected.
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            //DbConnection Open 
            if (!connection.IsOpen())
                await connection.OpenAsync(cancellationToken);

            //Prepare and execute DbCommand 
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsAffected;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first 
        /// row in the result set returned by the query. 
        /// Additional columns or rows are ignored.
        /// </summary>
        public T ExecuteScalar<T>(string query)
        {
            Task<T> action() => ExecuteScalarAsync<T>(query);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first 
        /// row in the result set returned by the query. 
        /// Additional columns or rows are ignored.
        /// </summary>
        public async Task<T> ExecuteScalarAsync<T>(string query, CancellationToken cancellationToken = default)
        {
            //DbConnection Open 
            if (!connection.IsOpen())
                await connection.OpenAsync(cancellationToken);

            //Prepare and execute DbCommand 
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            return (T)await command.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataTable to match those in the data source.
        /// </summary>
        public int Fill(string query, DataTable table)
        {
            Task<int> action() => FillAsync(query, table);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adds or refreshes rows in the DataTable to match those in the data source.
        /// </summary>
        public async Task<int> FillAsync(string query, DataTable table, CancellationToken cancellationToken = default)
        {
            table = table ?? throw new ArgumentNullException(nameof(table));

            //DbConnection Open
            if (!connection.IsOpen())
                await connection.OpenAsync(cancellationToken);

            //Prepare and execute DbCommand
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                table.Load(reader);

            int rowsAffected = table.Rows.Count;
            return rowsAffected;
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public int Fill(string query, DataSet set)
        {
            var mappings = Enumerable.Empty<DataTableMapping>();
            return Fill(query, set, mappings);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public int Fill(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings)
        {
            return Fill(query, set, tableMappings?.ToArray());
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public int Fill(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            Task<int> action() => FillAsync(query, set, tableMappings);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set)
        {
            var mappings = Enumerable.Empty<DataTableMapping>();
            return await FillAsync(query, set, mappings);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            set = set ?? throw new ArgumentNullException(nameof(set));
            tableMappings = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            return await FillAsync(query, set, tableMappings as IEnumerable<DataTableMapping>);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet 
        /// to match those in the data source. 
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings, CancellationToken cancellationToken = default)
        {
            set = set ?? throw new ArgumentNullException(nameof(set));
            tableMappings = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            //DbConnection Open 
            if (!connection.IsOpen())
                await connection.OpenAsync(cancellationToken);

            //Prepare DbCommand 
            using var command = connection.CreateCommand();
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            //Prepare and execute DbDataAdapter
            var adapter = connection.CreateDataAdapter();
            try
            {
                adapter.SelectCommand = command;

                if (!tableMappings.IsNullOrEmpty())
                    adapter.TableMappings.AddRange(tableMappings.ToArray());

                int rowsAffected = await Task.Run(() => adapter.Fill(set));
                return rowsAffected;
            }
            finally
            {
                if (adapter is IDisposable disposable)
                    disposable?.Dispose();
            }
        }
    }
}
