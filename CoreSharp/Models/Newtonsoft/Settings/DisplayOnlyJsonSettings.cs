using Newtonsoft.Json;

namespace CoreSharp.Models.Newtonsoft.Settings
{
    public class DisplayOnlyJsonSettings : JsonSerializerSettings
    {
        //Fields
        private static DefaultJsonSettings _instance;

        //Constructors
        public DisplayOnlyJsonSettings()
        {
            Formatting = Formatting.Indented;
            NullValueHandling = NullValueHandling.Include;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        //Properties
        public static DefaultJsonSettings Instance => _instance ??= new DefaultJsonSettings();
    }
}
