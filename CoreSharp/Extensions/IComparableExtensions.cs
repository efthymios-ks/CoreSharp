using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IComparable extensions. 
    /// </summary>
    public static partial class IComparableExtensions
    {
        /// <summary>
        /// Check if an IComparable object is between two values. 
        /// </summary>
        public static bool IsBetween<T>(this T value, T from, T to, bool includeEnds = true) where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;

            if (includeEnds)
                return (comparer.Compare(value, from) >= 0) && (comparer.Compare(value, to) <= 0);
            else
                return (comparer.Compare(value, from) > 0) && (comparer.Compare(value, to) < 0);
        }
    }
}
