using System;
using System.IO;

namespace CoreSharp.Extensions
{
    //TODO: Add unit tests for DirectoryExtensions. 
    /// <summary>
    /// <see cref="DirectoryInfo"/> extensions.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
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
