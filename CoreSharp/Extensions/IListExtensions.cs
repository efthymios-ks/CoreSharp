using System;
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
        /// Fill list with given value.
        /// </summary>
        public static void Fill<T>(this IList<T> source, T value)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            for (var i = 0; i < source.Count; i++)
                source[i] = value;
        }

        /// <summary>
        /// Remove all occurrences of items that match given expression.
        /// </summary>
        public static int Remove<T>(this IList<T> source, Func<T, bool> removeExpression)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            _ = removeExpression ?? throw new ArgumentNullException(nameof(removeExpression));

            var count = 0;
            while (source.Any(removeExpression))
            {
                var item = source.FirstOrDefault(removeExpression);
                source.Remove(item);
                count++;
            }
            return count;
        }

        /// <inheritdoc cref="InsertRange{T}(IList{T}, int, T[])"/>
        public static void InsertRange<T>(this IList<T> source, int index, IEnumerable<T> values)
            => source.InsertRange(index, values?.ToArray());

        /// <summary>
        /// Insert range in given position.
        /// </summary>
        public static void InsertRange<T>(this IList<T> source, int index, params T[] values)
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
