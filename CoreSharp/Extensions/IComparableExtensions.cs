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
        public static bool Between<T>(this T value, T from, T to, bool includeEnds = true) where T : IComparable<T>
        {
            if (includeEnds)
                return ((Comparer<T>.Default.Compare(value, from) >= 0) && (Comparer<T>.Default.Compare(value, to) <= 0));
            else
                return ((Comparer<T>.Default.Compare(value, from) > 0) && (Comparer<T>.Default.Compare(value, to) < 0));
        }
    }
}
