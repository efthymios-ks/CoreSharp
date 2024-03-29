﻿using CoreSharp.Constants;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="HttpContent"/> extensions.
/// </summary>
public static class HttpContentExtensions
{
    /// <inheritdoc cref="HttpContentHeadersExtensions.GetContentType(HttpContentHeaders)" />
    public static ContentType GetContentType(this HttpContent httpContent)
        => httpContent?.Headers.GetContentType();

    /// <summary>
    /// Check <see cref="HttpContent.Headers"/>
    /// for <see cref="HeaderNames.ContentType"/>
    /// and map accordingly.
    /// </summary>
    public static async Task<TResponse> DeserializeAsync<TResponse>(this HttpContent httpContent, CancellationToken cancellationToken = default) where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(httpContent);

        // Get content type 
        var contentType = httpContent.GetContentType()
            ?? throw new KeyNotFoundException($"{HeaderNames.ContentType} header missing from the response.");

        // Check content type 
        return contentType.MediaType switch
        {
            MediaTypeNames.Application.Json or
            MediaTypeNamesX.Application.ProblemJson => await httpContent.FromJsonAsync<TResponse>(cancellationToken),

            MediaTypeNames.Application.Xml or
            MediaTypeNamesX.Application.ProblemXml => await httpContent.FromXmlAsync<TResponse>(cancellationToken),

            _ => throw new NotSupportedException($"`{contentType.MediaType}` is not supported for automatic deserialization. Please use a more specific method."),
        };
    }

    /// <inheritdoc cref="StreamExtensions.FromJson{TEntity}(Stream, JsonSerializerSettings)"/>
    public static async Task<TResponse> FromJsonAsync<TResponse>(this HttpContent httpContent, CancellationToken cancellationToken = default) where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(httpContent);

        using var stream = await httpContent.ReadAsStreamAsync(cancellationToken);
        return stream.FromJson<TResponse>();
    }

    /// <inheritdoc cref="StreamExtensions.FromXmlAsync{TEntity}(Stream, CancellationToken)"/>
    public static async Task<TResponse> FromXmlAsync<TResponse>(this HttpContent httpContent, CancellationToken cancellationToken = default) where TResponse : class
    {
        ArgumentNullException.ThrowIfNull(httpContent);

        using var stream = await httpContent.ReadAsStreamAsync(cancellationToken);
        return await stream.FromXmlAsync<TResponse>(cancellationToken);
    }
}
