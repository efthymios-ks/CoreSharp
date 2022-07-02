using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Uri"/> utilities.
/// </summary>
public static class UriX
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
        _ = segments ?? throw new ArgumentNullException(nameof(segments));

        var builder = new StringBuilder();

        //Connect 
        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];
            var trimmed = $"{segment}".Trim();

            //If first segment is absolute (e.g. https://www.page.gr), do not insert '/' separator
            if (i == 0 && Uri.IsWellFormedUriString(trimmed, UriKind.Relative))
                builder.Append('/');

            builder.Append(trimmed);
        }

        builder.Append('/');

        //Build url 
        var url = builder.ToString();

        //Multiple forward-slashes to single one 
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

    /// <inheritdoc cref="Build{TValue}(string, IDictionary{string, TValue})"/>
    public static string ParseAndBuild<TEntity>(string baseUrl, TEntity entity)
        where TEntity : class
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));
        var properties = entity.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .ToDictionary(p => p.Name, p => p.GetValue(entity));
        return Build(baseUrl, properties);
    }

    /// <summary>
    /// Build url from base url and query parameters list.
    /// </summary>
    public static string Build<TValue>(string baseUrl, IDictionary<string, TValue> queryParameters)
    {
        _ = queryParameters ?? throw new ArgumentNullException(nameof(queryParameters));
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl));

        var query = queryParameters.ToUrlQueryString();

        var trimChars = new[] { ' ', '?', '&', '/' };
        baseUrl = baseUrl.Trim(trimChars);
        query = query.Trim(trimChars);

        return string.IsNullOrWhiteSpace(query) ? baseUrl : $"{baseUrl}/?{query}";
    }
}
