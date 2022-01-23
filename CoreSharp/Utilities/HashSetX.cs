using CoreSharp.EqualityComparers;
using System;
using System.Collections.Generic;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="HashSet{T}"/> utilities.
    /// </summary>
    public static class HashSetX
    {
        /// <inheritdoc cref="HashSet{T}.HashSet(IEqualityComparer{T}?)" />
        public static HashSet<TItem> Create<TItem, TKey>(Func<TItem, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var comparer = new KeyEqualityComparer<TItem, TKey>(keySelector);
            return new HashSet<TItem>(comparer);
        }

        /// <inheritdoc cref="HashSet{T}.HashSet(int, IEqualityComparer{T}?)" />
        public static HashSet<TItem> Create<TItem, TKey>(int capacity, Func<TItem, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var comparer = new KeyEqualityComparer<TItem, TKey>(keySelector);
            return new HashSet<TItem>(capacity, comparer);
        }

        /// <inheritdoc cref="HashSet{T}.HashSet(IEnumerable{T}, IEqualityComparer{T}?)" />
        public static HashSet<TItem> Create<TItem, TKey>(IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
        {
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var comparer = new KeyEqualityComparer<TItem, TKey>(keySelector);
            return new HashSet<TItem>(source, comparer);
        }
    }
}
