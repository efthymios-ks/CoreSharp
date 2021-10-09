using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="IQueryable{T}"/> extensions.
    /// </summary>
    public static class IQueryableExtensions
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

        /// <inheritdoc cref="FilterFlexible{TItem}(IQueryable{TItem}, Func{TItem, string}, string)"/>
        public static IQueryable<string> FilterFlexible(this IQueryable<string> source, string filter)
            => source.FilterFlexible(i => i, filter);

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
        public static IQueryable<TItem> FilterFlexible<TItem>(this IQueryable<TItem> source, Func<TItem, string> propertySelector, string filter)
        {
            //Argument validation 
            _ = source ?? throw new ArgumentNullException(nameof(source));
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

                if (i < (characters.Length - 1))
                    builder.Append(".*");
            }
            var pattern = $"{builder}";

            //Build RegEx object 
            var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //Filter 
            return source.Where(i => regex.IsMatch(propertySelector(i)));
        }
    }
}
