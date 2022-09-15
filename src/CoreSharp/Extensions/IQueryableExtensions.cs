using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="IQueryable{TElement}"/> extensions.
/// </summary>
public static class IQueryableExtensions
{
    /// <summary>
    /// Paginate collection on given size and return page of given number.
    /// </summary>
    public static IQueryable<TElement> GetPage<TElement>(this IQueryable<TElement> query, int pageNumber, int pageSize)
    {
        _ = query ?? throw new ArgumentNullException(nameof(query));
        if (pageNumber < 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), $"{nameof(pageNumber)} has to be positive.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} has to be positive and non-zero.");

        return query.Skip(pageNumber * pageSize).Take(pageSize);
    }

    /// <inheritdoc cref="FilterFlexible{TElement}(IQueryable{TElement}, Func{TElement, string}, string)"/>
    public static IQueryable<string> FilterFlexible(this IQueryable<string> query, string filter)
        => query.FilterFlexible(i => i, filter);

    /// <summary>
    /// Filter source by given value.<br/>
    /// In-between characters are allowed in filtering value.
    /// <example>
    /// <code>
    /// var source = new [] { "a", "b", "ab", ".a.b.", "AB", "ba" };
    /// var filter = "ab";
    /// var result = source.FilterFlexible(filter); // "ab", ".a.b.", "AB"
    /// </code>
    /// </example>
    /// </summary>
    public static IQueryable<TElement> FilterFlexible<TElement>(this IQueryable<TElement> query, Func<TElement, string> propertySelector, string filter)
    {
        // Argument validation 
        _ = query ?? throw new ArgumentNullException(nameof(query));
        _ = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
        filter ??= string.Empty;

        // Remove whitespace
        filter = Regex.Replace(filter, @"\s", string.Empty);

        // If empty, return empty 
        if (string.IsNullOrWhiteSpace(filter))
            return Enumerable.Empty<TElement>().AsQueryable();

        // Get all characters 
        var characters = filter.ToCharArray();

        // Build RegEx pattern 
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

        // Build RegEx object 
        var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

        // Filter 
        return query.Where(i => regex.IsMatch(propertySelector(i)));
    }
}
