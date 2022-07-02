using CoreSharp.EqualityComparers;
using System;
using System.Collections.Generic;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="HashSet{TElement}"/> utilities.
/// </summary>
public static class HashSetX
{
    /// <inheritdoc cref="HashSet{TElement}.HashSet(IEqualityComparer{TElement}?)" />
    public static HashSet<TElement> Create<TElement, TKey>(Func<TElement, TKey> keySelector)
    {
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        var comparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return new HashSet<TElement>(comparer);
    }

    /// <inheritdoc cref="HashSet{TElement}.HashSet(int, IEqualityComparer{TElement}?)" />
    public static HashSet<TElement> Create<TElement, TKey>(int capacity, Func<TElement, TKey> keySelector)
    {
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        var comparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return new HashSet<TElement>(capacity, comparer);
    }

    /// <inheritdoc cref="HashSet{TElement}.HashSet(IEnumerable{TElement}, IEqualityComparer{TElement}?)" />
    public static HashSet<TElement> Create<TElement, TKey>(IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        _ = source ?? throw new ArgumentNullException(nameof(source));

        var comparer = new KeyEqualityComparer<TElement, TKey>(keySelector);
        return new HashSet<TElement>(source, comparer);
    }
}
