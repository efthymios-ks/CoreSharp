using System;
using System.IO;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// FileInfo extensions. 
    /// </summary>
    public static partial class FileInfoExtensions
    {
        /// <summary>
        /// Change extension to file. 
        /// </summary> 
        public static string ChangeExtension(this FileInfo file, string extension)
        {
            file = file ?? throw new ArgumentNullException(nameof(file));

            return Path.ChangeExtension(file.FullName, extension);
        }

        /// <summary>
        /// Rename given file.
        /// </summary>
        /// <param name="name">New file name. May include or not a new extension.</param>
        /// <returns>FileInfo for renamed file.</returns>
        public static FileInfo Rename(this FileInfo file, string name, bool overwrite = false)
        {
            file = file ?? throw new ArgumentNullException(nameof(file));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            string oldParent = file.DirectoryName;
            string oldExtension = file.Extension.Trim('.');

            string newName = Path.GetFileNameWithoutExtension(name);
            string newExtension = Path.GetExtension(name).Trim('.');

            string finalExtension = string.IsNullOrWhiteSpace(newExtension) ? oldExtension : newExtension;
            string finalName = $"{Path.Combine(oldParent, newName)}.{finalExtension}";

            file.MoveTo(finalName, overwrite);

            return new FileInfo(finalName);
        }
    }
}
