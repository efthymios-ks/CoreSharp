using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IComparable"/> extensions.
/// </summary>
public static class IComparableExtensions
{
    /// <summary>
    /// Check if <see cref="IComparable"/> object is between two values.
    /// </summary>
    public static bool IsBetween<TElement>(this TElement value, TElement from, TElement to, bool includeEnds = true)
        where TElement : IComparable<TElement>
    {
        var comparer = Comparer<TElement>.Default;
        var comparisonFrom = comparer.Compare(value, from);
        var comparisonTo = comparer.Compare(value, to);

        if (includeEnds)
        {
            return comparisonFrom >= 0 && comparisonTo <= 0;
        }
        else
        {
            return comparisonFrom > 0 && comparisonTo < 0;
        }
    }
}
