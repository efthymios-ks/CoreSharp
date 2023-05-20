using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.Comparers;

/// <summary>
/// Compare <see cref="DirectoryInfo"/> using their file count.
/// </summary>
public class FileCountComparer : Comparer<DirectoryInfo>
{
    public override int Compare(DirectoryInfo x, DirectoryInfo y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        var currentLength = GetFileCount(x);
        var previousLength = GetFileCount(y);

        if (currentLength > previousLength)
        {
            return 1;
        }
        else if (currentLength < previousLength)
        {
            return -1;
        }

        return 0;
    }

    private static long GetFileCount(DirectoryInfo directoryInfo)
        => directoryInfo.GetFiles().LongLength;
}
