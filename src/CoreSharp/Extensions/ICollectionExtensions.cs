using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="ICollection{T}"/> extensions.
/// </summary>
public static class ICollectionExtensions
{
    /// <summary>
    /// Adds the elements to the end
    /// of the provided <see cref="ICollection{T}"/>.
    /// </summary>
    public static void AddRange<TElement>(this ICollection<TElement> source, IEnumerable<TElement> items)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            source.Add(item);
        }
    }

    /// <inheritdoc cref="TryAdd{TElement, TKey}(ICollection{TElement}, TElement, Func{TElement, TKey})"/>
    public static bool TryAdd<TElement>(this ICollection<TElement> source, TElement item)
        => source.TryAdd(item, i => i);

    /// <summary>
    /// Add item in <see cref="ICollection{T}"/> only if not already existing.
    /// </summary>
    public static bool TryAdd<TElement, TKey>(this ICollection<TElement> source, TElement item, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(keySelector);

        var itemKey = keySelector(item);
        foreach (var elelement in source)
        {
            var elementKey = keySelector(elelement);
            if (Equals(elementKey, itemKey))
            {
                return false;
            }
        }

        source.Add(item);
        return true;
    }
}
