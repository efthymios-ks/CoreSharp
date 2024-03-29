﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="DataTable"/> extensions.
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// Get <see cref="DataColumn.ColumnName"/> list
    /// from <see cref="DataTable"/>.
    /// </summary>
    public static IEnumerable<string> GetColumnNames(this DataTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var columns = table.Columns.Cast<DataColumn>();
        return columns.Select(c => c.ColumnName);
    }

    /// <summary>
    /// Map <see cref="DataTable"/> values provided type collection.
    /// </summary>
    public static IEnumerable<TEntity> ToEntities<TEntity>(this DataTable table)
        where TEntity : class, new()
    {
        ArgumentNullException.ThrowIfNull(table);

        return table.ToEntitiesInternal<TEntity>();
    }

    private static IEnumerable<TEntity> ToEntitiesInternal<TEntity>(this DataTable table)
        where TEntity : class, new()
        => from DataRow row in table.Rows select row.ToEntity<TEntity>();
}
