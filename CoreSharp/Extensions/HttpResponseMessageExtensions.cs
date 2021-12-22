﻿using CoreSharp.Models;
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
        /// Ensure <see cref="HttpResponseMessage" /> was successful using
        /// <see cref="HttpResponseMessage.IsSuccessStatusCode" />.
        /// Throws <see cref="HttpResponseException" /> if not,
        /// including <see cref="HttpResponseMessage.StatusCode" />
        /// and <see cref="HttpResponseMessage.Content" />.
        /// </summary>
        public static async Task EnsureSuccessAsync(this HttpResponseMessage response)
        {
            _ = response ?? throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
                return;

            var exception = await HttpResponseException.ParseAsync(response);
            response.Content?.Dispose();
            throw exception;
        }
    }
}
