using CoreSharp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace CoreSharp.Models.Exceptions
{
    public class ProblemDetailsException : Exception
    {
        //Constructors
        public ProblemDetailsException(HttpStatusCode httpStatusCode)
            : this(ProblemDetailsX.New(httpStatusCode))
        {
        }

        public ProblemDetailsException(HttpContext httpContext)
            : this(ProblemDetailsX.New(httpContext))
        {
        }

        public ProblemDetailsException(string type, string title, HttpStatusCode status, string detail = null, string instance = null)
            : this(ProblemDetailsX.New(type, title, status, detail, instance))
        {
        }

        public ProblemDetailsException(ProblemDetails problemDetails)
            : base($"{problemDetails?.Type} > {problemDetails?.Title}")
            => ProblemDetails = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));

        //Properties
        public ProblemDetails ProblemDetails { get; }
    }
}
