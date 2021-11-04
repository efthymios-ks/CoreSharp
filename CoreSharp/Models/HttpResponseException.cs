using CoreSharp.Utilities;
using System;
using System.Net;
using System.Net.Http;

namespace CoreSharp.Models
{
    /// <summary>
    /// Simple <see cref="HttpResponseMessage"/> exception.
    /// </summary>
    public class HttpResponseException : Exception
    {
        //Constructors
        public HttpResponseException(
            HttpRequestMessage request,
            HttpResponseMessage response,
            Exception innerException = null)
            : this(
                request.RequestUri?.AbsoluteUri,
                request.Method.Method,
                response.StatusCode,
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                innerException)
        {
        }

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
            if (JsonX.IsEmpty(ResponseContent))
                return ResponseStatus;
            else
                return ResponseStatus + Environment.NewLine + ResponseContent;
        }
    }
}
