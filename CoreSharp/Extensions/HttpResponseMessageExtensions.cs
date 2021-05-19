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
        /// Ensure http response was successful. 
        /// Throws HttpResponseException if not, including HttpStatusCode and Content. 
        /// </summary> 
        public async static Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = await response?.Content?.ReadAsStringAsync();
            response?.Content?.Dispose();

            throw new HttpResponseException(response.StatusCode, content);
        }
    }
}
