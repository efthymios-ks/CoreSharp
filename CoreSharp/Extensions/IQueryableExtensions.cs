using System;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// IQueryable extensions. 
    /// </summary>
    public static partial class IQueryableExtensions
    {
        /// <summary>
        /// Paginate collection on given size and return page of given index. 
        /// </summary> 
        public static IQueryable<T> QueryPage<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), $"{nameof(pageIndex)} has to be positive.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

            return source.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}
