using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft
{
    internal class JsonSerializerDefaultSettings : JsonSerializerSettings
    {
        internal JsonSerializerDefaultSettings()
        {
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = new WritableOnlyPropertiesResolver();
        }
    }
}
