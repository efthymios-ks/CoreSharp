using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters;

internal sealed class EnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : Enum
{
    // Methods 
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsEnum;

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueToParse = ReadValueAsObject(ref reader);
        var parsedValue = ParseFromObject(typeToConvert, valueToParse, options);

        if (parsedValue is null)
            throw new JsonException($"`{valueToParse}` is not a valid `{typeToConvert}` enum.");

        return (TEnum)parsedValue;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        => writer.WriteNumberValue(Convert.ToInt32(value));

    private static object ReadValueAsObject(ref Utf8JsonReader reader)
        => reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32(),
            JsonTokenType.StartArray => reader.GetString(),
            _ => null
        };

    private static object ParseFromObject(Type enumType, object value, JsonSerializerOptions jsonSerializerOptions)
        => value switch
        {
            int valueAsInt => ParseFromInt(enumType, valueAsInt),
            string valueAsString => ParseFromString(enumType, valueAsString, jsonSerializerOptions.PropertyNameCaseInsensitive),
            _ => null
        };

    private static object ParseFromInt(Type enumType, int value)
    {
        if (!Enum.IsDefined(enumType, value))
            return null;

        return Enum.ToObject(enumType, value);
    }

    private static object ParseFromString(Type enumType, string value, bool ignoreCase)
    {
        if (!Enum.TryParse(enumType, value, ignoreCase, out var enumValue))
            return null;

        return enumValue;
    }
}
