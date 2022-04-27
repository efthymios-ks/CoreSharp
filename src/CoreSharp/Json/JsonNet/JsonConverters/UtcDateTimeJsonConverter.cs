using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace CoreSharp.Json.JsonNet.JsonConverters
{
    public class UtcDateTimeJsonConverter : DateTimeConverterBase
    {
        //Fields
        private const string DateFormat = "O";

        //Properties
        private static CultureInfo CultureInfo
            => CultureInfo.InvariantCulture;

        //Methods
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not DateTime dateTime)
                throw new JsonSerializationException($"Expected `{typeof(DateTime).FullName}`, but got `{value.GetType().FullName}`.");

            var jsonValue = dateTime.ToUniversalTime().ToString(DateFormat, CultureInfo);
            writer.WriteValue(jsonValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => reader.TokenType switch
            {
                JsonToken.Null => ReadNull(objectType),
                JsonToken.Date => ReadDate(reader),
                JsonToken.String => ReadString(reader),
                _ => NotReadable(reader)
            };

        private static object ReadNull(Type objectType)
        {
            if (objectType == typeof(DateTime?))
                return null;
            else
                throw new JsonSerializationException($"Cannot convert null to `{objectType.FullName}`.");
        }

        private static object ReadDate(JsonReader reader)
            => reader.Value;

        private static object ReadString(JsonReader reader)
        {
            var dateAsText = $"{reader.Value}";
            if (DateTime.TryParseExact(dateAsText, DateFormat, CultureInfo, DateTimeStyles.None, out var result))
                return result;
            else
                return null;
        }

        private static object NotReadable(JsonReader reader)
            => throw new JsonSerializationException($"Expected `{typeof(DateTime).FullName}` or `{typeof(string).FullName}`, but got `{reader.TokenType}`.");
    }
}
