using System.Reflection;

namespace CoreSharp.Options
{
    public class EmbeddedFileConfiguration
    {
        //Properties
        public Assembly ScanAssembly { get; set; }
        public string Location { get; set; }
        public string Environment { get; set; }
    }
}
