using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters;

internal sealed class EnumJsonConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : Enum
{
    // Fields
    private static readonly TypeCode EnumTypeCode = Type.GetTypeCode(typeof(TEnum));

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
    {
        switch (EnumTypeCode)
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
                writer.WriteNumberValue(Convert.ToInt64(value));
                break;
            case TypeCode.UInt64:
                writer.WriteNumberValue(Convert.ToUInt64(value));
                break;
            default:
                throw new JsonException($"`{value}` is not a valid `{typeof(TEnum)}` enum.");
        }
    }

    private static object ReadValueAsObject(ref Utf8JsonReader reader)
        => reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => EnumTypeCode switch
            {
                TypeCode.Byte => reader.GetByte(),
                TypeCode.SByte => reader.GetSByte(),
                TypeCode.Int16 => reader.GetInt16(),
                TypeCode.UInt16 => reader.GetUInt16(),
                TypeCode.Int32 => reader.GetInt32(),
                TypeCode.UInt32 => reader.GetUInt32(),
                TypeCode.Int64 => reader.GetInt64(),
                TypeCode.UInt64 => reader.GetUInt64(),
                _ => null
            },
            _ => null
        };

    private static object ParseFromObject(Type enumType, object value, JsonSerializerOptions jsonSerializerOptions)
        => value switch
        {
            string valueAsString => ParseFromString(enumType, valueAsString, jsonSerializerOptions.PropertyNameCaseInsensitive),
            byte or sbyte or
            short or ushort or
            int or uint or
            long or ulong => ParseFromNumber(enumType, value),
            _ => null
        };

    private static object ParseFromNumber(Type enumType, object value)
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
