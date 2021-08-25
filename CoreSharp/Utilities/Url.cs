using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoreSharp.Utilities
{
    public static class Url
    {
        /// <summary>
        /// Combines url segments.
        /// "/sec1", "/sec2/", "sec3" results to "/sec1/sec2/sec3/". 
        /// </summary> 
        public static string Combine(params object[] segments)
        {
            _ = segments ?? throw new ArgumentNullException(nameof(segments));

            var builder = new StringBuilder();

            //Connect 
            foreach (var segment in segments)
            {
                var trimmed = $"/{segment}".Trim();
                builder.Append(trimmed);
            }
            builder.Append("/");

            //Build url 
            var url = builder.ToString();

            //Multiple forward-slashes to single one 
            url = Regex.Replace(url, @"\/+", @"/");

            return url;
        }

        /// <summary>
        /// Extract uri parameters. 
        /// </summary> 
        public static IDictionary<string, string> GetParameters(string url)
        {
            var parameters = HttpUtility.ParseQueryString(url);
            return parameters.AllKeys.ToDictionary(k => k, k => parameters[k]);
        }

        /// <summary>
        /// Build url from base url and parameters.
        /// </summary> 
        public static string Build<TKey, TValue>(string baseUrl, IDictionary<TKey, TValue> parameters, bool encodeParameters = true)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            string query = parameters.ToUrlQueryString(encodeParameters);

            var trimChars = new[] { ' ', '?', '&', '/' };
            baseUrl = baseUrl.Trim(trimChars);
            query = query.Trim(trimChars);

            return $"{baseUrl}/?{query}";
        }
    }
}
