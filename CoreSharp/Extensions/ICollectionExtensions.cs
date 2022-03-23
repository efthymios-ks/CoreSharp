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
        /// <inheritdoc cref="AddRange{TElement}(ICollection{TElement}, TElement[])"/>
        public static void AddRange<TElement>(this ICollection<TElement> source, IEnumerable<TElement> items)
            => source.AddRange(items?.ToArray());

        /// <summary>
        /// Adds the elements to the end
        /// of the provided <see cref="ICollection{T}"/>.
        /// </summary>
        public static void AddRange<TElement>(this ICollection<TElement> source, params TElement[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                source.Add(item);
        }

        /// <inheritdoc cref="TryAdd{TElement, TKey}(ICollection{TElement}, TElement, Func{TElement, TKey})"/>
        public static bool TryAdd<TElement>(this ICollection<TElement> source, TElement item)
            => source.TryAdd(item, i => i);

        /// <summary>
        /// Add item in <see cref="ICollection{T}"/> only if not already existing.
        /// </summary>
        public static bool TryAdd<TElement, TKey>(this ICollection<TElement> source, TElement item, Func<TElement, TKey> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            if (source.Any(i => Equals(keySelector(i), keySelector(item))))
                return false;

            source.Add(item);
            return true;
        }
    }
}
