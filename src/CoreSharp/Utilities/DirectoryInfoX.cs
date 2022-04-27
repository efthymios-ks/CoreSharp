using System.IO;
using System.Reflection;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// <see cref="DirectoryInfo"/> utilities.
    /// </summary>
    public static class DirectoryInfoX
    {
        /// <summary>
        /// Get entry's assembly output folder.
        /// </summary>
        public static DirectoryInfo GetOutputFolder()
        {
            var assembly = Assembly.GetEntryAssembly();
            var location = Path.GetDirectoryName(assembly.Location);
            return new DirectoryInfo(location);
        }
    }
}
