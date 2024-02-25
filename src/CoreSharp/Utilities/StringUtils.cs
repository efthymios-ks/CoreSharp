using CoreSharp.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="string"/> utilities.
/// </summary>
public static class StringUtils
{
    /// <inheritdoc cref="FirstNotEmpty(string[])"/>
    public static string FirstNotEmpty(IEnumerable<string> values)
        => FirstNotEmpty(values?.ToArray());

    /// <summary>
    /// Return first value not null or whitespace.
    /// </summary>
    public static string FirstNotEmpty(params string[] values)
        => values?.Aggregate((accumulated, next) => accumulated.Or(next));
}
