using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson.JsonConverters
{
    internal static class JsonConvertersSelect
    {
        //Properties
        public static IEnumerable<JsonConverter> All
        {
            get
            {
                yield return new TimeSpanJsonConverter();
            }
        }
    }
}