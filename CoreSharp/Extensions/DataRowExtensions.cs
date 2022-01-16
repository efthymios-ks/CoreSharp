using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CoreSharp.Extensions
{
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
            _ = row ?? throw new ArgumentNullException(nameof(row));

            return row.Table.GetColumnNames();
        }

        /// <summary>
        /// Get <see cref="DataRow"/> column values.
        /// </summary>
        public static IEnumerable<object> GetColumnValues(this DataRow row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            return row.ItemArray;
        }

        /// <summary>
        /// Map <see cref="DataRow"/> values to provided type.
        /// </summary>
        public static TEntity ToEntity<TEntity>(this DataRow row)
            where TEntity : class, new()
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            var columnNames = row.Table.Columns.Cast<DataColumn>().Select(c => c.ColumnName);
            var properties = columnNames.ToDictionary(column => column, column => row[column]);
            return properties.ToEntity<TEntity>();
        }
    }
}
