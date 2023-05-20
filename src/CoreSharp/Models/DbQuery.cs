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
public class DbQuery : IAsyncDisposable
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
    }

    public DbQuery(DbTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        _ = transaction?.Connection
            ?? throw new ArgumentException($"{nameof(transaction)}.{nameof(transaction.Connection)} cannot be null.", nameof(transaction));

        _transaction = transaction;
        _connection = transaction?.Connection;
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
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }

    /// <inheritdoc cref="DbParameterCollection.Add(object)"/>
    public void AddParameter(string name, object value)
    {
        var parameter = _connection.CreateParameter(name, value);
        Parameters.Add(parameter);
    }

    private async Task<DbCommand> BuildDbCommandAsync(string query, CancellationToken cancellationToken = default)
    {
        // Open connection
        if (!_connection.IsOpen())
        {
            await _connection.OpenAsync(cancellationToken);
        }

        // Prepare and execute DbCommand 
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

    /// <inheritdoc cref="DbCommand.ExecuteNonQueryAsync(CancellationToken)"/>
    public async Task<int> ExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default)
    {
        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <inheritdoc cref="DbCommand.ExecuteScalarAsync(CancellationToken)"/>
    public async Task<TResult> ExecuteScalarAsync<TResult>(string query, CancellationToken cancellationToken = default)
    {
        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        return (TResult)await command.ExecuteScalarAsync(cancellationToken);
    }

    /// <inheritdoc cref="DataTable.Load(IDataReader)"/>
    public async Task<int> FillAsync(string query, DataTable table, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(table);

        await using var command = await BuildDbCommandAsync(query, cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        table.Load(reader);
        return table.Rows.Count;
    }

    /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
    public async Task<int> FillAsync(string query, DataSet set)
    {
        var mappings = Enumerable.Empty<DataTableMapping>();
        return await FillAsync(query, set, mappings);
    }

    /// <inheritdoc cref="FillAsync(string, DataSet, IEnumerable{DataTableMapping}, CancellationToken)"/>
    public async Task<int> FillAsync(string query, DataSet set, params DataTableMapping[] tableMappings)
        => await FillAsync(query, set, tableMappings, CancellationToken.None);

    /// <inheritdoc cref="DbDataAdapter.Fill(DataSet)"/>
    public async Task<int> FillAsync(string query, DataSet set, IEnumerable<DataTableMapping> tableMappings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(set);
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

            return await Task.Run(() => adapter.Fill(set), cancellationToken);
        }
        finally
        {
            if (adapter is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }

            if (adapter is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
