using System;
using System.Net;

namespace CoreSharp.Models
{
    /// <summary>
    /// Simple HttpResponseMessage exception. 
    /// Stores a HttpStatusCode and Content (string). 
    /// </summary>
    public class HttpResponseException : Exception
    {
        //Constructors
        public HttpResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }

        //Properties
        public HttpStatusCode StatusCode { get; }
    }
}
