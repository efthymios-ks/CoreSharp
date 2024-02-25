using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Uri"/> utilities.
/// </summary>
public static class Uritils
{
    /// <summary>
    /// Combines url segments.
    /// Also takes care of multiple slashes.
    /// <example>
    /// <code>
    /// // "/sec1/sec2/sec3/"
    /// var url = UriX.JoinSegments("/sec1/", "/sec2/", "/sec3");
    /// </code>
    /// </example>
    /// </summary>
    public static string JoinSegments(params string[] segments)
    {
        ArgumentNullException.ThrowIfNull(segments);

        var builder = new StringBuilder();

        // Connect 
        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];
            var trimmed = $"{segment}".Trim();

            // If first segment is absolute (e.g. https://www.page.gr), do not insert '/' separator
            if (i == 0 && Uri.IsWellFormedUriString(trimmed, UriKind.Relative))
            {
                builder.Append('/');
            }

            builder.Append(trimmed).Append('/');
        }

        // Build url 
        var url = builder.ToString();

        // Multiple forward-slashes to single one 
        return Regex.Replace(url, @"(?<=[^:\s])(\/+\/)+", "/");
    }

    /// <summary>
    /// Extract uri parameters.
    /// </summary>
    public static IDictionary<string, string> GetParameters(string url)
    {
        var parameters = HttpUtility.ParseQueryString(url);
        return parameters.AllKeys.ToDictionary(k => k, k => parameters[k]);
    }

    /// <inheritdoc cref="ParseAndBuild{TEntity}(string, TEntity, IFormatProvider)"/>
    public static string ParseAndBuild<TEntity>(string baseUrl, TEntity entity)
        where TEntity : class
        => ParseAndBuild(baseUrl, entity, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="Build{TValue}(string, IDictionary{string, TValue})"/>
    public static string ParseAndBuild<TEntity>(string baseUrl, TEntity entity, IFormatProvider formatProvider)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(formatProvider);

        var properties = entity.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .ToDictionary(p => p.Name, p => p.GetValue(entity));
        return Build(baseUrl, properties, formatProvider);
    }

    /// <inheritdoc cref="Build{TValue}(string, IDictionary{string, TValue}, IFormatProvider)"/>
    public static string Build<TValue>(string baseUrl, IDictionary<string, TValue> queryParameters)
        => Build(baseUrl, queryParameters, CultureInfo.InvariantCulture);

    /// <summary>
    /// Build url from base url and query parameters list.
    /// </summary>
    public static string Build<TValue>(string baseUrl, IDictionary<string, TValue> queryParameters, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);
        ArgumentNullException.ThrowIfNull(formatProvider);
        ArgumentException.ThrowIfNullOrEmpty(baseUrl);

        // Try parse URL 
        if (!Uri.TryCreate(baseUrl, UriKind.RelativeOrAbsolute, out var baseUri))
        {
            throw new ArgumentException($"`{nameof(baseUrl)}` is not a valid URL.", nameof(baseUrl));
        }

        // Check if base URL has existing query parameters...
        var baseQueryParameters = baseUri.GetQueryParameters();
        if (baseQueryParameters.Any())
        {
            baseUrl = baseUri.GetLeftPart(UriPartial.Path);
        }

        // ...and merge with provided.
        foreach (var queryParameter in queryParameters)
        {
            baseQueryParameters[queryParameter.Key] = Convert.ToString(queryParameter.Value, formatProvider);
        }

        var query = baseQueryParameters.ToUrlQueryString();

        var trimChars = new[] { ' ', '?', '&', '/' };
        baseUrl = baseUrl.Trim(trimChars);
        query = query.Trim(trimChars);

        return string.IsNullOrWhiteSpace(query) ? baseUrl : $"{baseUrl}/?{query}";
    }
}
