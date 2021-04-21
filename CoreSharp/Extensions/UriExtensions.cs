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

            return GetUriParametersInternal(uri.Query);
        }

        /// <summary>
        /// Get url fragment parameters to dictionary. 
        /// </summary> 
        public static IDictionary<string, string> GetFragmentParameters(this Uri uri)
        {
            uri = uri ?? throw new ArgumentNullException(nameof(uri));

            return GetUriParametersInternal(uri.Fragment.TrimStart('#'));
        }

        private static IDictionary<string, string> GetUriParametersInternal(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
                throw new ArgumentNullException(nameof(uri));

            var parameters = HttpUtility.ParseQueryString(uri);
            var dictionary = parameters.AllKeys.ToDictionary(k => k, k => parameters[k]);
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
