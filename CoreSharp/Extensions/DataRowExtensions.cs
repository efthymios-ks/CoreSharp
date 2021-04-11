using System;
using System.Collections.Generic;
using System.Data;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// DataRow extensions.
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// Get DataRow column names.
        /// </summary>
        public static IEnumerable<string> GetColumnNames(this DataRow row)
        {
            var table = row?.Table;
            return table.GetColumnNames();
        }

        /// <summary>
        /// Get column values.
        /// </summary> 
        public static IEnumerable<object> GetColumnValues(this DataRow row)
        {
            row = row ?? throw new ArgumentNullException(nameof(row));

            var values = row.ItemArray;
            return values;
        }
    }
}
