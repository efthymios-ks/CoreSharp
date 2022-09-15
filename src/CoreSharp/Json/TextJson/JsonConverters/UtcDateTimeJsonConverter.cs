using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters;

public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
{
    // Fields
    private const string DateFormat = "O";

    // Properties
    private static CultureInfo CultureInfo
        => CultureInfo.InvariantCulture;

    // Methods
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var jsonValue = value.ToUniversalTime().ToString(DateFormat, CultureInfo);
        writer.WriteStringValue(jsonValue);
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateAsString = reader.GetString();
        var date = DateTime.ParseExact(dateAsString, DateFormat, CultureInfo, DateTimeStyles.None);

        // Avoid `DateTime.SpecifyKind` which changes values.
        // Use `TimeZoneInfo.ConvertTimeToUtc` which changes just the `DateTime.Kind`.
        return TimeZoneInfo.ConvertTimeToUtc(date);
    }
}
