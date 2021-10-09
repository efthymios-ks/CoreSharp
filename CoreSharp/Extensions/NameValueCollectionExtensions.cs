using System;
using System.Collections.Specialized;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="NameValueCollection"/> extensions.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <inheritdoc cref="IDictionaryExtensions.ToUrlQueryString{TValue}(System.Collections.Generic.IDictionary{string, TValue}, bool)"/>
        public static string ToUrlQueryString(this NameValueCollection source, bool encodeParameters = true)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var keys = source?.AllKeys;
            var dictionary = keys?.ToDictionary(k => k, k => source[k]);
            return dictionary?.ToUrlQueryString(encodeParameters);
        }
    }
}
