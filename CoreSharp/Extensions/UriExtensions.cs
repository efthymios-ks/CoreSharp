using CoreSharp.Utilities;
using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Uri"/> extensions.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Get url query parameters to dictionary.
        /// </summary>
        public static IDictionary<string, string> GetQueryParameters(this Uri uri)
        {
            _ = uri ?? throw new ArgumentNullException(nameof(uri));

            return Url.GetParameters(uri.Query);
        }

        /// <summary>
        /// Get url fragment parameters to dictionary.
        /// </summary>
        public static IDictionary<string, string> GetFragmentParameters(this Uri uri)
        {
            _ = uri ?? throw new ArgumentNullException(nameof(uri));

            return Url.GetParameters(uri.Fragment.TrimStart('#'));
        }
    }
}
