using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="List{TElement}"/> extensions.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Sorts the elements in the <see cref="List{TElement}"/>.
    /// </summary>
    public static void Sort<TElement, TKey>(this List<TElement> source, Func<TElement, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);

        var comparer = Comparer<TKey>.Default;
        source.Sort((x, y) => comparer.Compare(keySelector(x), keySelector(y)));
    }
}
