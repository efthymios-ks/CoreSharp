using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Generic extensions. 
    /// </summary>
    public static partial class GenericExtensions
    {
        /// <summary>
        /// Check if value is contained in list. 
        /// </summary>
        public static bool IsIn<T>(this T item, IEnumerable<T> source)
        {
            return item.IsIn(source?.ToArray());
        }

        /// <summary>
        /// Check if value is contained in list. 
        /// </summary>
        public static bool IsIn<T>(this T item, params T[] source)
        {
            item = item ?? throw new ArgumentNullException(nameof(item));
            source = source ?? throw new ArgumentNullException(nameof(source));

            return source.Contains(item);
        }

        /// <summary>
        /// Get property name. 
        /// </summary>
        public static string GetPropertyName<TItem, TProperty>(this TItem item, Expression<Func<TItem, TProperty>> propertySelector)
        {
            propertySelector = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));

            var body = (MemberExpression)propertySelector.Body;
            return body.Member.Name;
        }

        /// <summary> 
        /// Perform a deep copy using Json serialization. 
        /// </summary> 
        public static TEntity JsonClone<TEntity>(this TEntity item, JsonSerializerOptions options = null) where TEntity : class
        {
            item = item ?? throw new ArgumentNullException(nameof(item));

            options ??= new JsonSerializerOptions()
            {
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(item, options);
            return JsonSerializer.Deserialize<TEntity>(json, options);
        }

        /// <summary>
        /// Check if class is null. 
        /// </summary>
        public static bool IsNull<T>(this T input) where T : class
        {
            return input == null;
        }

        /// <summary>
        /// Gets a value indicating whether the current nullable 
        /// object has a valid value of its underlying type. 
        /// </summary>
        public static bool IsNull<T>(this T? input) where T : struct
        {
            return !input.HasValue;
        }

        /// <summary>
        /// Check if struct has default value. 
        /// </summary>
        public static bool IsDefault<T>(this T input) where T : struct
        {
            var obj = default(T);
            return input.Equals(obj);
        }
    }
}
