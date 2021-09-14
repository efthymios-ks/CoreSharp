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
        private readonly DbConnection _connection;
        private readonly DbTransaction _transaction;
        private int _timeoutSeconds = 0;

        //Properties 
        /// <summary>
        /// Gets or sets the wait time (in seconds) before terminating
        /// the attempt to execute a command and generating an error.
        /// </summary>
        public int TimeoutSeconds
        {
            get => _timeoutSeconds;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TimeoutSeconds), $"{nameof(TimeoutSeconds)} ({value}) cannot have negative value.");
                _timeoutSeconds = value;
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
        public ICollection<DbParameter> Parameters { get; } = new HashSet<DbParameter>();

        //Constructors 
        public DbHelper(DbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public DbHelper(DbTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = transaction?.Connection ?? throw new ArgumentException($"{nameof(transaction)}.{nameof(transaction.Connection)} cannot be null.", nameof(transaction));
        }

        //Methods 
        /// <summary>
        /// Performs application-defined tasks associated with
        /// freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _connection?.Dispose();
        }

        /// <summary>
        /// Adds a SqlParameter to the SqlParameterCollection.
        /// </summary>
        public void AddParameter(string name, object value)
        {
            var parameter = _connection.CreateParameter(name, value);
            Parameters.Add(parameter);
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and
        /// returns the number of rows affected.
        /// </summary>
        public int ExecuteNonQuery(string query)
        {
            Task<int> Action() => ExecuteNonQueryAsync(query);
            return Task.Run(Action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and
        /// returns the number of rows affected.
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            //DbConnection Open 
            if (!_connection.IsOpen())
                await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            //Prepare and execute DbCommand 
            await using var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            return rowsAffected;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first
        /// row in the result set returned by the query.
        /// Additional columns or rows are ignored.
        /// </summary>
        public T ExecuteScalar<T>(string query)
        {
            Task<T> Action() => ExecuteScalarAsync<T>(query);
            return Task.Run(Action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first
        /// row in the result set returned by the query.
        /// Additional columns or rows are ignored.
        /// </summary>
        public async Task<T> ExecuteScalarAsync<T>(string query, CancellationToken cancellationToken = default)
        {
            //DbConnection Open 
            if (!_connection.IsOpen())
                await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            //Prepare and execute DbCommand 
            await using var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            return (T)await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataTable to match those in the data source.
        /// </summary>
        public int Fill(string query, DataTable table)
        {
            Task<int> Action() => FillAsync(query, table);
            return Task.Run(Action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adds or refreshes rows in the DataTable to match those in the data source.
        /// </summary>
        public async Task<int> FillAsync(string query, DataTable table, CancellationToken cancellationToken = default)
        {
            _ = table ?? throw new ArgumentNullException(nameof(table));

            //DbConnection Open
            if (!_connection.IsOpen())
                await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            //Prepare and execute DbCommand
            await using var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                table.Load(reader);

            var rowsAffected = table.Rows.Count;
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
            => Fill(query, set, tableMappings?.ToArray());

        /// <summary>
        /// Adds or refreshes rows in the DataSet
        /// to match those in the data source.
        /// </summary>
        public int Fill(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            Task<int> Action() => FillAsync(query, set, tableMappings);
            return Task.Run(Action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet
        /// to match those in the data source.
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set)
        {
            var mappings = Enumerable.Empty<DataTableMapping>();
            return await FillAsync(query, set, mappings).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet
        /// to match those in the data source.
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            _ = set ?? throw new ArgumentNullException(nameof(set));
            _ = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            return await FillAsync(query, set, tableMappings as IEnumerable<DataTableMapping>).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet
        /// to match those in the data source.
        /// </summary>
        public async Task<int> FillAsync(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings, CancellationToken cancellationToken = default)
        {
            _ = set ?? throw new ArgumentNullException(nameof(set));
            _ = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            //DbConnection Open 
            if (!_connection.IsOpen())
                await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            //Prepare DbCommand 
            await using var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (!Parameters.IsNullOrEmpty())
                command.Parameters.AddRange(Parameters.ToArray());

            //Prepare and execute DbDataAdapter
            var adapter = _connection.CreateDataAdapter();
            try
            {
                adapter.SelectCommand = command;

                var tableMappingsArray = tableMappings?.ToArray();
                if (tableMappingsArray is not null)
                    adapter.TableMappings.AddRange(tableMappingsArray);

                var rowsAffected = await Task.Run(() => adapter.Fill(set), cancellationToken).ConfigureAwait(false);
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
