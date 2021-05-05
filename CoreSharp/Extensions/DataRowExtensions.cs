using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            row = row ?? throw new ArgumentNullException(nameof(row));

            return row?.Table.GetColumnNames();
        }

        /// <summary>
        /// Get column values.
        /// </summary> 
        public static IEnumerable<object> GetColumnValues(this DataRow row)
        {
            row = row ?? throw new ArgumentNullException(nameof(row));

            return row.ItemArray;
        }

        /// <summary>
        /// Map DataRow values to TEntity.
        /// </summary>
        public static TEntity MapTo<TEntity>(this DataRow row, bool ignoreCase = false) where TEntity : new()
        {
            row = row ?? throw new ArgumentNullException(nameof(row));

            //Object setup 
            var result = new TEntity();
            Type impliedType = typeof(TEntity);
            var properties = impliedType.GetProperties();
            var fields = impliedType.GetFields();

            //DataTable setup 
            var columnNames = row.GetColumnNames();

            //Scan Properties
            foreach (var property in properties)
            {
                //Check if Property exists  
                var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.CurrentCulture;
                var foundColumnName = columnNames.FirstOrDefault(i => i.Equals(property.Name, comparison));
                if (string.IsNullOrWhiteSpace(foundColumnName))
                    continue;

                //Get value 
                var value = row[foundColumnName] ?? DBNull.Value;

                //Extract useful flags 
                bool valueIsNull = value == DBNull.Value;
                bool propertyIsNullable = property.PropertyType.IsValueType && (Nullable.GetUnderlyingType(property.PropertyType) != null);
                bool propertyIsEnum = property.PropertyType.IsEnum;
                bool isSameType = property.PropertyType == value.GetType();
                bool valueIsAssignable = property.PropertyType.IsAssignableFrom(value.GetType());

                /*
                    With the following checks, 
                    1. Prevent null-related exceptions. 
                    2. Keep initial values from T class intact. 
                        *Setting a non-nullable variable to 'null', 
                         sets the variable value to 'default(T)'.
                */

                //Fix value 
                object finalValue = value;

                //Property=Nullable && Value=Null, because you cannot assign DBNull.Value. 
                if (propertyIsNullable && valueIsNull)
                    finalValue = null;

                //Property!=Nullable && Value=Null, to retain initial value. 
                else if (!propertyIsNullable && valueIsNull)
                    finalValue = property.GetValue(result);

                //Value!=Null && DifferentType && Property!=Enum, to convert (mostly numeric) variables. 
                else if ((!valueIsNull) && (!isSameType) && (!propertyIsEnum))
                    finalValue = Convert.ChangeType(value, property.PropertyType);

                property.SetValue(result, finalValue);
            }

            //Return 
            return result;
        }
    }
}
