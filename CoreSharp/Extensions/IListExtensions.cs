using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IList extensions. 
    /// </summary>
    public static partial class IListExtensions
    {
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
    }
}
