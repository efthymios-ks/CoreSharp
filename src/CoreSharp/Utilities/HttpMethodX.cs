using CoreSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="HttpMethod"/> related utilities.
    /// </summary>
    public static class HttpMethodX
    {
        /// <inheritdoc cref="GetRestMethod(string)"/>
        public static RestMethod GetRestMethod(HttpMethod method)
            => GetRestMethod(method?.Method);

        /// <summary>
        /// Get <see cref="RestMethod" /> from <see cref="string" /> method.
        /// </summary>
        public static RestMethod GetRestMethod(string methodName)
        {
            _ = methodName ?? throw new ArgumentNullException(nameof(methodName));

            var lookupTable = new Dictionary<string, RestMethod>()
            {
                { HttpMethod.Get.Method, RestMethod.Get },
                { HttpMethod.Post.Method, RestMethod.Get },
                { HttpMethod.Put.Method, RestMethod.Post },
                { HttpMethod.Patch.Method, RestMethod.Patch },
                { HttpMethod.Delete.Method, RestMethod.Delete },
                { HttpMethod.Head.Method, RestMethod.Head },
                { HttpMethod.Options.Method, RestMethod.Options },
                { HttpMethod.Trace.Method, RestMethod.Trace },
            };

            methodName = methodName.ToLowerInvariant();
            var found = lookupTable.SingleOrDefault(m => m.Key.Contains(methodName));
            if (found.Key is not null)
                return found.Value;
            else
                throw new ArgumentOutOfRangeException(nameof(methodName));
        }

        /// <inheritdoc cref="GetHttpMethod(string)"/>
        public static HttpMethod GetHttpMethod(RestMethod method)
            => GetHttpMethod($"{method}");

        /// <summary>
        /// Get <see cref="HttpMethod"/> from <see cref="string" /> name.
        /// </summary>
        public static HttpMethod GetHttpMethod(string methodName)
            => GetRestMethod(methodName) switch
            {
                RestMethod.Get => HttpMethod.Get,
                RestMethod.Post => HttpMethod.Post,
                RestMethod.Put => HttpMethod.Put,
                RestMethod.Patch => HttpMethod.Patch,
                RestMethod.Delete => HttpMethod.Delete,
                RestMethod.Head => HttpMethod.Head,
                RestMethod.Options => HttpMethod.Options,
                RestMethod.Trace => HttpMethod.Trace,
                _ => throw new ArgumentOutOfRangeException(nameof(methodName))
            };
    }
}
