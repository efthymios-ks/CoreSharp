using CoreSharp.Models.Newtonsoft.Resolvers;
using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft.Settings
{
    public class PrimitiveOnlyJsonSettings : JsonSerializerSettings
    {
        //Fields
        private static PrimitiveOnlyJsonSettings _instance;

        //Constructors
        public PrimitiveOnlyJsonSettings()
        {
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = PrimitiveOnlyResolver.Instance;
        }

        //Properties
        public static PrimitiveOnlyJsonSettings Instance => _instance ??= new PrimitiveOnlyJsonSettings();
    }
}
