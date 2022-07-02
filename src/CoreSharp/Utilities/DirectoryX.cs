using System.Diagnostics;
using System.IO;

namespace CoreSharp.Utilities;

/// <summary>
/// <see cref="Directory"/> utilities.
/// </summary>
public static class DirectoryX
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
