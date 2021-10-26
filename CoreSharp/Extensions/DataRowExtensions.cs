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
        /// Get DataRow column names.
        /// </summary>
        public static IEnumerable<string> GetColumnNames(this DataRow row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            return row.Table.GetColumnNames();
        }

        /// <summary>
        /// Get DataRow column values.
        /// </summary>
        public static IEnumerable<object> GetColumnValues(this DataRow row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            return row.ItemArray;
        }

        /// <summary>
        /// Map DataRow values to TEntity.
        /// </summary>
        public static TEntity ToEntity<TEntity>(this DataRow row, bool ignoreCase = false) where TEntity : new()
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));

            //Object setup 
            var result = new TEntity();
            var impliedType = typeof(TEntity);
            var properties = impliedType.GetProperties();

            //DataTable setup 
            var columnNames = row.GetColumnNames().ToArray();

            //Scan Properties
            foreach (var property in properties)
            {
                //Check if Property exists  
                var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture;
                var foundColumnName = Array.Find(columnNames, i => i.Equals(property.Name, comparison));
                if (string.IsNullOrWhiteSpace(foundColumnName))
                    continue;

                //Get value 
                var value = row[foundColumnName] ?? DBNull.Value;

                //Extract useful flags 
                var valueIsNull = value == DBNull.Value;
                var propertyIsNullable = property.PropertyType.IsValueType && (Nullable.GetUnderlyingType(property.PropertyType) is not null);
                var propertyIsEnum = property.PropertyType.IsEnum;
                var isSameType = property.PropertyType == value.GetType();

                /*
                    With the following checks, 
                    1. Prevent null-related exceptions. 
                    2. Keep initial values from T class intact. 
                        *Setting a non-nullable variable to 'null', 
                         sets the variable value to 'default(T)'.
                */

                //Fix value 
                var finalValue = value;

                //Property=Nullable && Value=Null, because you cannot assign DBNull.Value. 
                if (propertyIsNullable && valueIsNull)
                    finalValue = null;

                //Property!=Nullable && Value=Null, to retain initial value. 
                else if (!propertyIsNullable && valueIsNull)
                    finalValue = property.GetValue(result);

                //Value!=Null && DifferentType && Property!=Enum, to convert (mostly numeric) variables. 
                else if (!valueIsNull && !isSameType && !propertyIsEnum)
                    finalValue = Convert.ChangeType(value, property.PropertyType);

                property.SetValue(result, finalValue);
            }

            //Return 
            return result;
        }
    }
}
