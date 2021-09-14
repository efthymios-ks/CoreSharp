using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreSharp.Models;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// HttpResponseMessage extensions.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Ensure http response was successful using HttpStatusCode.
        /// Throws HttpResponseException if not, including HttpStatusCode and Content.
        /// </summary>
        public static async Task EnsureSuccessAsync(this HttpResponseMessage response)
        {
            _ = response ?? throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
                return;

            var content = await (response?.Content?.ReadAsStringAsync()).ConfigureAwait(false);
            response?.Content?.Dispose();

            throw new HttpResponseException(response.StatusCode, content);
        }
    }
}
