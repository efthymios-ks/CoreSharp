using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="ICollection{T}"/> extensions.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <inheritdoc cref="AddRange{T}(ICollection{T}, T[])"/>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
            => source.AddRange(items?.ToArray());

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        public static void AddRange<T>(this ICollection<T> source, params T[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var value in items)
                source.Add(value);
        }
    }
}
