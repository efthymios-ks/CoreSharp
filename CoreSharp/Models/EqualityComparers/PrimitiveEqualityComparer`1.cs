using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.Models.EqualityComparers
{
    /// <summary>
    /// Defines methods to support the comparison of objects for equality.
    /// Uses only primitive types for comparisons and hashes.
    /// </summary>
    public class PrimitiveEqualityComparer<TEntity> : IEqualityComparer<TEntity>
        where TEntity : class
    {
        //Methods 
        public bool Equals(TEntity left, TEntity right)
        {
            if (left is null && right is not null)
                return false;
            else if (left is not null && right is null)
                return false;
            else if (left is null && right is null)
                return true;

            var leftProperties = GetPrimitiveProperties(left);
            var rightProperties = GetPrimitiveProperties(right);
            var dictionaryComparer = new DictionaryEqualityComparer<string, object>();
            return dictionaryComparer.Equals(leftProperties, rightProperties);
        }

        public int GetHashCode(TEntity entity)
        {
            if (entity is null)
                return default(TEntity).GetHashCode();

            var primitiveValues = GetPrimitiveProperties(entity).Select(p => p.Value);
            var enumerableComparer = new EnumerableEqualityComparer<object>();
            return enumerableComparer.GetHashCode(primitiveValues);
        }

        /// <inheritdoc cref="Type.IsPrimitive"/>
        protected virtual bool IsTypePrimitive(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));

            return type.IsPrimitiveExtended();
        }

        /// <summary>
        /// Get key-value <see cref="IDictionary{TKey, TValue}"/>
        /// of primitive types only.
        /// </summary>
        private IDictionary<string, object> GetPrimitiveProperties(TEntity item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            return item.GetType()
                       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(p => p.CanRead && IsTypePrimitive(p.PropertyType))
                       .ToDictionary(p => p.Name, p => p.GetValue(item));
        }
    }
}
