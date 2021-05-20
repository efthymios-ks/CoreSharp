using System;
using System.Net;

namespace CoreSharp.Models
{
    /// <summary>
    /// Simple HttpResponseMessage exception. 
    /// Stores an HttpStatusCode and Content (Exception.Message). 
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
