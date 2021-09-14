using CoreSharp.Utilities;
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
        public HttpResponseException(HttpStatusCode statusCode, string content, Exception innerException = null) : base(content, innerException)
        {
            StatusCode = statusCode;
        }

        //Properties
        public HttpStatusCode StatusCode { get; }

        public string Content => Message;

        public string Status => $"{(int)StatusCode} {StatusCode}";

        //Methods 
        public override string ToString()
        {
            if (Json.IsEmpty(Content))
                return Status;
            else
                return Status + Environment.NewLine + Content;
        }
    }
}
