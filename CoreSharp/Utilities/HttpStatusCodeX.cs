using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="HttpStatusCode"/> utilities.
    /// </summary>
    public static class HttpStatusCodeX
    {
        /// <summary>
        /// Convert <see cref="HttpStatusCode"/> to reference url.
        /// <code>
        /// // https://httpstatuses.com/500
        /// var type = HttpStatusCode.InternalServerError.ToProblemDetailsType();
        /// </code>
        /// </summary>
        public static string GetReferenceUrl(HttpStatusCode httpStatusCode)
            => $"https://httpstatuses.com/{(int)httpStatusCode}";

        /// <summary>
        /// Translate <see cref="Exception"/>
        /// to relevant <see cref="HttpStatusCode"/>.
        /// </summary>
        public static HttpStatusCode FromException(Exception exception)
        {
            var httpStatusCode = ExtractHttpStatusCode(exception);
            return httpStatusCode ?? exception switch
            {
                ApplicationException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                NotImplementedException => HttpStatusCode.NotImplemented,
                NotSupportedException => HttpStatusCode.NotImplemented,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };
        }

        /// <summary>
        /// Use reflection to look for the first
        /// <see cref="HttpStatusCode"/> property in given exception.
        /// </summary>
        private static HttpStatusCode? ExtractHttpStatusCode(Exception exception)
        {
            _ = exception ?? throw new ArgumentNullException(nameof(exception));

            var properties = exception.GetType()
                                      .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Extract HttpStatusCode directly 
            var statusCodeProperty = Array.Find(properties, p => p.PropertyType == typeof(HttpStatusCode));
            if (statusCodeProperty?.GetValue(exception) is HttpStatusCode statusCode)
                return statusCode;

            //Extract from HttpResponseMessage 
            var httpResponseMessageProperty = Array.Find(properties, p => p.PropertyType == typeof(HttpResponseMessage));
            if (httpResponseMessageProperty?.GetValue(exception) is HttpResponseMessage response)
                return response.StatusCode;

            //None found
            return null;
        }
    }
}
