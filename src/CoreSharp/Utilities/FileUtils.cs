using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreSharp.Utilities;

/// <summary>
/// File utilities.
/// </summary>
public static class FileUtils
{
    // Fields
    private static Regex _sanitizeFileNameRegex;

    // Properties
    private static Regex SanitizeFileNameRegex
    {
        get
        {
            if (_sanitizeFileNameRegex is null)
            {
                var invalidChars = Path.GetInvalidFileNameChars()
                    .Concat(Path.GetInvalidPathChars())
                    .ToArray();
                var invalidCharsAsString = new string(invalidChars);
                var invalidCharsAsEscapedString = Regex.Escape(invalidCharsAsString);
                var invalidCharsPattern = $"[{invalidCharsAsEscapedString}]";
                _sanitizeFileNameRegex = new Regex(invalidCharsPattern, RegexOptions.Compiled);
            }

            return _sanitizeFileNameRegex;
        }
    }

    public static string SanitizeFileName(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        return SanitizeFileNameRegex.Replace(fileName, "_");
    }
}