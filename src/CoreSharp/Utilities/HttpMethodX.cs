using CoreSharp.Enums;
using System;
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

            methodName = methodName.ToLowerInvariant();

            if (methodName.Contains(HttpMethod.Get.Method))
                return RestMethod.Get;
            else if (methodName.Contains(HttpMethod.Post.Method))
                return RestMethod.Post;
            else if (methodName.Contains(HttpMethod.Put.Method))
                return RestMethod.Put;
            else if (methodName.Contains(HttpMethod.Patch.Method))
                return RestMethod.Patch;
            else if (methodName.Contains(HttpMethod.Delete.Method))
                return RestMethod.Delete;
            else if (methodName.Contains(HttpMethod.Head.Method))
                return RestMethod.Head;
            else if (methodName.Contains(HttpMethod.Options.Method))
                return RestMethod.Options;
            else if (methodName.Contains(HttpMethod.Trace.Method))
                return RestMethod.Trace;
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
