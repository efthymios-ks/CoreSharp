using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft
{
    internal class JsonSerializerDefaultSettings : JsonSerializerSettings
    {
        internal JsonSerializerDefaultSettings()
        {
            NullValueHandling = NullValueHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = new WritableOnlyPropertiesResolver();
        }
    }
}
