using CoreSharp.Enums;
using System;
using System.Net.Http;

namespace CoreSharp.Utilities
{
    public static class Http
    {
        /// <summary>
        /// Get RestMethod from string name. 
        /// </summary> 
        public static RestMethod GetRestMethod(string methodName)
        {
            _ = methodName ?? throw new ArgumentNullException(nameof(methodName));

            methodName = methodName.Trim().ToLowerInvariant();

            if (methodName.Contains("get"))
                return RestMethod.Get;
            else if (methodName.Contains("post"))
                return RestMethod.Post;
            else if (methodName.Contains("put"))
                return RestMethod.Put;
            else if (methodName.Contains("patch"))
                return RestMethod.Patch;
            else if (methodName.Contains("delete"))
                return RestMethod.Delete;
            else
                throw new ArgumentOutOfRangeException(nameof(methodName));
        }

        /// <summary>
        /// Get RestMethod from HttpMethod. 
        /// </summary> 
        public static RestMethod GetRestMethod(HttpMethod method) => GetRestMethod(method?.Method);

        /// <summary>
        /// Get HttpMethod from string name. 
        /// </summary> 
        public static HttpMethod GetHttpMethod(string methodName) => GetRestMethod(methodName) switch
        {
            RestMethod.Get => HttpMethod.Get,
            RestMethod.Post => HttpMethod.Post,
            RestMethod.Put => HttpMethod.Put,
            RestMethod.Patch => HttpMethod.Patch,
            RestMethod.Delete => HttpMethod.Delete,
            _ => throw new ArgumentOutOfRangeException(nameof(methodName))
        };

        /// <summary>
        /// Get HttpMethod from RestMethod. 
        /// </summary> 
        public static HttpMethod GetHttpMethod(RestMethod method) => GetHttpMethod($"{method}");
    }
}
