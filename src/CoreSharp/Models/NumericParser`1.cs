using CoreSharp.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoreSharp.Models;

public sealed class NumericParser<TNumber>
{
    //Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CultureInfo _cultureInfo;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string _format;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool _isFormatPercentage;

    //Constructors
    public NumericParser()
        : this(CultureInfo.CurrentCulture)
    {
    }

    public NumericParser(CultureInfo cultureInfo)
        : this(null, cultureInfo)
    {
    }

    public NumericParser(string format)
        : this(format, CultureInfo.CurrentCulture)
    {
    }

    public NumericParser(string format, CultureInfo cultureInfo)
    {
        if (!typeof(TNumber).IsNumeric())
            throw new ArgumentException($"{nameof(TNumber)} ({typeof(TNumber).FullName}) is not a numeric type.");

        _cultureInfo = cultureInfo ?? throw new ArgumentNullException(nameof(cultureInfo));
        _format = format ?? string.Empty;
        _isFormatPercentage = Regex.IsMatch(_format, @"^[pP]\d+$");
    }

    //Methods
    /// <summary>
    /// Parse and convert <see cref="string"/> input to <see cref="decimal"/>.
    /// If fails to do so, value remains untouched.
    /// </summary>
    public bool TryParse(string input, ref TNumber number)
    {
        try
        {
            //If input is empty 
            if (string.IsNullOrWhiteSpace(input))
            {
                //Nullable: Null 
                //Non-Nullable: 0  
                return false;
            }

            //Parse to decimal? (cause it can fit all numeric types) 
            var tempNumber = ToDecimal(input);

            //If failed, throw 
            if (tempNumber is null)
                throw new ArgumentNullException($"Failed to parse input=`{input}` to {typeof(TNumber).GetNullableBaseType().FullName}.");

            //Format to string... 
            input = ToString(tempNumber);

            //...and parse back to decimal? to keep formatting or rounding 
            tempNumber = ToDecimal(input);

            //Finally convert to required type 
            number = tempNumber.ChangeType<TNumber>(_cultureInfo);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Parse value to <see cref="decimal"/>.
    /// Handles any type and format.
    /// </summary>
    private decimal? ToDecimal(string input)
    {
        var isValuePercentage = input.Contains('%');

        //Remove all whitespace 
        input = Regex.Replace(input, @"\s+", string.Empty);

        //Remove percentage symbol 
        if (isValuePercentage)
            input = input.Replace(_cultureInfo.NumberFormat.PercentSymbol, string.Empty);

        if (!decimal.TryParse(input, NumberStyles.Any, _cultureInfo, out var result))
            return default;

        //Divide by 100, since `ToString("P")` format specifier multiplies by 100 
        return _isFormatPercentage || isValuePercentage ? result / 100 : result;
    }

    /// <inheritdoc cref="ToString(decimal?)"/>
    public string ToString(TNumber number)
        => ToString(number.ChangeType<decimal?>(_cultureInfo));

    /// <summary>
    /// Conver number to <see cref="string"/>.
    /// </summary>
    private string ToString(decimal? number)
        => number?.ToString(_format, _cultureInfo);
}
