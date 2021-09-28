using CoreSharp.Models.Newtonsoft.Resolvers;
using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft.Settings
{
    public class DefaultJsonSettings : JsonSerializerSettings
    {
        //Fields
        private static DefaultJsonSettings _instance;

        //Constructors
        public DefaultJsonSettings()
        {
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = WritableOnlyPropertiesResolver.Instance;
        }

        //Properties
        public static DefaultJsonSettings Instance => _instance ??= new DefaultJsonSettings();
    }
}
