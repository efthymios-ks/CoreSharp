using CoreSharp.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="HttpResponseMessage"/> extensions.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Ensure http response was successful using <see cref="HttpResponseMessage.IsSuccessStatusCode"/>.
        /// Throws <see cref="HttpResponseException"/> if not, including <see cref="HttpResponseMessage.StatusCode"/> and <see cref="HttpResponseMessage.Content"/>.
        /// </summary>
        public static async Task EnsureSuccessAsync(this HttpResponseMessage response)
        {
            _ = response ?? throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
                return;

            var requestUrl = response?.RequestMessage?.RequestUri?.AbsoluteUri;
            var requestMethod = $"{nameof(HttpMethod)}.{response?.RequestMessage?.Method?.Method}";
            var responseStatus = response.StatusCode;
            var responseContent = await response?.Content?.ReadAsStringAsync();
            response?.Content?.Dispose();

            throw new HttpResponseException(requestUrl, requestMethod, responseStatus, responseContent);
        }
    }
}
