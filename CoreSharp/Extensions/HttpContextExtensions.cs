﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="HttpContext"/> extensions.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Set <see cref="IExceptionHandlerFeature"/>
        /// and <see cref="IExceptionHandlerPathFeature"/>
        /// to given <see cref="Exception"/>.
        /// </summary>
        public static void SetExceptionHandlerFeature(this HttpContext httpContext, Exception exception)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _ = exception ?? throw new ArgumentNullException(nameof(exception));

            var feature = new ExceptionHandlerFeature()
            {
                Path = httpContext.Request.Path,
                Error = exception
            };
            httpContext.Features.Set<IExceptionHandlerFeature>(feature);
            httpContext.Features.Set<IExceptionHandlerPathFeature>(feature);
        }
    }
}
