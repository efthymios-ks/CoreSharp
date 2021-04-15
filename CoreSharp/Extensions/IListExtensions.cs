using System;
using System.Collections.Generic;

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
    }
}
