﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CoreSharp.Regexs;

/// <summary>
/// Check if input is valid <see cref="int"/>.
/// Either positive or negative.
/// </summary>
public sealed class IntegerRegex
{
    // Fields
    private const string PatternTemplate = @"^(?<Sign>[+-]?)[ ]?(?<Value>(?:\d{1,3}(?:{ThousandSeparator}\d{3})*)|\d+)$";
    private readonly CultureInfo _culture;
    private readonly Match _match;

    // Constructors
    public IntegerRegex(string input)
        : this(input, CultureInfo.CurrentCulture)
    {
    }

    public IntegerRegex(string input, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(culture);

        Input = input;
        _culture = culture;

        Input = input;
        var regex = new Regex(Pattern);
        _match = regex.Match(input.Trim());
    }

    // Properties
    private string Pattern
        => PatternTemplate
            .Replace("{ThousandSeparator}", Regex.Escape(_culture.NumberFormat.NumberGroupSeparator));

    public string Input { get; }

    public bool IsMatch
        => _match.Success;

    public char? Sign
    {
        get
        {
            var signAsString = _match.Groups["Sign"].Value;
            return signAsString.Length switch
            {
                > 0 => signAsString[0],
                _ => null
            };
        }
    }

    public string Value
        => _match.Groups["Value"].Value;
}
