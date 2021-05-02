﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IList extensions. 
    /// </summary>
    public static partial class IListExtensions
    {
        /// <summary>
        /// Fill list with given value. 
        /// </summary> 
        public static void Fill<T>(this IList<T> source, T value)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));

            for (int i = 0; i < source.Count; i++)
                source[i] = value;
        }

        /// <summary>
        /// Remove all occurences of items that match given expression. 
        /// </summary> 
        public static int Remove<T>(this IList<T> source, Func<T, bool> removeExpression)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            removeExpression = removeExpression ?? throw new ArgumentNullException(nameof(removeExpression));

            int count = 0;
            while (source.Any(removeExpression))
            {
                var item = source.FirstOrDefault(removeExpression);
                source.Remove(item);
                count++;
            }
            return count;
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
