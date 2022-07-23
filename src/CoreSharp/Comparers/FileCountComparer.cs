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
        _ = x ?? throw new ArgumentNullException(nameof(x));
        _ = y ?? throw new ArgumentNullException(nameof(y));

        var currentLength = x.GetFiles().LongLength;
        var previousLength = y.GetFiles().LongLength;

        if (currentLength > previousLength)
            return 1;
        else if (currentLength < previousLength)
            return -1;
        else
            return 0;
    }
}
