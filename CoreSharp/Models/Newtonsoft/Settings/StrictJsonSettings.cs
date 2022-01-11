using CoreSharp.Models.Newtonsoft.Resolvers;
using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft.Settings
{
    public class StrictJsonSettings : JsonSerializerSettings
    {
        //Fields
        private static StrictJsonSettings _instance;

        //Constructors
        public StrictJsonSettings()
        {
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Include;
            MissingMemberHandling = MissingMemberHandling.Error;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = WritableOnlyPropertiesResolver.Instance;
        }

        //Properties
        public static StrictJsonSettings Instance
            => _instance ??= new StrictJsonSettings();
    }
}
