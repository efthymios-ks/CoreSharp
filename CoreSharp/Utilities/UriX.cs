using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="Uri"/> utilities.
    /// </summary>
    public static class UriX
    {
        /// <summary>
        /// Combines url segments.
        /// <example>
        /// <code>
        /// // "/sec1/sec2/sec3/"
        /// var url = Url.JoinSegments("/sec1/", "/sec2/", "/sec3");
        /// </code>
        /// </example>
        /// </summary>
        public static string JoinSegments(params object[] segments)
        {
            _ = segments ?? throw new ArgumentNullException(nameof(segments));

            var builder = new StringBuilder();

            //Connect 
            foreach (var segment in segments)
            {
                var trimmed = $"/{segment}".Trim();
                builder.Append(trimmed);
            }

            builder.Append('/');

            //Build url 
            var url = builder.ToString();

            //Multiple forward-slashes to single one 
            return Regex.Replace(url, @"\/+", "/");
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
        /// Build url from base url and parameters.
        /// </summary>
        public static string Build<TValue>(string baseUrl, IDictionary<string, TValue> parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            var query = parameters.ToUrlQueryString();

            var trimChars = new[] { ' ', '?', '&', '/' };
            baseUrl = baseUrl.Trim(trimChars);
            query = query.Trim(trimChars);

            return string.IsNullOrWhiteSpace(query) ? baseUrl : $"{baseUrl}/?{query}";
        }
    }
}
