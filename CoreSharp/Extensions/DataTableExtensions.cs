using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DataTable extensions. 
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Get DataTable column names.
        /// </summary>
        public static IEnumerable<string> GetColumnNames(this DataTable table)
        {
            table = table ?? throw new ArgumentNullException(nameof(table));

            var columns = table.Columns.Cast<DataColumn>();
            var names = columns.Select(x => x.ColumnName);
            return names;
        }

        /// <summary>
        /// Map DataTable values TEntity collection. 
        /// </summary>
        public static IEnumerable<TEntity> MapTo<TEntity>(this DataTable table, bool ignoreCase = false) where TEntity : new()
        {
            table = table ?? throw new ArgumentNullException(nameof(table));

            return table.MapToInternal<TEntity>(ignoreCase);
        }

        private static IEnumerable<TEntity> MapToInternal<TEntity>(this DataTable table, bool ignoreCase = false) where TEntity : new()
        {
            foreach (DataRow row in table.Rows)
                yield return row.MapTo<TEntity>(ignoreCase);
        }
    }
}
