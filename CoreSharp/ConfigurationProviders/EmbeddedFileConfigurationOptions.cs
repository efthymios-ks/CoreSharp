using System.Reflection;

namespace CoreSharp.ConfigurationProviders
{
    public class EmbeddedFileConfigurationOptions
    {
        //Properties
        public Assembly ScanAssembly { get; set; }
        public string Location { get; set; }
        public string Environment { get; set; }
    }
}
