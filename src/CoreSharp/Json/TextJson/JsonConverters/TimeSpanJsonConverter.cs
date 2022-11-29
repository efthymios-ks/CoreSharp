using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters;

public sealed class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
    // Fields 
    private const string Format = "c";
    private const string FormatTemplate = "[d'.']hh':'mm':'ss['.'fffffff]";

    // Properties 
    private static CultureInfo CultureInfo
        => CultureInfo.InvariantCulture;

    // Methods 
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeSpanAsString = reader.GetString();

        if (TimeSpan.TryParseExact(timeSpanAsString, Format, CultureInfo, out var timeSpan))
            return timeSpan;
        else
            throw new FormatException($"{Format} ({FormatTemplate}) is required.");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        var timeSpanAsString = value.ToString(Format, CultureInfo);
        writer.WriteStringValue(timeSpanAsString);
    }
}
