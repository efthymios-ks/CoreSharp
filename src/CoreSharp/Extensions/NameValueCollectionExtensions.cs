using CoreSharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="NameValueCollection"/> extensions.
/// </summary>
public static class NameValueCollectionExtensions
{
    /// <inheritdoc cref="IDictionaryExtensions.ToUrlQueryString{TValue}(IDictionary{string, TValue})"/>
    public static string ToUrlQueryString(this NameValueCollection source)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        var dictionary = source.AllKeys.ToDictionary(key => key, key => source[key]);
        var builder = new UrlQueryBuilder
        {
            dictionary
        };
        return builder.ToString();
    }
}
