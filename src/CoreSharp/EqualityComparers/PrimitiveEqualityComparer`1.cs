using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreSharp.EqualityComparers;

/// <summary>
/// Defines methods to support the comparison of objects for equality.
/// Uses only primitive types for comparisons and hashes.
/// </summary>
public class PrimitiveEqualityComparer<TEntity> : IEqualityComparer<TEntity>
    where TEntity : class
{
    // Methods 
    public bool Equals(TEntity x, TEntity y)
    {
        // Same reference 
        if (x == y)
        {
            return true;
        }

        // Null 
        else if (x is null || y is null)
        {
            return false;
        }

        var leftProperties = GetPrimitiveProperties(x);
        var rightProperties = GetPrimitiveProperties(y);
        var dictionaryComparer = new DictionaryEqualityComparer<string, object>();
        return dictionaryComparer.Equals(leftProperties, rightProperties);
    }

    public int GetHashCode(TEntity obj)
    {
        if (obj is null)
        {
            return default(TEntity).GetHashCode();
        }

        var primitiveValues = GetPrimitiveProperties(obj).Select(p => p.Value);
        var enumerableComparer = new EnumerableEqualityComparer<object>();
        return enumerableComparer.GetHashCode(primitiveValues);
    }

    /// <inheritdoc cref="Type.IsPrimitive"/>
    protected virtual bool IsTypePrimitive(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IsPrimitiveExtended();
    }

    /// <summary>
    /// Get key-value <see cref="IDictionary{TKey, TValue}"/>
    /// of primitive types only.
    /// </summary>
    private IDictionary<string, object> GetPrimitiveProperties(TEntity item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return item.GetType()
                   .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Where(p => p.CanRead && IsTypePrimitive(p.PropertyType))
                   .ToDictionary(p => p.Name, p => p.GetValue(item));
    }
}
