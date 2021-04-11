using System;
using System.Diagnostics;
using System.IO;

namespace CoreSharp.Extensions
{
    //TODO: Add unit tests for DirectoryExtensions. 
    /// <summary>
    /// Directory extensions. 
    /// </summary>
    public static class DirectoryExtensions
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

        /// <summary>
        /// Deletes all files from directory. 
        /// </summary>
        public static void Clear(this DirectoryInfo input, bool recursive = false)
        {
            Array.ForEach(input.GetFiles(), i => i.Delete());

            if (recursive)
                Array.ForEach(input.GetDirectories(), i => i.Delete(true));
        }
    }
}
