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
        public static FileInfo ChangeExtension(this FileInfo file, string extension)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));

            string newFileName = Path.ChangeExtension(file.FullName, extension);
            return new(newFileName);
        }

        /// <summary>
        /// Rename given file.
        /// </summary>
        /// <param name="name">New file name. May include or not a new extension.</param>
        /// <returns>FileInfo for renamed file.</returns>
        public static FileInfo Rename(this FileInfo file, string name, bool overwrite = false)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            string oldParent = file.DirectoryName;
            string oldExtension = file.Extension.Trim('.');

            string newName = Path.GetFileNameWithoutExtension(name);
            string newExtension = Path.GetExtension(name).Trim('.');

            string finalExtension = string.IsNullOrWhiteSpace(newExtension) ? oldExtension : newExtension;
            string finalName = $"{Path.Combine(oldParent, newName)}.{finalExtension}";

            file.MoveTo(finalName, overwrite);

            return new(finalName);
        }
    }
}
