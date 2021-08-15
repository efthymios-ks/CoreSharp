using System;
using System.Collections.Specialized;
using System.Linq;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// NameValueCollection extensions. 
    /// </summary>
    public static partial class NameValueCollectionExtensions
    {
        /// <summary>
        /// Build url query string from dictionary parameters. 
        /// </summary> 
        public static string ToUrlQueryString(this NameValueCollection source, bool encodeParameters = true)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var keys = source?.AllKeys;
            var dictionary = keys?.ToDictionary(k => k, k => source[k]);
            return dictionary?.ToUrlQueryString(encodeParameters);
        }
    }
}
