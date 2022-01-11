using CoreSharp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace CoreSharp.EntityFramework.Models.Abstracts
{
    public class UtcDateTimeConverter : DateTimeConverterBase
    {
        //Methods 
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not DateTime dateTime)
                throw new JsonSerializationException($"Expected `{typeof(DateTime).FullName}`, but got `{value.GetType().FullName}`.");

            var jsonValue = dateTime.ToStringSortableUtc();
            writer.WriteValue(jsonValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //Null
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType == typeof(DateTime?))
                    return null;
                else
                    throw new JsonSerializationException($"Cannot convert null to `{objectType.FullName}`.");
            }

            //Date
            if (reader.TokenType == JsonToken.Date)
            {
                return reader.Value;
            }

            //String 
            if (reader.TokenType == JsonToken.String)
            {
                var dateAsText = $"{reader.Value}";
                return dateAsText.ToDateTimeSortableUtc();
            }

            //Else throw 
            else
            {
                throw new JsonSerializationException($"Expected `{typeof(DateTime).FullName}` or `{typeof(string).FullName}`, but got `{reader.TokenType}`.");
            }
        }
    }
}
