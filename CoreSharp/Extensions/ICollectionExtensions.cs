using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// ICollection extensions.
    /// </summary>
    public static partial class ICollectionExtensions
    {
        /// <summary> 
        /// Adds multiple items to ICollection.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            source.AddRange(items?.ToArray());
        }

        /// <summary> 
        /// Adds multiple items to ICollection.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> source, params T[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var value in items)
                source.Add(value);
        }
    }
}
