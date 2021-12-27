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

            return DictionariesEquals(leftProperties, rightProperties);
        }

        public int GetHashCode(TEntity item)
        {
            if (item is null)
                return 0;

            var hash = new HashCode();
            var primitiveValues = GetPrimitiveProperties(item).Select(p => p.Value);
            foreach (var value in primitiveValues)
                hash.Add(value);
            return hash.ToHashCode();
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

        /// <summary>
        /// Compare two <see cref="IDictionary{TKey, TValue}"/>
        /// for equality based on keys and values.
        /// </summary>
        private static bool DictionariesEquals<TValue>(IDictionary<string, TValue> leftDictionary, IDictionary<string, TValue> rightDictionary)
        {
            _ = leftDictionary ?? throw new ArgumentNullException(nameof(leftDictionary));
            _ = rightDictionary ?? throw new ArgumentNullException(nameof(rightDictionary));

            if (leftDictionary.Count != rightDictionary.Count)
                return false;

            foreach (var leftPair in leftDictionary)
            {
                //Key doesn't exist 
                if (!rightDictionary.TryGetValue(leftPair.Key, out var rightValue))
                    return false;

                //Different value
                if (!Equals(leftPair.Value, rightValue))
                    return false;
            }

            return true;
        }
    }
}
