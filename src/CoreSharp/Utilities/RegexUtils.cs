using System;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Regex"/> utilities.
/// </summary>
public static class RegexUtils
{
    // Methods
    /// <summary>
    /// Reduces many consequtive occurences of a char to single one.
    /// <code>
    /// var input = "11.2.3.4";
    /// var result = RegexX.ReduceMany(input, '1'); // 1.2.3.4
    /// </code>
    /// </summary>
    public static string ReduceMany(string input, char occurence)
    {
        ArgumentNullException.ThrowIfNull(input);

        var escapedOccurence = Regex.Escape($"{occurence}");
        var pattern = $"{escapedOccurence}{{2,}}";
        return Regex.Replace(input, pattern, $"{occurence}");
    }
}
