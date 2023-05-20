using System;
using System.IO;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="DirectoryInfo"/> extensions.
/// </summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Deletes all files from directory.
    /// </summary>
    public static void Clear(this DirectoryInfo directory, bool recursive = false)
    {
        Array.ForEach(directory.GetFiles(), i => i.Delete());

        if (recursive)
        {
            Array.ForEach(directory.GetDirectories(), i => i.Delete(true));
        }
    }
}
