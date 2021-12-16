using CoreSharp.Models.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IQueryable{T}"/> extensions.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <inheritdoc cref="GetPageAsync{TEntity}(IQueryable{TEntity}, int, int, CancellationToken)"/>
        public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), $"{nameof(pageNumber)} has to be positive.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

            return query.Skip(pageNumber * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Paginate collection on given size and return page of given number.
        /// </summary>
        public static async Task<Page<TEntity>> GetPageAsync<TEntity>(this IQueryable<TEntity> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            //Validate args 
            _ = query ?? throw new ArgumentNullException(nameof(query));
            if (pageNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), $"{nameof(pageNumber)} has to be positive.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

            //Calculate and paginate 
            var pagedQuery = query.GetPage(pageNumber, pageSize);
            TEntity[] items;
            int totalItems;
            if (query is IAsyncEnumerable<TEntity>)
            {
                items = await pagedQuery.ToArrayAsync(cancellationToken);
                totalItems = await query.CountAsync(cancellationToken);
            }
            else
            {
                items = pagedQuery.ToArray();
                totalItems = query.Count();
            }
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            //Return
            return new(pageNumber, pageSize, totalItems, totalPages, items);
        }

        /// <inheritdoc cref="FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
        public static IQueryable<string> FilterFlexible(this IQueryable<string> query, string filter)
            => query.FilterFlexible(i => i, filter);

        /// <summary>
        /// Filter source by given value.
        /// In-between characters are allowed in filtering value.
        /// <example>
        /// <code>
        /// var source = new [] { "a", "b", "ab", ".a.b.", "AB", "ba" };
        /// var filter = "ab";
        /// var result = source.FilterFlexible(filter); // "ab", ".a.b.", "AB"
        /// </code>
        /// </example>
        /// </summary>
        public static IQueryable<TItem> FilterFlexible<TItem>(this IQueryable<TItem> query, Func<TItem, string> propertySelector, string filter)
        {
            //Argument validation 
            _ = query ?? throw new ArgumentNullException(nameof(query));
            _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
            filter ??= string.Empty;

            //Remove whitespace
            filter = Regex.Replace(filter, @"\s", string.Empty);

            //If empty, return empty 
            if (string.IsNullOrWhiteSpace(filter))
                return Enumerable.Empty<TItem>().AsQueryable();

            //Get all characters 
            var characters = filter.ToCharArray();

            //Build RegEx pattern 
            var builder = new StringBuilder();
            for (var i = 0; i < characters.Length; i++)
            {
                var character = characters[i];
                var escapedChar = Regex.Escape($"{character}");

                builder.Append(escapedChar);

                if (i < characters.Length - 1)
                    builder.Append(".*");
            }
            var pattern = $"{builder}";

            //Build RegEx object 
            var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //Filter 
            return query.Where(i => regex.IsMatch(propertySelector(i)));
        }
    }
}
