using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CoreSharp.Models.Newtonsoft;
using CoreSharp.Sources;
using Newtonsoft.Json;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Truncates string.
        /// </summary>
        /// <param name="input">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        public static string Truncate(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be a positive and non-zero.");

            int maxLength = Math.Min(input.Length, length);
            return input.Substring(0, maxLength);
        }

        /// <summary>
        /// Replace each ASCII control character with its corresponding abbreviation.
        /// </summary> 
        public static string FormatAsciiControls(this string input, char openBracket = '<', char closeBracked = '>')
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            var formatedControls = new Dictionary<string, string>();
            foreach (var control in AsciiControls.Dictionary)
                formatedControls.Add(control.Value.ToString(), $"{openBracket}{control.Key}{closeBracked}");

            foreach (var control in formatedControls)
                input = input.Replace(control.Key, control.Value);

            return input;
        }

        /// <summary>
        /// Split text into fixed-length chuncks.
        /// </summary>
        public static IEnumerable<string> SplitChunks(this string input, int chunkSize)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (chunkSize < 1)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), $"{nameof(chunkSize)} has to be greater than 0.");

            return input.SplitChunksInternal(chunkSize);
        }

        private static IEnumerable<string> SplitChunksInternal(this string input, int chunkSize)
        {
            int index = 0;

            while ((index + chunkSize) < input.Length)
            {
                yield return input.Substring(index, chunkSize);
                index += chunkSize;
            }

            yield return input[index..];
        }

        /// <summary>
        /// Center align text.
        /// </summary> 
        public static string PadCenter(this string input, int totalWidth, char paddingChar = ' ')
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException(nameof(totalWidth), $"{nameof(totalWidth)} has to be zero or greater.");

            int padding = totalWidth - input.Length;
            int padLeft = padding / 2 + input.Length;

            return input.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }

        /// <summary>
        /// Removes the first occurence of a given value. 
        /// </summary>
        public static string RemoveFirst(this string input, string value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            value = value ?? throw new ArgumentNullException(nameof(value));

            int index = input.IndexOf(value);
            if (index >= 0)
                input = input.Remove(index, value.Length);

            return input;
        }

        /// <summary>
        /// Remove all the occurences of a given value. 
        /// </summary>
        public static string RemoveAll(this string input, string value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            value = value ?? throw new ArgumentNullException(nameof(value));

            return input.Replace(value, string.Empty);
        }

        /// <summary>
        /// Take left N characters. Similar to Sql.Functions.Left.
        /// </summary>
        public static string Left(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");

            if (length <= input.Length)
                return input.Substring(0, length);
            else
                return input;
        }

        /// <summary>
        /// Take right N characters. Similar to Sql.Functions.Right.
        /// </summary>
        public static string Right(this string input, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} has to be greater than 0.");

            if (length <= input.Length)
            {
                int start = input.Length - length;
                return input.Substring(start, length);
            }
            else
                return input;
        }

        /// <summary>
        /// Take substring from given index. Similar to Sql.Functions.Mid.
        /// </summary>
        public static string Mid(this string input, int start)
        {
            return input.Mid(start, input?.Length ?? 0);
        }

        /// <summary>
        /// Take N characters from given index. Similar to Sql.Functions.Mid.
        /// </summary>
        public static string Mid(this string input, int start, int length)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
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

        /// <summary>
        /// String.Format with custom IFormatProvider setting.
        /// </summary>
        public static string FormatWith(this string format, params object[] parameters)
        {
            return format.FormatWith(CultureInfo.DefaultThreadCurrentCulture, parameters);
        }

        /// <summary>
        /// String.Format with custom IFormatProvider setting.
        /// </summary>
        public static string FormatWith(this string format, IFormatProvider formatProvider, params object[] parameters)
        {
            format = format ?? throw new ArgumentNullException(nameof(format));
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
            parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));

            return string.Format(formatProvider, format, parameters);
        }

        /// <summary>
        /// String.Format with InvariantCulture.
        /// </summary>
        public static string FormatWithCI(this string format, params object[] parameters)
        {
            return format.FormatWith(CultureInfo.InvariantCulture, parameters);
        }

        /// <summary>
        /// Check if given input equals to any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EqualsAnyCI(this string input, IEnumerable<string> values)
        {
            return input.EqualsAnyCI(values?.ToArray());
        }

        /// <summary>
        /// Check if given input equals to any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EqualsAnyCI(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.Equals(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given input starts with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool StartsWithAnyCI(this string input, IEnumerable<string> values)
        {
            return input.StartsWithAnyCI(values?.ToArray());
        }

        /// <summary>
        /// Check if given input starts with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool StartsWithAnyCI(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.StartsWith(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given input ends with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EndsWithAnyCI(this string input, IEnumerable<string> values)
        {
            return input.EndsWithAnyCI(values?.ToArray());
        }

        /// <summary>
        /// Check if given input ends with any of the given values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool EndsWithAnyCI(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.EndsWith(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if given Input contains any of the given Values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool ContainsAnyCI(this string input, IEnumerable<string> values)
        {
            return input.ContainsAnyCI(values?.ToArray());
        }

        /// <summary>
        /// Check if given Input contains any of the given Values (StringComparison.InvariantCultureIgnoreCase).
        /// </summary>
        public static bool ContainsAnyCI(this string input, params string[] values)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            values = values ?? throw new ArgumentNullException(nameof(values));

            return values.Any(v => input.Contains(v, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check if input is null or empty. 
        /// </summary>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Check if input is null or whitespace. 
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Reverse a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(this string input)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            var array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        /// <summary>
        /// Erase given value from String.
        /// </summary>
        public static string Erase(this string input, char value)
        {
            return input.Erase($"{value}");
        }

        /// <summary>
        /// Erase given value from string.
        /// </summary>
        public static string Erase(this string input, string value)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            return input.Replace(value, string.Empty);
        }

        /// <summary>
        /// Trim with null check. 
        /// </summary> 
        public static string SafeTrim(this string input, params char[] trimChars)
        {
            trimChars = trimChars ?? throw new ArgumentNullException(nameof(trimChars));

            input ??= string.Empty;
            input = input.Trim();
            input = input.Trim(trimChars);
            return input;
        }

        /// <summary>
        /// Parse json to entity. 
        /// </summary> 
        public static TEntity ToEntity<TEntity>(this string json) where TEntity : class
        {
            var settings = new JsonSerializerDefaultSettings();
            return json.ToEntity<TEntity>(settings);
        }

        /// <summary>
        /// Parse json to entity. 
        /// </summary> 
        public static TEntity ToEntity<TEntity>(this string json, JsonSerializerSettings settings) where TEntity : class
        {
            settings = settings ?? throw new ArgumentNullException(nameof(settings));

            try
            {
                return JsonConvert.DeserializeObject<TEntity>(json, settings);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Split string to array of lines on new line indication. 
        /// </summary> 
        public static IEnumerable<string> GetLines(this string input, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));

            return input.Split(new[] { "\r\n", "\r", "\n" }, stringSplitOptions);
        }

        /// <summary>
        /// Replace dictionary entries in string. 
        /// </summary> 
        public static string Replace<TValue>(this string input, IDictionary<string, TValue> dictionary)
        {
            input = input ?? throw new ArgumentNullException(nameof(input));
            dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));

            foreach (var pair in dictionary)
                input = input.Replace(pair.Key, $"{pair.Value}");

            return input;
        }

        /// <summary>
        /// User-friendly int.TryParse resulting to int?.
        /// </summary> 
        public static int? ToInt(this string input)
        {
            return input.ToInt(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly int.TryParse resulting to int?.
        /// </summary> 
        public static int? ToInt(this string input, NumberStyles numberStyles)
        {
            return input.ToInt(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly int.TryParse resulting to int?.
        /// </summary> 
        public static int? ToInt(this string input, IFormatProvider formatProvider)
        {
            return input.ToInt(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly int.TryParse resulting to int?.
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static int? ToIntCI(this string input)
        {
            return input.ToInt(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly int.TryParse resulting to int?.
        /// </summary> 
        public static int? ToInt(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (int.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// User-friendly long.TryParse resulting to long?.
        /// </summary> 
        public static long? ToLong(this string input)
        {
            return input.ToLong(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly long.TryParse resulting to long?.
        /// </summary> 
        public static long? ToLong(this string input, NumberStyles numberStyles)
        {
            return input.ToLong(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly long.TryParse resulting to long?.
        /// </summary> 
        public static long? ToLong(this string input, IFormatProvider formatProvider)
        {
            return input.ToLong(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly long.TryParse resulting to long?.
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static long? ToLongCI(this string input)
        {
            return input.ToLong(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly long.TryParse resulting to long?.
        /// </summary> 
        public static long? ToLong(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (long.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// User-friendly short.TryParse resulting to short?.
        /// </summary> 
        public static short? ToShort(this string input)
        {
            return input.ToShort(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly short.TryParse resulting to short?.
        /// </summary> 
        public static short? ToShort(this string input, NumberStyles numberStyles)
        {
            return input.ToShort(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly short.TryParse resulting to short?.
        /// </summary> 
        public static short? ToShort(this string input, IFormatProvider formatProvider)
        {
            return input.ToShort(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly short.TryParse resulting to short?.
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static short? ToShortCI(this string input)
        {
            return input.ToShort(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly short.TryParse resulting to short?.
        /// </summary> 
        public static short? ToShort(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (short.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// User-friendly float.TryParse resulting to float?.
        /// </summary> 
        public static float? ToFloat(this string input)
        {
            return input.ToFloat(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly float.TryParse resulting to float?.
        /// </summary> 
        public static float? ToFloat(this string input, NumberStyles numberStyles)
        {
            return input.ToFloat(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly float.TryParse resulting to float?.
        /// </summary> 
        public static float? ToFloat(this string input, IFormatProvider formatProvider)
        {
            return input.ToFloat(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly float.TryParse resulting to float?.
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static float? ToFloatCI(this string input)
        {
            return input.ToFloat(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly float.TryParse resulting to float?.
        /// </summary> 
        public static float? ToFloat(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (float.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// User-friendly double.TryParse resulting to double?.
        /// </summary> 
        public static double? ToDouble(this string input)
        {
            return input.ToDouble(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly double.TryParse resulting to double?.
        /// </summary> 
        public static double? ToDouble(this string input, NumberStyles numberStyles)
        {
            return input.ToDouble(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly double.TryParse resulting to double?.
        /// </summary> 
        public static double? ToDouble(this string input, IFormatProvider formatProvider)
        {
            return input.ToDouble(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly double.TryParse resulting to double?. 
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static double? ToDoubleCI(this string input)
        {
            return input.ToDouble(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly double.TryParse resulting to double?.
        /// </summary> 
        public static double? ToDouble(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (double.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// User-friendly decimal.TryParse resulting to decimal?.
        /// </summary> 
        public static decimal? ToDecimal(this string input)
        {
            return input.ToDecimal(NumberStyles.None);
        }

        /// <summary>
        /// User-friendly decimal.TryParse resulting to decimal?.
        /// </summary> 
        public static decimal? ToDecimal(this string input, NumberStyles numberStyles)
        {
            return input.ToDecimal(numberStyles, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly decimal.TryParse resulting to decimal?.
        /// </summary> 
        public static decimal? ToDecimal(this string input, IFormatProvider formatProvider)
        {
            return input.ToDecimal(NumberStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly decimal.TryParse resulting to decimal?.
        /// Uses NumberStyles.Any and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static decimal? ToDecimalCI(this string input)
        {
            return input.ToDecimal(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly decimal.TryParse resulting to decimal?.
        /// </summary> 
        public static decimal? ToDecimal(this string input, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (decimal.TryParse(input, numberStyles, formatProvider, out var result))
                return result;
            else
                return null;
        }

        /// <summary>
        /// Convert string to bool. 
        /// Can convert "true/false", "1/0" and "yes/no" strings. 
        /// </summary> 
        public static bool? ToBool(this string input)
        {
            input ??= string.Empty;
            input = input.ToLowerInvariant().Trim();

            var intInput = input.ToIntCI();

            if (bool.TryParse(input, out var result))
                return result;

            //1 or 0 
            else if (intInput.HasValue && intInput == 1)
                return true;
            else if (intInput.HasValue && intInput == 0)
                return false;

            //true or false 
            else if (input == "true")
                return true;
            else if (input == "false")
                return false;

            //Yes or no 
            else if (input == "yes")
                return true;
            else if (input == "no")
                return false;

            //Not valid 
            else
                return null;
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// </summary> 
        public static DateTime? ToDateTime(this string input, string dateTimeFormat)
        {
            return input.ToDateTime(dateTimeFormat, DateTimeStyles.None);
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// </summary> 
        public static DateTime? ToDateTime(this string input, string dateTimeFormat, DateTimeStyles dateTimeStyle)
        {
            return input.ToDateTime(dateTimeFormat, dateTimeStyle, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// </summary> 
        public static DateTime? ToDateTime(this string input, string dateTimeFormat, IFormatProvider formatProvider)
        {
            return input.ToDateTime(dateTimeFormat, DateTimeStyles.None, formatProvider);
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// Uses "u" format and CultureInfo.InvariantCulture. 
        /// </summary> 
        public static DateTime? ToDateTimeSortable(this string input)
        {
            return input.ToDateTime("u", DateTimeStyles.None, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// Uses "o" format, CultureInfo.InvariantCulture and ToUniversalTime(). 
        /// </summary> 
        public static DateTime? ToDateTimeSortableUtc(this string input)
        {
            return input.ToDateTime("o", DateTimeStyles.None, CultureInfo.InvariantCulture)?.ToUniversalTime();
        }

        /// <summary>
        /// User-friendly DateTime.TryParse resulting to DateTime?. 
        /// </summary> 
        public static DateTime? ToDateTime(this string input, string dateTimeFormat, DateTimeStyles dateTimeStyle, IFormatProvider formatProvider)
        {
            dateTimeFormat = dateTimeFormat ?? throw new ArgumentNullException(nameof(dateTimeFormat));
            formatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));

            if (DateTime.TryParseExact(input, dateTimeFormat, formatProvider, dateTimeStyle, out var result))
                return result;
            else
                return null;
        }
    }
}