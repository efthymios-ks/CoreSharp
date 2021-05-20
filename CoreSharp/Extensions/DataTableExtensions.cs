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
            return columns.Select(x => x.ColumnName);
        }

        /// <summary>
        /// Map DataTable values to TEntity collection. 
        /// </summary>
        public static IEnumerable<TEntity> ToEntities<TEntity>(this DataTable table, bool ignoreCase = true) where TEntity : new()
        {
            table = table ?? throw new ArgumentNullException(nameof(table));

            return table.ToEntitiesInternal<TEntity>(ignoreCase);
        }

        private static IEnumerable<TEntity> ToEntitiesInternal<TEntity>(this DataTable table, bool ignoreCase) where TEntity : new()
        {
            foreach (DataRow row in table.Rows)
                yield return row.ToEntity<TEntity>(ignoreCase);
        }
    }
}
