using CoreSharp.Extensions;
using CoreSharp.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="ProblemDetails"/> utilities.
    /// </summary>
    public static class ProblemDetailsX
    {
        /// <inheritdoc cref="Create(string, string, HttpStatusCode, string, string)"/>
        public static ProblemDetails Create(HttpContext httpContext)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

            //Get exception
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            _ = exception ?? throw new ArgumentException($"Provided {nameof(HttpContext)} does not feature any {nameof(Exception)}.", nameof(exception));

            //If ProblemDetailsException
            if (exception is ProblemDetailsException pde)
                return pde.ProblemDetails;

            //Else extract information
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isProduction = string.Equals(environment, "Production", StringComparison.InvariantCultureIgnoreCase);
            var httpStatusCode = ToHttpStatusCode(exception);
            var instance = httpContext.Request.Path;
            string type = null;
            string title = null;
            string detail;
            if (isProduction)
            {
                detail = exception.Message;
            }
            else
            {
                title = exception.Message;
                detail = exception.StackTrace;
            }

            return Create(type, title, httpStatusCode, detail, instance);
        }

        /// <inheritdoc cref="Create(string, string, HttpStatusCode, string, string)"/>
        public static ProblemDetails Create(HttpStatusCode httpStatusCode, string detail = null, string instance = null)
            => Create(null, null, httpStatusCode, detail, instance);

        /// <summary>
        /// Create new <see cref="ProblemDetails"/>
        /// with arguments validation for empty and null.
        /// </summary>
        public static ProblemDetails Create(string type, string title, HttpStatusCode httpStatusCode, string detail = null, string instance = null)
        {
            if (string.IsNullOrWhiteSpace(type))
                type = httpStatusCode.ToProblemDetailsType();
            if (string.IsNullOrWhiteSpace(title))
                title = ReasonPhrases.GetReasonPhrase((int)httpStatusCode);

            return new()
            {
                Type = type,
                Title = title,
                Status = (int)httpStatusCode,
                Detail = detail,
                Instance = instance
            };
        }

        /// <summary>
        /// Translate <see cref="Exception"/>
        /// to relevant <see cref="HttpStatusCode"/>.
        /// </summary>
        private static HttpStatusCode ToHttpStatusCode(Exception exception)
        {
            var httpStatusCode = ExtractHttpStatusCode(exception);
            if (httpStatusCode is not null)
                return httpStatusCode.Value;

            return exception switch
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
