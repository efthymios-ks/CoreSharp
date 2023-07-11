using CoreSharp.Json.TextJson;
using CoreSharp.Sources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="string"/> extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncates <see cref="string"/>.
    /// </summary>
    public static string Truncate(this string input, int length)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be a positive and non-zero.");
        }

        var maxLength = Math.Min(input.Length, length);
        return input[..maxLength];
    }

    /// <summary>
    /// Replace each ASCII control character with its corresponding abbreviation.
    /// </summary>
    public static string FormatAsciiControls(this string input, char openBracket = '<', char closeBracket = '>')
    {
        ArgumentNullException.ThrowIfNull(input);

        var formattedControls = new Dictionary<string, string>();
        foreach (var (key, value) in AsciiControls.Dictionary)
        {
            formattedControls.Add($"{value}", $"{openBracket}{key}{closeBracket}");
        }

        return formattedControls.Aggregate(input, (current, control) => current.Replace(control.Key, control.Value));
    }

    /// <summary>
    /// Center align text.
    /// </summary>
    public static string PadCenter(this string input, int totalWidth, char paddingChar = ' ')
    {
        ArgumentNullException.ThrowIfNull(input);

        if (totalWidth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalWidth), $"{nameof(totalWidth)} has to be zero or greater.");
        }

        var padding = totalWidth - input.Length;
        var padLeft = padding / 2 + input.Length;

        return input.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
    }

    /// <summary>
    /// Removes the first occurence of a given value.
    /// </summary>
    public static string RemoveFirst(this string input, string value)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(value);

        var index = input.IndexOf(value, StringComparison.Ordinal);
        if (index >= 0)
        {
            input = input.Remove(index, value.Length);
        }

        return input;
    }

    /// <summary>
    /// Removes the last occurence of a given value.
    /// </summary>
    public static string RemoveLast(this string input, string value)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(value);

        var index = input.LastIndexOf(value, StringComparison.Ordinal);
        if (index >= 0)
        {
            input = input.Remove(index, value.Length);
        }

        return input;
    }

    /// <summary>
    /// Remove all the occurrences of a given value.
    /// </summary>
    public static string RemoveAll(this string input, string value)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(value);

        return input.Replace(value, string.Empty);
    }

    /// <summary>
    /// Take left N characters. Similar to Sql.Functions.Left.
    /// </summary>
    public static string Left(this string input, int length)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");
        }

        return length <= input.Length ? input[..length] : input;
    }

    /// <summary>
    /// Take right N characters. Similar to Sql.Functions.Right.
    /// </summary>
    public static string Right(this string input, int length)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");
        }

        if (length > input.Length)
        {
            return input;
        }

        var start = input.Length - length;
        return input.Substring(start, length);
    }

    /// <inheritdoc cref="Mid(string, int, int)"/>
    public static string Mid(this string input, int start)
        => input.Mid(start, input?.Length ?? 0);

    /// <summary>
    /// Take N characters from given index. Similar to Sql.Functions.Mid.
    /// </summary>
    public static string Mid(this string input, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), $"{nameof(start)} has to be greater than 0.");
        }
        else if (start > input.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(start), $"{nameof(start)} cannot be greater than {nameof(input)}.{input.Length} ({input.Length}).");
        }
        else if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");
        }

        return start + length > input.Length ? input[start..] : input.Substring(start, length);
    }

    /// <inheritdoc cref="EqualsAnyCI(string, string[])"/>
    public static bool EqualsAnyCI(this string input, IEnumerable<string> values)
        => input.EqualsAnyCI(values?.ToArray());

    /// <summary>
    /// Check if given input equals to any of
    /// the given values <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
    /// </summary>
    public static bool EqualsAnyCI(this string input, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(values);

        return values.Any(v => input.Equals(v, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc cref="StartsWithAnyCI(string, string[])"/>
    public static bool StartsWithAnyCI(this string input, IEnumerable<string> values)
        => input.StartsWithAnyCI(values?.ToArray());

    /// <summary>
    /// Check if given input starts with any of
    /// the given values <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
    /// </summary>
    public static bool StartsWithAnyCI(this string input, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(values);

        return values.Any(v => input.StartsWith(v, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc cref="EndsWithAnyCI(string, string[])"/>
    public static bool EndsWithAnyCI(this string input, IEnumerable<string> values)
        => input.EndsWithAnyCI(values?.ToArray());

    /// <summary>
    /// Check if given input ends with any of
    /// the given values <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
    /// </summary>
    public static bool EndsWithAnyCI(this string input, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(values);

        return values.Any(v => input.EndsWith(v, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc cref="ContainsAnyCI(string, string[])"/>
    public static bool ContainsAnyCI(this string input, IEnumerable<string> values)
        => input.ContainsAnyCI(values?.ToArray());

    /// <summary>
    /// Check if given input contains any of
    /// the given values <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
    /// </summary>
    public static bool ContainsAnyCI(this string input, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(values);

        return values.Any(v => input.Contains(v, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc cref="string.IsNullOrEmpty(string?)"/>
    public static bool IsNullOrEmpty(this string input)
        => string.IsNullOrEmpty(input);

    /// <inheritdoc cref="string.IsNullOrWhiteSpace(string?)"/>
    public static bool IsNullOrWhiteSpace(this string input)
        => string.IsNullOrWhiteSpace(input);

    /// <summary>
    /// Reverse <see cref="string.IsNullOrWhiteSpace(string?)"/>.
    /// </summary>
    public static bool HasValue(this string input)
        => !string.IsNullOrWhiteSpace(input);

    /// <summary>
    /// Return given input or <see cref="string.Empty"/> if null.
    /// </summary>
    public static string OrEmpty(this string input)
        => input ?? string.Empty;

    /// <summary>
    /// Reverse a <see cref="string"/>.
    /// </summary>
    public static string Reverse(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var array = input.ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }

    /// <inheritdoc cref="Erase(string, string)"/>
    public static string Erase(this string input, char value)
        => input.Erase($"{value}");

    /// <summary>
    /// Erase given value from <see cref="string"/>.
    /// </summary>
    public static string Erase(this string input, string value)
    {
        ArgumentNullException.ThrowIfNull(input);

        return input.Replace(value, string.Empty);
    }

    /// <summary>
    /// Trim with null check.
    /// </summary>
    public static string SafeTrim(this string input, params char[] trimChars)
    {
        ArgumentNullException.ThrowIfNull(trimChars);

        input ??= string.Empty;
        input = input.Trim();
        input = input.Trim(trimChars);
        return input;
    }

    /// <inheritdoc cref="FromJson(string, Type)"/>
    public static TEntity FromJson<TEntity>(this string json)
        where TEntity : class
        => json.FromJson(typeof(TEntity)) as TEntity;

    /// <inheritdoc cref="FromJson(string, Type, TextJson.JsonSerializerOptions)"/>
    public static object FromJson(this string json, Type entityType)
        => json.FromJson(entityType, JsonOptions.Default);

    /// <inheritdoc cref="FromJson(string, Type, JsonNet.JsonSerializerSettings)"/>
    public static TEntity FromJson<TEntity>(this string json, JsonNet.JsonSerializerSettings settings)
        where TEntity : class
       => json.FromJson(typeof(TEntity), settings) as TEntity;

    /// <inheritdoc cref="StreamExtensions.FromJson{TEntity}(Stream, JsonNet.JsonSerializerSettings)"/>
    public static object FromJson(this string json, Type entityType, JsonNet.JsonSerializerSettings settings)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(settings);

        try
        {
            return JsonNet.JsonConvert.DeserializeObject(json, entityType, settings);
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc cref="FromJson(string, Type, TextJson.JsonSerializerOptions)"/>
    public static TEntity FromJson<TEntity>(this string json, TextJson.JsonSerializerOptions options)
        where TEntity : class
       => json.FromJson(typeof(TEntity), options) as TEntity;

    /// <inheritdoc cref="StreamExtensions.FromJsonAsync(Stream, TextJson.JsonSerializerOptions)"/>
    public static object FromJson(this string json, Type entityType, TextJson.JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        ArgumentNullException.ThrowIfNull(options);

        try
        {
            return TextJson.JsonSerializer.Deserialize(json, entityType, options);
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc cref="StreamExtensions.FromXmlAsync{TEntity}(Stream, CancellationToken)"/>
    public static TEntity FromXml<TEntity>(this string xml)
        where TEntity : class
        => XDocument.Parse(xml).ToEntity<TEntity>();

    /// <summary>
    /// Convert JSON to <see cref="ExpandoObject"/>.
    /// </summary>
    public static dynamic ToExpandoObject(this string json)
    {
        var token = JToken.Parse(json);

        return token switch
        {
            JArray => token.ToObject<IEnumerable<ExpandoObject>>(),
            JObject => token.ToObject<ExpandoObject>(),
            _ => throw new InvalidOperationException($"{nameof(json)} is not in a valid json format.")
        };
    }

    /// <summary>
    /// Split string to array of lines on new line indication.
    /// </summary>
    public static IEnumerable<string> GetLines(this string input, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
    {
        ArgumentNullException.ThrowIfNull(input);

        return input.Split(new[] { "\r", "\n", "\r\n", Environment.NewLine }, stringSplitOptions);
    }

    /// <inheritdoc cref="ToInt(string, NumberStyles, IFormatProvider)"/>
    public static int? ToInt(this string input)
        => input.ToInt(NumberStyles.Number);

    /// <inheritdoc cref="ToInt(string, NumberStyles, IFormatProvider)"/>
    public static int? ToInt(this string input, NumberStyles numberStyles)
        => input.ToInt(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToInt(string, NumberStyles, IFormatProvider)"/>
    public static int? ToInt(this string input, IFormatProvider formatProvider)
        => input.ToInt(NumberStyles.Number, formatProvider);

    /// <summary>
    /// User-friendly <see cref="int.TryParse(string?, NumberStyles, IFormatProvider?, out int)"/> resulting to <see cref="int"/>.
    /// </summary>
    public static int? ToInt(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return int.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToInt(string, NumberStyles, IFormatProvider)"/>
    public static int? ToIntCI(this string input)
        => input.ToInt(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
    public static long? ToLong(this string input)
        => input.ToLong(NumberStyles.Number);

    /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
    public static long? ToLong(this string input, NumberStyles numberStyles)
        => input.ToLong(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
    public static long? ToLong(this string input, IFormatProvider formatProvider)
        => input.ToLong(NumberStyles.Number, formatProvider);

    /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
    public static long? ToLong(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return long.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
    public static long? ToLongCI(this string input)
        => input.ToLong(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
    public static short? ToShort(this string input)
        => input.ToShort(NumberStyles.Number);

    /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
    public static short? ToShort(this string input, NumberStyles numberStyles)
        => input.ToShort(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
    public static short? ToShort(this string input, IFormatProvider formatProvider)
        => input.ToShort(NumberStyles.Number, formatProvider);

    /// <summary>
    /// User-friendly <see cref="short.TryParse(string?, NumberStyles, IFormatProvider?, out short)"/> resulting to <see cref="short"/>.
    /// </summary>
    public static short? ToShort(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return short.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
    public static short? ToShortCI(this string input)
        => input.ToShort(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
    public static float? ToFloat(this string input)
        => input.ToFloat(NumberStyles.Number);

    /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
    public static float? ToFloat(this string input, NumberStyles numberStyles)
        => input.ToFloat(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
    public static float? ToFloat(this string input, IFormatProvider formatProvider)
        => input.ToFloat(NumberStyles.Number, formatProvider);

    /// <summary>
    /// User-friendly <see cref="float.TryParse(string?, NumberStyles, IFormatProvider?, out float)"/> resulting to <see cref="float"/>.
    /// </summary>
    public static float? ToFloat(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return float.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
    public static float? ToFloatCI(this string input)
        => input.ToFloat(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
    public static double? ToDouble(this string input)
        => input.ToDouble(NumberStyles.Number);

    /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
    public static double? ToDouble(this string input, NumberStyles numberStyles)
        => input.ToDouble(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
    public static double? ToDouble(this string input, IFormatProvider formatProvider)
        => input.ToDouble(NumberStyles.Number, formatProvider);

    /// <summary>
    /// User-friendly <see cref="double.TryParse(string?, NumberStyles, IFormatProvider?, out double)"/> resulting to <see cref="double"/>.
    /// </summary>
    public static double? ToDouble(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return double.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
    public static double? ToDoubleCI(this string input)
        => input.ToDouble(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
    public static decimal? ToDecimal(this string input)
        => input.ToDecimal(NumberStyles.Number);

    /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
    public static decimal? ToDecimal(this string input, NumberStyles numberStyles)
        => input.ToDecimal(numberStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
    public static decimal? ToDecimal(this string input, IFormatProvider formatProvider)
        => input.ToDecimal(NumberStyles.Number, formatProvider);

    /// <summary>
    /// User-friendly <see cref="decimal.TryParse(string?, NumberStyles, IFormatProvider?, out decimal)"/> resulting to <see cref="decimal"/>.
    /// </summary>
    public static decimal? ToDecimal(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return decimal.TryParse(input, numberStyles, formatProvider, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
    public static decimal? ToDecimalCI(this string input)
        => input.ToDecimal(NumberStyles.Number, CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert <see cref="string"/> to <see cref="bool"/>.
    /// Can convert "true/false", "1/0" and "yes/no" strings.
    /// </summary>
    public static bool? ToBool(this string input)
    {
        input ??= string.Empty;
        input = input.ToLowerInvariant().Trim();

        // Valid boolean 
        if (bool.TryParse(input, out var result))
        {
            return result;
        }

        // String interpretation
        bool? resultFromString = input switch
        {
            "true" or "yes" => true,
            "false" or "no" => false,
            _ => null
        };

        if (resultFromString is not null)
        {
            return resultFromString;
        }

        // Int interpretation 
        return !int.TryParse(input, out var inputAsInt)
            ? null
            : inputAsInt switch
            {
                1 => true,
                0 => false,
                _ => null
            };
    }

    /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
    public static DateTime? ToDateTime(this string input, string dateTimeFormat)
       => input.ToDateTime(dateTimeFormat, DateTimeStyles.None);

    /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
    public static DateTime? ToDateTime(this string input, string dateTimeFormat, DateTimeStyles dateTimeStyles)
     => input.ToDateTime(dateTimeFormat, dateTimeStyles, CultureInfo.CurrentCulture);

    /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
    public static DateTime? ToDateTime(this string input, string dateTimeFormat, IFormatProvider formatProvider)
        => input.ToDateTime(dateTimeFormat, DateTimeStyles.None, formatProvider);

    /// <summary>
    /// User-friendly <see cref="DateTime.TryParseExact(string?, string?, IFormatProvider?, DateTimeStyles, out DateTime)"/> resulting to <see cref="DateTime"/>.
    /// </summary>
    public static DateTime? ToDateTime(this string input, string dateTimeFormat, DateTimeStyles dateTimeStyles, IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(dateTimeFormat);
        ArgumentNullException.ThrowIfNull(formatProvider);

        return DateTime.TryParseExact(input, dateTimeFormat, formatProvider, dateTimeStyles, out var result) ? result : null;
    }

    /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
    public static DateTime? ToDateTimeSortable(this string input)
        => input.ToDateTime("u", DateTimeStyles.None, CultureInfo.InvariantCulture);

    /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
    public static DateTime? ToDateTimeSortableUtc(this string input)
       => input.ToDateTime("o", DateTimeStyles.None, CultureInfo.InvariantCulture)?.ToUniversalTime();

    /// <inheritdoc cref="ToGuid(string, string)"/>>
    public static Guid? ToGuid(this string input)
        => input?.ToGuid("D");

    /// <summary>
    /// User-friendly <see cref="Guid.TryParseExact(string?, string?, out Guid)"/> resulting to <see cref="Guid"/>.
    /// </summary>
    public static Guid? ToGuid(this string input, string format)
    {
        ArgumentNullException.ThrowIfNull(format);

        return Guid.TryParseExact(input, format, out var result) ? result : null;
    }

    /// <summary>
    /// Return the one that is not <see cref="string.IsNullOrEmpty(string?)"/>.
    /// </summary>
    public static string Or(this string left, string right)
        => string.IsNullOrWhiteSpace(left) ? right : left;

    /// <summary>
    /// Return value or null if <see cref="string.IsNullOrEmpty(string?)"/>.
    /// </summary>
    public static string OrNull(this string value)
        => value.Or(null);

    /// <summary>
    /// Returns substring after first match.
    /// </summary>
    public static string SubstringAfter(this string input, string match)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrEmpty(match);

        var matchStartIndex = input.IndexOf(match, StringComparison.OrdinalIgnoreCase);
        return matchStartIndex < 0 ? null : input[(matchStartIndex + match.Length)..];
    }

    /// <summary>
    /// Returns substring after last match.
    /// </summary>
    public static string SubstringAfterLast(this string input, string match)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrEmpty(match);

        var matchStartIndex = input.LastIndexOf(match, StringComparison.OrdinalIgnoreCase);
        return matchStartIndex < 0 ? null : input[(matchStartIndex + match.Length)..];
    }

    /// <summary>
    /// Returns substring before first match.
    /// </summary>
    public static string SubstringBefore(this string input, string match)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrEmpty(match);

        var matchStartIndex = input.IndexOf(match, StringComparison.OrdinalIgnoreCase);
        return matchStartIndex < 0 ? null : input[..matchStartIndex];
    }

    /// <summary>
    /// Returns substring before last match.
    /// </summary>
    public static string SubstringBeforeLast(this string input, string match)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentException.ThrowIfNullOrEmpty(match);

        var matchStartIndex = input.LastIndexOf(match, StringComparison.OrdinalIgnoreCase);
        return matchStartIndex < 0 ? null : input[..matchStartIndex];
    }

    /// <inheritdoc cref="Format(string, IFormatProvider, object[])"/>
    public static string Format(this string format, params object[] parameters)
       => format.Format(CultureInfo.CurrentCulture, parameters);

    /// <summary>
    /// String.Format with custom IFormatProvider setting.
    /// </summary>
    public static string Format(this string format, IFormatProvider formatProvider, params object[] arguments)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        return string.Format(formatProvider, format, arguments);
    }

    /// <see cref="Format{TValue}(string, IDictionary{string, TValue}, IFormatProvider)"/>
    public static string Format<TValue>(this string format, IDictionary<string, TValue> arguments)
        => format.Format(arguments, null);

    /// <summary>
    /// Format string using named arguments. May use optional formatting.
    /// </summary>
    public static string Format<TValue>(this string format, IDictionary<string, TValue> arguments, IFormatProvider formatProvider)
    {
        if (format is null)
        {
            return null;
        }

        if (arguments?.Count is not > 0)
        {
            return format;
        }

        var openBracketIndex = FindOpenBracketFrom(0);
        while (openBracketIndex > -1)
        {
            var closeBracketIndex = FindCloseBracketFrom(openBracketIndex);
            if (closeBracketIndex < 0)
            {
                break;
            }

            // Extract placeholder metadata 
            var semiColonIndex = FindSemicolon(openBracketIndex, closeBracketIndex);
            var argumentName = ExtractName(openBracketIndex, semiColonIndex >= 0 ? semiColonIndex : closeBracketIndex);
            var argumentFormat = ExtractFormat(semiColonIndex, closeBracketIndex);

            // If key exists, replace  
            if (arguments.TryGetValue(argumentName, out var argumentValue))
            {
                format = ReplaceAtPosition(argumentValue, argumentFormat, openBracketIndex, ref closeBracketIndex);
            }

            // Find next 
            openBracketIndex = FindOpenBracketFrom(closeBracketIndex);
        }

        return format;

        // Helpers 
        int FindFromTo(char character, int startIndex, int endIndex)
            => format.IndexOf(character, startIndex, endIndex - startIndex);
        int FindFrom(char character, int startIndex)
            => FindFromTo(character, startIndex, format.Length);
        int FindOpenBracketFrom(int startIndex)
            => FindFrom('{', startIndex);
        int FindCloseBracketFrom(int startIndex)
            => FindFrom('}', startIndex);
        int FindSemicolon(int openBracketIndex, int endBracketIndex)
            => FindFromTo(':', openBracketIndex, endBracketIndex);

        string ExtractBetween(int startIndex, int endIndex)
            => format.Substring(startIndex + 1, endIndex - startIndex - 1);
        string ExtractName(int openBracketIndex, int closeBracketIndex)
            => ExtractBetween(openBracketIndex, closeBracketIndex);
        string ExtractFormat(int semiColonIndex, int closeBracketIndex)
            => semiColonIndex >= 0 ? ExtractBetween(semiColonIndex, closeBracketIndex) : null;

        string ToString(object value, string format = null)
            => value is IFormattable formattable ? formattable.ToString(format, formatProvider) : $"{value}";

        string ReplaceAtPosition(object argumentValue, string argumentFormat, int openBracketIndex, ref int closeBracketIndex)
        {
            var argumentValueAsString = ToString(argumentValue, argumentFormat);
            format = format.Remove(openBracketIndex, closeBracketIndex - openBracketIndex + 1);
            format = format.Insert(openBracketIndex, argumentValueAsString);
            closeBracketIndex = openBracketIndex + argumentValueAsString.Length;
            return format;
        }
    }

    /// <inheritdoc cref="Format(string, IFormatProvider, object[])"/>
    public static string FormatCI(this string format, params object[] parameters)
        => format.Format(CultureInfo.InvariantCulture, parameters);

    /// <see cref="FormatWithNames(string, object, IFormatProvider)"/>
    public static string FormatWithNames(this string format, object arguments)
        => format.FormatWithNames(arguments, null);

    /// <see cref="FormatWithNames(string, object, BindingFlags, IFormatProvider)"/>
    public static string FormatWithNames(this string format, object arguments, BindingFlags bindingFlags)
        => format.FormatWithNames(arguments, bindingFlags, null);

    /// <see cref="FormatWithNames(string, object, BindingFlags, IFormatProvider)"/>
    public static string FormatWithNames(this string format, object arguments, IFormatProvider formatProvider)
        => format.FormatWithNames(arguments, BindingFlags.Instance | BindingFlags.Public, formatProvider);

    /// <see cref="Format{TValue}(string, IDictionary{string, TValue}, IFormatProvider)"/>
    public static string FormatWithNames(this string format, object arguments, BindingFlags bindingFlags, IFormatProvider formatProvider)
    {
        var argumentsAsDictionary = arguments?.GetType()
                                              .GetProperties(bindingFlags)
                                              .ToDictionary(
                                                p => p.Name,
                                                p => p.GetValue(arguments),
                                                StringComparer.OrdinalIgnoreCase);
        return format.Format(argumentsAsDictionary, formatProvider);
    }

#if !NET6_0_OR_GREATER
    /// <inheritdoc cref="IEnumerableExtensions.Chunk{TItem}(IEnumerable{TItem}, int)"/>
    public static IEnumerable<string> Chunk(this string input, int size)
    {
        var chunks = input.Chunk<char>(size);
        return chunks.Select(chunk => new string(chunk.ToArray()));
    }
#endif 
}
