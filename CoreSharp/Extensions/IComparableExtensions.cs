using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IComparable extensions.
    /// </summary>
    public static class IComparableExtensions
    {
        /// <summary>
        /// Check if an IComparable object is between two values.
        /// </summary>
        public static bool IsBetween<T>(this T value, T from, T to, bool includeEnds = true) where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;
            var comparisonFrom = comparer.Compare(value, from);
            var comparisonTo = comparer.Compare(value, to);

            if (includeEnds)
                return (comparisonFrom >= 0) && (comparisonTo <= 0);
            else
                return (comparisonFrom > 0) && (comparisonTo < 0);
        }
    }
}
