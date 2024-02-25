using System.Collections.Generic;
using System.IO;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="FileInfo"/> utilities.
/// </summary>
public static class FileInfoUtils
{
    /// <summary>
    /// Get entry's assembly output folder files.
    /// </summary>
    public static IEnumerable<FileInfo> GetOutputDlls()
        => DirectoryInfoUtils.GetOutputFolder().GetFiles("*.dll");
}
