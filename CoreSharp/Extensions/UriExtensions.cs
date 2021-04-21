using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// Uri extensions.
    /// </summary>
    public static partial class UriExtensions
    {
        /// <summary>
        /// Get url query parameters to dictionary. 
        /// </summary> 
        public static IDictionary<string, string> GetQueryParameters(this Uri uri)
        {
            uri = uri ?? throw new ArgumentNullException(nameof(uri));

            var collection = HttpUtility.ParseQueryString(uri.Query);
            var dictionary = collection.AllKeys.ToDictionary(k => k, k => collection[k]);
            return dictionary;
        }

        /// <summary>
        /// Build url from base url and parameters.
        /// </summary> 
        public static Uri BuildUri<TKey, TValue>(string baseUrl, IDictionary<TKey, TValue> parameters, bool encodeParameters = true)
        {
            parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException(nameof(baseUrl));

            string query = parameters.ToUrlQueryString(encodeParameters);

            var trimChars = new char[] { ' ', '?', '&', '/' };
            baseUrl = baseUrl.Trim(trimChars);
            query = query.Trim(trimChars);

            string url = $"{baseUrl}/?{query}";
            return new Uri(url);
        }
    }
}
