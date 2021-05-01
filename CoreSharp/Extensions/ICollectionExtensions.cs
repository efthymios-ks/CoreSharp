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
            source = source ?? throw new ArgumentNullException(nameof(source));
            items = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var value in items)
                source.Add(value);
        }

        /// <summary>
        /// Insert range in given position. 
        /// </summary> 
        public static void InsertRange<T>(this IList<T> source, int index, IEnumerable<T> values)
        {
            source.InsertRange(index, values?.ToArray());
        }

        /// <summary>
        /// Insert range in given position. 
        /// </summary> 
        public static void InsertRange<T>(this IList<T> source, int index, params T[] values)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            values = values ?? throw new ArgumentNullException(nameof(values));
            if (index < 0 || index >= source.Count)
                throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} should be between 0 and {source.Count - 1}.");

            foreach (var value in values)
                source.Insert(index++, value);
        }
    }
}
