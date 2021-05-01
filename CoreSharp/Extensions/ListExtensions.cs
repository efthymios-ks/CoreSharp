using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// List extensions.
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// In-place sort for given key. 
        /// </summary> 
        public static void Sort<TEntity, TKey>(this List<TEntity> source, Func<TEntity, TKey> keySelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

            var comparer = Comparer<TKey>.Default;
            source.Sort((x, y) => comparer.Compare(keySelector(x), keySelector(y)));
        }
    }
}
