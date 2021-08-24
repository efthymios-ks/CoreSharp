using CoreSharp.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoreSharp.Models
{
    public class NumericParser<TValue>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CultureInfo _cultureInfo;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _format;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool _isFormatPercentage;

        //Constructors
        public NumericParser() : this(CultureInfo.CurrentCulture)
        {
        }

        public NumericParser(CultureInfo cultureInfo) : this(null, cultureInfo)
        {
        }

        public NumericParser(string format) : this(format, CultureInfo.CurrentCulture)
        {
        }

        public NumericParser(string format, CultureInfo cultureInfo)
        {
            if (!typeof(TValue).IsNumeric())
                throw new ArgumentException($"{nameof(TValue)} ({typeof(TValue).FullName}) is not a numeric type.");

            _cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
            _format = format;
            _isFormatPercentage = Regex.IsMatch(_format ?? string.Empty, @"^[pP]\d+$");
        }

        //Methods
        /// <summary>
        /// Parse and convert string input to TValue. 
        /// If fails to do so, value remains untouched. 
        /// </summary> 
        public bool TryParseValue(string input, ref TValue value)
        {
            try
            {
                //If input is empty 
                if (string.IsNullOrWhiteSpace(input))
                {
                    //Nullable: Null 
                    //Non-Nullable: 0 
                    value = default;
                    return true;
                }

                //If value given 
                else
                {
                    //Parse to decimal? (cause it can fit all numeric types) 
                    var tempValue = ParseValue(input);

                    //Validate
                    if (tempValue is null)
                        throw new NullReferenceException($"Failed to parse input=`{input}` to {typeof(TValue).GetNullableBaseType().FullName}.");

                    //Format to string... 
                    input = FormatValue(tempValue);

                    //...and parse back to decimal? to keep formatting or rounding 
                    tempValue = ParseValue(input);

                    //Finally convert to required type 
                    value = tempValue.ChangeType<TValue>(_cultureInfo);
                    return true;
                }
            }
            catch
            {
                //Revert to last valid value 
                return false;
            }
        }

        /// <summary>
        /// Parse value to decimal?. 
        /// Handles any type and format. 
        /// </summary> 
        private decimal? ParseValue(string input)
        {
            bool isValuePercentage = input.Contains("%");

            //Remove all whitespace 
            input = Regex.Replace(input, @"\s+", string.Empty);

            //Remove percentage symbol 
            if (isValuePercentage)
                input = input.Replace(_cultureInfo.NumberFormat.PercentSymbol, string.Empty);

            if (decimal.TryParse(input, NumberStyles.Any, _cultureInfo, out decimal result))
            {
                //Divive by 100, since `ToString("P")` format specifier multiplies by 100 
                if (_isFormatPercentage || isValuePercentage)
                    return result / 100;
                else
                    return result;
            }
            else
                return default;
        }

        /// <summary>
        /// Format TValue to string. 
        /// </summary> 
        public string FormatValue(TValue value) => FormatValue(value.ChangeType<decimal?>(_cultureInfo));

        /// <summary>
        /// Format decimal? to string. 
        /// </summary> 
        private string FormatValue(decimal? value) => value?.ToString(_format, _cultureInfo);
    }
}
