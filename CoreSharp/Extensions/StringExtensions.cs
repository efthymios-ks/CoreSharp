using CoreSharp.Models.Newtonsoft.Settings;
using CoreSharp.Sources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace CoreSharp.Extensions
{
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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be a positive and non-zero.");

            var maxLength = Math.Min(input.Length, length);
            return input[..maxLength];
        }

        /// <summary>
        /// Replace each ASCII control character with its corresponding abbreviation.
        /// </summary>
        public static string FormatAsciiControls(this string input, char openBracket = '<', char closeBracket = '>')
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));

            var formattedControls = new Dictionary<string, string>();
            foreach (var (key, value) in AsciiControls.Dictionary)
                formattedControls.Add($"{value}", $"{openBracket}{key}{closeBracket}");

            return formattedControls.Aggregate(input, (current, control) => current.Replace(control.Key, control.Value));
        }

        /// <inheritdoc cref="IEnumerableExtensions.Chunk{TItem}(IEnumerable{TItem}, int)"/>
        public static IEnumerable<string> Chunk(this string input, int size)
        {
            var chunks = input.Chunk<char>(size);
            return chunks.Select(c => new string(c.ToArray()));
        }

        /// <summary>
        /// Center align text.
        /// </summary>
        public static string PadCenter(this string input, int totalWidth, char paddingChar = ' ')
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException(nameof(totalWidth), $"{nameof(totalWidth)} has to be zero or greater.");

            var padding = totalWidth - input.Length;
            var padLeft = (padding / 2) + input.Length;

            return input.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }

        /// <summary>
        /// Removes the first occurence of a given value.
        /// </summary>
        public static string RemoveFirst(this string input, string value)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var index = input.IndexOf(value, StringComparison.Ordinal);
            if (index >= 0)
                input = input.Remove(index, value.Length);

            return input;
        }

        /// <summary>
        /// Removes the last occurence of a given value.
        /// </summary>
        public static string RemoveLast(this string input, string value)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var index = input.LastIndexOf(value, StringComparison.Ordinal);
            if (index >= 0)
                input = input.Remove(index, value.Length);

            return input;
        }

        /// <summary>
        /// Remove all the occurrences of a given value.
        /// </summary>
        public static string RemoveAll(this string input, string value)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = value ?? throw new ArgumentNullException(nameof(value));

            return input.Replace(value, string.Empty);
        }

        /// <summary>
        /// Take left N characters. Similar to Sql.Functions.Left.
        /// </summary>
        public static string Left(this string input, int length)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");

            if (length <= input.Length)
                return input[..length];
            else
                return input;
        }

        /// <summary>
        /// Take right N characters. Similar to Sql.Functions.Right.
        /// </summary>
        public static string Right(this string input, int length)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");

            if (length > input.Length)
                return input;

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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), $"{nameof(start)} has to be greater than 0.");
            else if (start > input.Length)
                throw new ArgumentOutOfRangeException(nameof(start), $"{nameof(start)} cannot be greater than {nameof(input)}.{input.Length} ({input.Length}).");
            else if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");

            if ((start + length) > input.Length)
                return input[start..];
            else
                return input.Substring(start, length);
        }

        /// <inheritdoc cref="FormatWith(string, IFormatProvider, object[])"/>
        public static string FormatWith(this string format, params object[] parameters)
           => format.FormatWith(CultureInfo.CurrentCulture, parameters);

        /// <inheritdoc cref="FormatWith(string, IFormatProvider, object[])"/>
        public static string FormatWithCI(this string format, params object[] parameters)
            => format.FormatWith(CultureInfo.InvariantCulture, parameters);

        /// <summary>
        /// String.Format with custom IFormatProvider setting.
        /// </summary>
        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] arguments)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            return string.Format(formatProvider, format, arguments);
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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.Equals(v, StringComparison.InvariantCultureIgnoreCase));
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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = values ?? throw new ArgumentNullException(nameof(values));

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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = values ?? throw new ArgumentNullException(nameof(values));

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
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.Contains(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <inheritdoc cref="string.IsNullOrEmpty(string?)"/>
        public static bool IsNullOrEmpty(this string input)
            => string.IsNullOrEmpty(input);

        /// <inheritdoc cref="string.IsNullOrWhiteSpace(string?)"/>
        public static bool IsNullOrWhiteSpace(this string input)
            => string.IsNullOrWhiteSpace(input);

        /// <summary>
        /// Reverse a <see cref="string"/>.
        /// </summary>
        public static string Reverse(this string input)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));

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
            _ = input ?? throw new ArgumentNullException(nameof(input));

            return input.Replace(value, string.Empty);
        }

        /// <summary>
        /// Trim with null check.
        /// </summary>
        public static string SafeTrim(this string input, params char[] trimChars)
        {
            _ = trimChars ?? throw new ArgumentNullException(nameof(trimChars));

            input ??= string.Empty;
            input = input.Trim();
            input = input.Trim(trimChars);
            return input;
        }

        /// <inheritdoc cref="FromJson(string, Type, JsonSerializerSettings)"/>
        public static TEntity FromJson<TEntity>(this string json) where TEntity : class
            => json.FromJson(typeof(TEntity)) as TEntity;

        /// <inheritdoc cref="FromJson(string, Type, JsonSerializerSettings)"/>
        public static object FromJson(this string json, Type entityType)
            => json.FromJson(entityType, DefaultJsonSettings.Instance);

        /// <inheritdoc cref="FromJson(string, Type, JsonSerializerSettings)"/>
        public static TEntity FromJson<TEntity>(this string json, JsonSerializerSettings settings) where TEntity : class
           => json.FromJson(typeof(TEntity), settings) as TEntity;

        /// <inheritdoc cref="StreamExtensions.FromJson(Stream)"/>
        public static object FromJson(this string json, Type entityType, JsonSerializerSettings settings)
        {
            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            try
            {
                return JsonConvert.DeserializeObject(json, entityType, settings);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc cref="StreamExtensions.FromXmlAsync{TEntity}(Stream, CancellationToken)"/>
        public static TEntity FromXml<TEntity>(this string xml) where TEntity : class
            => XDocument.Parse(xml).To<TEntity>();

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
            _ = input ?? throw new ArgumentNullException(nameof(input));

            return input.Split(new[] { "\r", "\n", "\r\n", Environment.NewLine }, stringSplitOptions);
        }

        /// <summary>
        /// Replace dictionary entries in string.
        /// </summary>
        public static string Replace<TValue>(this string input, IDictionary<string, TValue> dictionary)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            foreach (var (key, value) in dictionary)
                input = input.Replace(key, $"{value}");

            return input;
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

        /// <inheritdoc cref="ToInt(string, NumberStyles, IFormatProvider)"/>
        public static int? ToIntCI(this string input)
            => input.ToInt(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>
        /// User-friendly <see cref="int.TryParse(string?, NumberStyles, IFormatProvider?, out int)"/> resulting to <see cref="int"/>.
        /// </summary>
        public static int? ToInt(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (int.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

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
        public static long? ToLongCI(this string input)
            => input.ToLong(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <inheritdoc cref="ToLong(string, NumberStyles, IFormatProvider)"/>
        public static long? ToLong(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (long.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
        public static short? ToShort(this string input)
            => input.ToShort(NumberStyles.Number);

        /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
        public static short? ToShort(this string input, NumberStyles numberStyles)
            => input.ToShort(numberStyles, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
        public static short? ToShort(this string input, IFormatProvider formatProvider)
            => input.ToShort(NumberStyles.Number, formatProvider);

        /// <inheritdoc cref="ToShort(string, NumberStyles, IFormatProvider)"/>
        public static short? ToShortCI(this string input)
            => input.ToShort(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>
        /// User-friendly <see cref="short.TryParse(string?, NumberStyles, IFormatProvider?, out short)"/> resulting to <see cref="short"/>.
        /// </summary>
        public static short? ToShort(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (short.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
        public static float? ToFloat(this string input)
            => input.ToFloat(NumberStyles.Number);

        /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
        public static float? ToFloat(this string input, NumberStyles numberStyles)
            => input.ToFloat(numberStyles, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
        public static float? ToFloat(this string input, IFormatProvider formatProvider)
            => input.ToFloat(NumberStyles.Number, formatProvider);

        /// <inheritdoc cref="ToFloat(string, NumberStyles, IFormatProvider)"/>
        public static float? ToFloatCI(this string input)
            => input.ToFloat(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>
        /// User-friendly <see cref="float.TryParse(string?, NumberStyles, IFormatProvider?, out float)"/> resulting to <see cref="float"/>.
        /// </summary>
        public static float? ToFloat(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (float.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
        public static double? ToDouble(this string input)
            => input.ToDouble(NumberStyles.Number);

        /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
        public static double? ToDouble(this string input, NumberStyles numberStyles)
            => input.ToDouble(numberStyles, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
        public static double? ToDouble(this string input, IFormatProvider formatProvider)
            => input.ToDouble(NumberStyles.Number, formatProvider);

        /// <inheritdoc cref="ToDouble(string, NumberStyles, IFormatProvider)"/>
        public static double? ToDoubleCI(this string input)
            => input.ToDouble(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>
        /// User-friendly <see cref="double.TryParse(string?, NumberStyles, IFormatProvider?, out double)"/> resulting to <see cref="double"/>.
        /// </summary>
        public static double? ToDouble(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (double.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
        public static decimal? ToDecimal(this string input)
            => input.ToDecimal(NumberStyles.Number);

        /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
        public static decimal? ToDecimal(this string input, NumberStyles numberStyles)
            => input.ToDecimal(numberStyles, CultureInfo.CurrentCulture);

        /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
        public static decimal? ToDecimal(this string input, IFormatProvider formatProvider)
            => input.ToDecimal(NumberStyles.Number, formatProvider);

        /// <inheritdoc cref="ToDecimal(string, NumberStyles, IFormatProvider)"/>
        public static decimal? ToDecimalCI(this string input)
            => input.ToDecimal(NumberStyles.Number, CultureInfo.InvariantCulture);

        /// <summary>
        /// User-friendly <see cref="decimal.TryParse(string?, NumberStyles, IFormatProvider?, out decimal)"/> resulting to <see cref="decimal"/>.
        /// </summary>
        public static decimal? ToDecimal(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (decimal.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// Convert <see cref="string"/> to <see cref="bool"/>.
        /// Can convert "true/false", "1/0" and "yes/no" strings.
        /// </summary>
        public static bool? ToBool(this string input)
        {
            input ??= string.Empty;
            input = input.ToLowerInvariant().Trim();

            if (bool.TryParse(input, out var result))
                return result;

            var inputAsInt = input.ToIntCI();
            return inputAsInt switch
            {
                //1 or 0 
                1 => true,
                0 => false,
                _ => input switch
                {
                    //true or false 
                    "true" => true,
                    "false" => false,

                    //Yes or no 
                    "yes" => true,
                    "no" => false,

                    _ => null
                }
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

        /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
        public static DateTime? ToDateTimeSortable(this string input)
            => input.ToDateTime("u", DateTimeStyles.None, CultureInfo.InvariantCulture);

        /// <inheritdoc cref="ToDateTime(string, string, DateTimeStyles, IFormatProvider)"/>
        public static DateTime? ToDateTimeSortableUtc(this string input)
           => input.ToDateTime("o", DateTimeStyles.None, CultureInfo.InvariantCulture)?.ToUniversalTime();

        /// <summary>
        /// User-friendly <see cref="DateTime.TryParseExact(string?, string?, IFormatProvider?, DateTimeStyles, out DateTime)"/> resulting to <see cref="DateTime"/>.
        /// </summary>
        public static DateTime? ToDateTime(this string input, string dateTimeFormat, DateTimeStyles dateTimeStyles, IFormatProvider formatProvider)
        {
            _ = dateTimeFormat ?? throw new ArgumentNullException(nameof(dateTimeFormat));
            _ = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (DateTime.TryParseExact(input, dateTimeFormat, formatProvider, dateTimeStyles, out var result))
                return result;
            else
                return null;
        }

        /// <inheritdoc cref="ToGuid(string, string)"/>>
        public static Guid? ToGuid(this string input)
            => input?.ToGuid("D");

        /// <summary>
        /// User-friendly <see cref="Guid.TryParseExact(string?, string?, out Guid)"/> resulting to <see cref="Guid"/>.
        /// </summary>
        public static Guid? ToGuid(this string input, string format)
        {
            _ = format ?? throw new ArgumentNullException(nameof(format));

            if (Guid.TryParseExact(input, format, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// Return the one that is not <see cref="string.IsNullOrEmpty(string?)"/>.
        /// </summary>
        public static string Or(this string left, string right)
            => !string.IsNullOrEmpty(left) ? left : right;
    }
}
