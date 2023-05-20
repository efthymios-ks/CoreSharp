using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IList{TElement}"/> extensions.
/// </summary>
public static class IListExtensions
{
    /// <summary>
    /// Fill <see cref="IList{TElement}"/> with given value.
    /// </summary>
    public static void Fill<TElement>(this IList<TElement> source, TElement item)
    {
        ArgumentNullException.ThrowIfNull(source);

        for (var i = 0; i < source.Count; i++)
        {
            source[i] = item;
        }
    }

    /// <summary>
    /// Remove all occurrences of items that match given expression.
    /// </summary>
    public static int Remove<TElement>(this IList<TElement> source, Func<TElement, bool> removeExpression)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(removeExpression);

        var count = 0;
        while (source.Any(removeExpression))
        {
            var occurence = source.FirstOrDefault(removeExpression);
            source.Remove(occurence);
            count++;
        }

        return count;
    }

    /// <inheritdoc cref="InsertRange{TElement}(IList{TElement}, int, TElement[])"/>
    public static void InsertRange<TElement>(this IList<TElement> source, int index, IEnumerable<TElement> values)
        => source.InsertRange(index, values?.ToArray());

    /// <summary>
    /// Insert range in given position.
    /// </summary>
    public static void InsertRange<TElement>(this IList<TElement> source, int index, params TElement[] values)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(values);
        if (index < 0 || index >= source.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} should be between 0 and {source.Count - 1}.");
        }

        foreach (var value in values)
        {
            source.Insert(index++, value);
        }
    }
}
