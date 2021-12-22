﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IList{T}"/> extensions.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Fill <see cref="IList{T}"/> with given value.
        /// </summary>
        public static void Fill<TItem>(this IList<TItem> source, TItem item)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            for (var i = 0; i < source.Count; i++)
                source[i] = item;
        }

        /// <summary>
        /// Remove all occurrences of items that match given expression.
        /// </summary>
        public static int Remove<TItem>(this IList<TItem> source, Func<TItem, bool> removeExpression)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = removeExpression ?? throw new ArgumentNullException(nameof(removeExpression));

            var count = 0;
            while (source.Any(removeExpression))
            {
                var occurence = source.FirstOrDefault(removeExpression);
                source.Remove(occurence);
                count++;
            }
            return count;
        }

        /// <inheritdoc cref="InsertRange{T}(IList{T}, int, T[])"/>
        public static void InsertRange<TItem>(this IList<TItem> source, int index, IEnumerable<TItem> values)
            => source.InsertRange(index, values?.ToArray());

        /// <summary>
        /// Insert range in given position.
        /// </summary>
        public static void InsertRange<TItem>(this IList<TItem> source, int index, params TItem[] values)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = values ?? throw new ArgumentNullException(nameof(values));
            if (index < 0 || index >= source.Count)
                throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} should be between 0 and {source.Count - 1}.");

            foreach (var value in values)
                source.Insert(index++, value);
        }
    }
}
