using System;
using System.IO;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="FileInfo"/> extensions.
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// Change given file extension.
    /// </summary>
    public static FileInfo ChangeExtension(this FileInfo file, string extension)
    {
        ArgumentNullException.ThrowIfNull(file);

        var newFileName = Path.ChangeExtension(file.FullName, extension);
        return new FileInfo(newFileName);
    }

    /// <summary>
    /// Rename given file.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="name">New file name. May include or not a new extension.</param>
    /// <param name="overwrite"></param>
    public static FileInfo Rename(this FileInfo file, string name, bool overwrite = false)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentException.ThrowIfNullOrEmpty(name);

        var oldParent = file.DirectoryName;
        var oldExtension = file.Extension.Trim('.');

        var newName = Path.GetFileNameWithoutExtension(name);
        var newExtension = Path.GetExtension(name).Trim('.');

        var finalExtension = string.IsNullOrWhiteSpace(newExtension) ? oldExtension : newExtension;
        var finalName = $"{Path.Combine(oldParent!, newName)}.{finalExtension}";

        file.MoveTo(finalName, overwrite);

        return new FileInfo(finalName);
    }
}
