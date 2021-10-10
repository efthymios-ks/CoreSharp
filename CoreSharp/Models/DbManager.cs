using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Models
{
    /// <summary>
    /// An extension to <see cref="DbConnection"/> to run quick actions on it.
    /// </summary>
    public class DbManager : IDisposable
    {
        //Fields 
        private readonly DbConnection _connection;
        private readonly DbTransaction _transaction;
        private int _timeoutSeconds = 0;

        //Constructors 
        public DbManager(DbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public DbManager(DbTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = transaction?.Connection ?? throw new ArgumentException($"{nameof(transaction)}.{nameof(transaction.Connection)} cannot be null.", nameof(transaction));
        }

        //Properties 
        /// <summary>
        /// Gets or sets the wait time (in seconds) before terminating
        /// the attempt to execute a <see cref="DbCommand.CommandTimeout"/> and generating an error.
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
        /// Gets or sets a value indicating how the <see cref="DbCommand.CommandText"/> property is to be interpreted.
        /// </summary>
        public CommandType QueryType { get; set; } = CommandType.Text;

        /// <summary>
        /// Represents a collection of parameters associated with a <see cref="DbCommand"/>
        /// and their respective mappings to columns in a DataSet.
        /// </summary>
        public ICollection<DbParameter> Parameters { get; } = new HashSet<DbParameter>();

        //Methods 
        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _connection?.Dispose();
        }

        /// <inheritdoc cref="DbParameterCollection.Add(object)"/>
        public void AddParameter(string name, object value)
        {
            var parameter = _connection.CreateParameter(name, value);
            Parameters.Add(parameter);
        }

        private async Task<DbCommand> GetCommandAsync(string query, CancellationToken cancellationToken = default)
        {
            //Open connection
            if (!_connection.IsOpen())
                await _connection.OpenAsync(cancellationToken);

            //Prepare and execute DbCommand 
            var command = _connection.CreateCommand();
            command.Connection = _connection;
            command.Transaction = _transaction;
            command.CommandTimeout = TimeoutSeconds;
            command.CommandType = QueryType;
            command.CommandText = query;

            if (Parameters.Any() is true)
                command.Parameters.AddRange(Parameters.ToArray());

            return command;
        }

        /// <inheritdoc cref="DbCommand.ExecuteNonQuery"/>
        public int ExecuteNonQuery(string query)
        {
            Task<int> action() => ExecuteNonQueryAsync(query);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="DbCommand.ExecuteNonQueryAsync(CancellationToken)"/>
        public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default)
        {
            using var command = await GetCommandAsync(query, cancellationToken);
            return await command.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <inheritdoc cref="DbCommand.ExecuteScalar"/>
        public T ExecuteScalar<T>(string query)
        {
            Task<T> action() => ExecuteScalarAsync<T>(query);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="DbCommand.ExecuteScalarAsync(CancellationToken)"/>
        public async Task<T> ExecuteScalarAsync<T>(string query, CancellationToken cancellationToken = default)
        {
            using var command = await GetCommandAsync(query, cancellationToken);
            return (T)await command.ExecuteScalarAsync(cancellationToken);
        }

        /// <inheritdoc cref="FillAsync(string, DataTable, CancellationToken)"/>
        public int Fill(string query, DataTable table)
        {
            Task<int> action() => FillAsync(query, table);
            return Task.Run(action).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="DataTable.Load(IDataReader)"/>
        public async Task<int> FillAsync(string query, DataTable table, CancellationToken cancellationToken = default)
        {
            _ = table ?? throw new ArgumentNullException(nameof(table));

            using var command = await GetCommandAsync(query, cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            table.Load(reader);
            return table.Rows.Count;
        }

        /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
        public int Fill(string query, DataSet set)
        {
            var mappings = Enumerable.Empty<DataTableMapping>();
            return Fill(query, set, mappings);
        }

        /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
        public int Fill(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings)
            => Fill(query, set, tableMappings?.ToArray());

        /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
        public int Fill(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            Task<int> Action() => FillAsync(query, set, tableMappings);
            return Task.Run(Action).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
        public async Task<int> FillAsync(string query, DataSet set)
        {
            var mappings = Enumerable.Empty<DataTableMapping>();
            return await FillAsync(query, set, mappings);
        }

        /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
        public async Task<int> FillAsync(string query, DataSet set, params DataTableMapping[] tableMappings)
        {
            _ = set ?? throw new ArgumentNullException(nameof(set));
            _ = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            return await FillAsync(query, set, tableMappings as IEnumerable<DataTableMapping>);
        }

        /// <inheritdoc cref="DbDataAdapter.Fill(DataSet)"/>
        public async Task<int> FillAsync(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings, CancellationToken cancellationToken = default)
        {
            _ = set ?? throw new ArgumentNullException(nameof(set));
            _ = tableMappings ?? throw new ArgumentNullException(nameof(tableMappings));

            using var command = await GetCommandAsync(query, cancellationToken);
            var adapter = _connection.CreateDataAdapter();
            try
            {
                adapter.SelectCommand = command;
                if (tableMappings?.Any() is true)
                    adapter.TableMappings.AddRange(tableMappings?.ToArray());

                return await Task.Run(() => adapter.Fill(set), cancellationToken);
            }
            finally
            {
                if (adapter is IDisposable disposable)
                    disposable?.Dispose();
            }
        }
    }
}
