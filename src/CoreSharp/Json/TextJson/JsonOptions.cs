using CoreSharp.Json.TextJson.JsonConverters;
using System.Diagnostics;
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
                IgnoreReadOnlyProperties = true
            };
            foreach (var serializer in JsonConvertersSelect.All)
                options.Converters.Add(serializer);

            return options;
        }
    }
}
