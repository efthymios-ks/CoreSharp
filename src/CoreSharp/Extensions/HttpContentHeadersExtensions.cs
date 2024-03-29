﻿using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="HttpContentHeaders"/> extensions.
/// </summary>
public static class HttpContentHeadersExtensions
{
    /// <summary>
    /// Extract <see cref="ContentType"/> using
    /// <see cref="HeaderNames.ContentType"/> header.
    /// If any.
    /// </summary>
    public static ContentType GetContentType(this HttpContentHeaders httpContentHeaders)
    {
        ArgumentNullException.ThrowIfNull(httpContentHeaders);

        if (!httpContentHeaders.TryGetValues(HeaderNames.ContentType, out var contentTypes))
        {
            return null;
        }

        var contentTypeAsString = contentTypes.First();
        return new ContentType(contentTypeAsString);
    }
}
