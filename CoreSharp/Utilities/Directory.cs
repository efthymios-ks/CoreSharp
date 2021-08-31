using System.Diagnostics;

namespace CoreSharp.Utilities
{
    /// <summary>
    /// Directory utilities. 
    /// </summary>
    public static partial class Directory
    {
        /// <summary>
        /// Open directory with explorer.exe. 
        /// </summary> 
        public static bool Open(string path)
        {
            try
            {
                Process.Start("explorer.exe", path);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
