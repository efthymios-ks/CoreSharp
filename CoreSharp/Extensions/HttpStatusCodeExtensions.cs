using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="HttpStatusCode"/> extensions.
    /// </summary>
    public static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Convert <see cref="HttpStatusCode"/> to
        /// default value for <see cref="ProblemDetails.Type"/>
        /// <code>
        /// // https://httpstatuses.com/500
        /// var type = HttpStatusCode.InternalServerError.ToProblemDetailsType();
        /// </code>
        /// </summary>
        public static string ToProblemDetailsType(this HttpStatusCode httpStatusCode)
            => $"https://httpstatuses.com/{(int)httpStatusCode}";
    }
}
