using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="List{T}"/> extensions.
    /// </summary>
    public static class ListExtensions
    {
        /// <inheritdoc cref="List{T}.Sort(IComparer{T}?)" />
        public static void Sort<TEntity, TKey>(this List<TEntity> source, Func<TEntity, TKey> keySelector)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var comparer = Comparer<TKey>.Default;
            source.Sort((x, y) => comparer.Compare(keySelector(x), keySelector(y)));
        }
    }
}
