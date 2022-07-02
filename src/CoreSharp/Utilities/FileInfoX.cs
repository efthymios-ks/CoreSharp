using System.Collections.Generic;
using System.IO;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="FileInfo"/> utilities.
/// </summary>
public static class FileInfoX
{
    /// <summary>
    /// Get entry's assembly output folder files.
    /// </summary>
    public static IEnumerable<FileInfo> GetOutputDlls()
        => DirectoryInfoX.GetOutputFolder().GetFiles("*.dll");
}
