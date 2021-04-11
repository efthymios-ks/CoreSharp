using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
