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
        _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
        _valueComparer = valueComparer ?? throw new ArgumentNullException(nameof(valueComparer));
    }

    // Methods
    public bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
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

        // Keys don't match
        var keysComparer = new EnumerableEqualityComparer<TKey>(_keyComparer);
        if (!keysComparer.Equals(x.Keys, y.Keys))
        {
            return false;
        }

        // Values don't match 
        foreach (var (key, value) in x)
        {
            var rightValue = y[key];
            if (!_valueComparer.Equals(value, rightValue))
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

        var hash = new HashCode();
        foreach (var (_, value) in obj)
        {
            hash.Add(value, _valueComparer);
        }

        return hash.ToHashCode();
    }
}
