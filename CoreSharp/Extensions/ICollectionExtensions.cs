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
        /// <inheritdoc cref="AddRange{TItem}(ICollection{TItem}, TItem[])"/>
        public static void AddRange<TItem>(this ICollection<TItem> source, IEnumerable<TItem> items)
            => source.AddRange(items?.ToArray());

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        public static void AddRange<TItem>(this ICollection<TItem> source, params TItem[] items)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = items ?? throw new ArgumentNullException(nameof(items));

            foreach (var value in items)
                source.Add(value);
        }

        /// <inheritdoc cref="TryAdd{TItem, TKey}(ICollection{TItem}, TItem, Func{TItem, TKey})"/>
        public static bool TryAdd<TItem>(this ICollection<TItem> source, TItem item)
            => source.TryAdd(item, i => i);

        /// <summary>
        /// Add item in collection only if not already existing.
        /// </summary>
        public static bool TryAdd<TItem, TKey>(this ICollection<TItem> source, TItem item, Func<TItem, TKey> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = item ?? throw new ArgumentNullException(nameof(item));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var sourceKeys = source.Select(keySelector);
            var itemKey = keySelector(item);

            if (sourceKeys.Contains(itemKey))
                return false;
            else
            {
                source.Add(item);
                return true;
            }
        }
    }
}
