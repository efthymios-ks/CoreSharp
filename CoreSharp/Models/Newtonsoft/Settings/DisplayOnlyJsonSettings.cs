using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft.Settings
{
    public class DisplayOnlyJsonSettings : JsonSerializerSettings
    {
        //Fields
        private static DisplayOnlyJsonSettings _instance;

        //Constructors
        public DisplayOnlyJsonSettings()
        {
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Include;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        //Properties
        public static DisplayOnlyJsonSettings Instance
            => _instance ??= new DisplayOnlyJsonSettings();
    }
}
