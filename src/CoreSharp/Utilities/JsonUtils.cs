using System;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities;

/// <summary>
/// Json utilities.
/// </summary>
public static class JsonUtils
{
    /// <summary>
    /// Check if string is an empty json.
    /// </summary>
    public static bool IsEmpty(string json)
    {
        json ??= string.Empty;

        // Remove spaces, line-breaks and whitespace
        json = Regex.Replace(json, @"\s+", string.Empty);

        // Empty formats
        var emptyFormats = new[] { "", "{}", "[]", "[{}]" };

        return Array.Exists(emptyFormats, format => format == json);
    }
}
