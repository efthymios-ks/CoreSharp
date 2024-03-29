﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="DataRow"/> extensions.
/// </summary>
public static class DataRowExtensions
{
    /// <summary>
    /// Get <see cref="DataColumn.Namespace"/>
    /// from <see cref="DataRow"/>.
    /// </summary>
    public static IEnumerable<string> GetColumnNames(this DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        return row.Table.GetColumnNames();
    }

    /// <summary>
    /// Get <see cref="DataRow"/> column values.
    /// </summary>
    public static object[] GetColumnValues(this DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        return row.ItemArray;
    }

    /// <summary>
    /// Map <see cref="DataRow"/> values to provided type.
    /// </summary>
    public static TEntity ToEntity<TEntity>(this DataRow row)
        where TEntity : class, new()
    {
        ArgumentNullException.ThrowIfNull(row);

        var columnNames = row.Table.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
        var properties = columnNames.ToDictionary(columnName => columnName, column => row[column]);
        return properties.ToEntity<TEntity>();
    }
}
