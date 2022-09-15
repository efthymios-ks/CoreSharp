using System;
using System.Collections.Generic;

namespace CoreSharp.EqualityComparers;

public class KeyEqualityComparer<TEntity, TKey> : IEqualityComparer<TEntity>
{
    // Fields
    private readonly IEqualityComparer<TKey> _keyComparer;

    // Constructors
    public KeyEqualityComparer(Func<TEntity, TKey> keySelector)
        : this(keySelector, EqualityComparer<TKey>.Default)
    {
    }

    public KeyEqualityComparer(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
    {
        KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        _keyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
    }

    // Properties
    protected Func<TEntity, TKey> KeySelector { get; }

    // Methods 
    public bool Equals(TEntity x, TEntity y)
    {
        // Same reference 
        if (ReferenceEquals(x, y))
            return true;
        // Null 
        else if (x is null || y is null)
            return false;

        var leftKey = KeySelector(x);
        var rightKey = KeySelector(y);
        return _keyComparer.Equals(leftKey, rightKey);
    }

    public int GetHashCode(TEntity obj)
    {
        if (obj is null)
            return _keyComparer.GetHashCode(default);

        var key = KeySelector(obj);
        return _keyComparer.GetHashCode(key);
    }
}
