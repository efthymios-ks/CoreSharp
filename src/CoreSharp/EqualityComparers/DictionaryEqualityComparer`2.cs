using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.EqualityComparers;

public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEqualityComparer<TKey> _keyComparer;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEqualityComparer<TValue> _valueComparer;

    // Constructors 
    public DictionaryEqualityComparer()
        : this(EqualityComparer<TKey>.Default, EqualityComparer<TValue>.Default)
    {
    }

    public DictionaryEqualityComparer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
    {
        ArgumentNullException.ThrowIfNull(keyComparer);
        ArgumentNullException.ThrowIfNull(valueComparer);

        _keyComparer = keyComparer;
        _valueComparer = valueComparer;
    }

    // Methods
    public bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out var value) || !_valueComparer.Equals(kvp.Value, value))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        if (obj is null)
        {
            return _valueComparer.GetHashCode(default);
        }

        var hashCode = 0;
        foreach (var (key, value) in obj)
        {
            var keyHashCode = GetKeyHashCode(key);
            var valueHashCode = GetValueHashCode(value);

            hashCode ^= keyHashCode;
            hashCode ^= valueHashCode;
        }

        return hashCode;

        int GetKeyHashCode(TKey key)
            => key == null ? 0 : _keyComparer.GetHashCode(key);

        int GetValueHashCode(TValue value)
            => value == null ? 0 : _valueComparer.GetHashCode(value);
    }
}
