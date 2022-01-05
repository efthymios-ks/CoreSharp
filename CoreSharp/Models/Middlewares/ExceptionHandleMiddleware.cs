using CoreSharp.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreSharp.Models.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        //Fields 
        private readonly RequestDelegate _next;

        //Constructors
        public ExceptionHandleMiddleware(RequestDelegate next)
            => _next = next;

        //Methods
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception) when (!httpContext.Response.HasStarted)
            {
                SetContextExceptionFeature(httpContext, exception);
                await HandleExceptionAsync(httpContext);
            }
        }

        private static void SetContextExceptionFeature(HttpContext httpContext, Exception exception)
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

        private static async Task HandleExceptionAsync(HttpContext httpContext)
        {
            _ = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

            var response = httpContext.Response;
            var problemDetails = ProblemDetailsX.Create(httpContext);
            await WriteResponseAsync(response, problemDetails);
        }

        private static async Task WriteResponseAsync(HttpResponse httpResponse, ProblemDetails problemDetails)
        {
            _ = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));

            if (httpResponse.HasStarted)
                return;

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var problemDetailsAsJson = JsonSerializer.Serialize(problemDetails, jsonSerializerOptions);

            httpResponse.Clear();
            httpResponse.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
            httpResponse.ContentType = MediaTypeNamesX.Application.ProblemJson;
            await httpResponse.WriteAsync(problemDetailsAsJson);
        }
    }
}
