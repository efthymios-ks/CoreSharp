using System;
using System.Net.Http;
using System.Threading.Tasks;
using CoreSharp.Models;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// HttpResponseMessage extensions. 
    /// </summary>
    public static partial class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Ensure http response was successfu using HttpStatusCode. 
        /// Throws HttpResponseException if not, including HttpStatusCode and Content. 
        /// </summary> 
        public async static Task EnsureSuccessAsync(this HttpResponseMessage response)
        {
            response = response ?? throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
                return;

            var content = await response?.Content?.ReadAsStringAsync();
            response?.Content?.Dispose();

            throw new HttpResponseException(response.StatusCode, content);
        }
    }
}
