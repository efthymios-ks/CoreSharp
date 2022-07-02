using CoreSharp.Utilities;
using System;
using System.Collections.Generic;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Uri"/> extensions.
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Get <see cref="Uri.Query"/> parameters.
    /// </summary>
    public static IDictionary<string, string> GetQueryParameters(this Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));

        return UriX.GetParameters(uri.Query);
    }

    /// <summary>
    /// Get <see cref="Uri.Fragment"/> parameters.
    /// </summary>
    public static IDictionary<string, string> GetFragmentParameters(this Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));

        return UriX.GetParameters(uri.Fragment.TrimStart('#'));
    }
}
