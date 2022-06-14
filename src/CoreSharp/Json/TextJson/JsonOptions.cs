using CoreSharp.Json.TextJson.JsonConverters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreSharp.Json.TextJson
{
    public static class JsonOptions
    {
        //Fields 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static JsonSerializerOptions _default;

        //Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static IEnumerable<JsonConverter> DefaultJsonConverters
            => new[]
            {
                new TimeSpanJsonConverter()
            };

        public static JsonSerializerOptions Default
            => _default ??= CreateDefault();

        //Methods
        private static JsonSerializerOptions CreateDefault()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                IncludeFields = false,
                WriteIndented = true,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            foreach (var jsonConverter in DefaultJsonConverters)
                options.Converters.Add(jsonConverter);

            return options;
        }
    }
}
