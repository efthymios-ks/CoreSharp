using CoreSharp.Utilities;
using System;
using System.Net;

namespace CoreSharp.Models
{
    /// <summary>
    /// Simple HttpResponseMessage exception.
    /// </summary>
#pragma warning disable RCS1194 // Implement exception constructors.
    public class HttpResponseException : Exception
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        //Constructors  
        public HttpResponseException(
            string requestUrl,
            string requestMethod,
            HttpStatusCode responseStatusCode,
            string responseContent,
            Exception innerException = null) : base(responseContent, innerException)
        {
            RequestUrl = requestUrl;
            RequestMethod = requestMethod;
            ResponseStatusCode = responseStatusCode;
        }

        //Properties
        public string RequestUrl { get; }

        public string RequestMethod { get; }

        public HttpStatusCode ResponseStatusCode { get; }

        public string ResponseContent => Message;

        public string ResponseStatus => $"{RequestMethod} > {RequestUrl} > {(int)ResponseStatusCode} {ResponseStatusCode}";

        public override string ToString()
        {
            if (Json.IsEmpty(ResponseContent))
                return ResponseStatus;
            else
                return ResponseStatus + Environment.NewLine + ResponseContent;
        }
    }
}
