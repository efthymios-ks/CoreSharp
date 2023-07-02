using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Models;

/// <summary>
/// An extension to <see cref="DbConnection"/> to run quick actions on it.
/// </summary>
public sealed class DbQuery
{
    // Fields 
    private readonly DbConnection _connection;
    private readonly DbTransaction _transaction;
    private int _timeoutSeconds;

    // Constructors 
    public DbQuery(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        _connection = connection;
        _timeoutSeconds = _connection.ConnectionTimeout;
    }

    public DbQuery(DbTransaction transaction)
        : this(transaction?.Connection)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        _transaction = transaction;
    }

    // Properties 
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
            {
                throw new ArgumentOutOfRangeException(nameof(TimeoutSeconds), $"{nameof(TimeoutSeconds)} ({value}) cannot have negative value.");
            }

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

    // Methods 
    /// <summary>
    /// Adds the specified <see cref="DbParameter"/> object to the <see cref="DbParameterCollection"/>.
    /// </summary>
    public void AddParameter(string parameterName, object parameterValue)
    {
        var parameter = _connection.CreateParameter(parameterName, parameterValue);
        Parameters.Add(parameter);
    }

    /// <summary>
    /// Executes the command against its connection object, 
    /// returning the number of rows affected.
    /// </summary>
    public async Task<int> ExecuteAsync(string query, CancellationToken cancellationToken = default)
    {
        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Executes the command and returns the first column of the first row in the first returned result set.
    /// All other columns, rows and result sets are ignored.
    /// </summary>
    public async Task<TResult> ExecuteAsync<TResult>(string query, CancellationToken cancellationToken = default)
    {
        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        return (TResult)await command.ExecuteScalarAsync(cancellationToken);
    }

    /// <summary>
    /// Fills a <see cref="DataTable"/> with values from a data source using the <see cref="DbDataReader"/>.
    /// If the <see cref="DataTable"/> already contains rows, the incoming data from the data source is merged with the existing rows.
    /// </summary>
    public async Task<int> FillAsync(string query, DataTable dataTable, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dataTable);

        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        dataTable.Load(reader);
        return dataTable.Rows.Count;
    }

    /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
    public async Task<int> FillAsync(string query, DataSet dataSet, CancellationToken cancellationToken = default)
    {
        var mappings = Enumerable.Empty<DataTableMapping>();
        return await FillAsync(query, dataSet, mappings, cancellationToken);
    }

    /// <summary>
    /// Adds or refreshes rows in the DataSet.
    /// </summary>
    public async Task<int> FillAsync(string query, DataSet dataSet, IEnumerable<DataTableMapping> tableMappings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dataSet);
        ArgumentNullException.ThrowIfNull(tableMappings);

        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        var adapter = _connection.CreateDataAdapter();

        try
        {
            adapter.SelectCommand = command;

            if (tableMappings.Any())
            {
                adapter.TableMappings.AddRange(tableMappings.ToArray());
            }

            return adapter.Fill(dataSet);
        }
        finally
        {
            if (adapter is IAsyncDisposable adapterAsAsyncDisposable)
            {
                await adapterAsAsyncDisposable.DisposeAsync();
            }

            if (adapter is IDisposable adapterAsDisposable)
            {
                adapterAsDisposable.Dispose();
            }
        }
    }

    private async Task<DbCommand> BuildDbCommandAsync(string query, CancellationToken cancellationToken = default)
    {
        // Open connection
        if (!_connection.IsOpen())
        {
            await _connection.OpenAsync(cancellationToken);
        }

        // Prepare DbCommand 
        var command = _connection.CreateCommand();
        command.Connection = _connection;
        command.Transaction = _transaction;
        command.CommandTimeout = TimeoutSeconds;
        command.CommandType = QueryType;
        command.CommandText = query;

        if (Parameters.Count > 0)
        {
            command.Parameters.AddRange(Parameters.ToArray());
        }

        return command;
    }
}
