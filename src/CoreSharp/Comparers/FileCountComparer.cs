using System;
using System.Collections.Generic;
using System.IO;

namespace CoreSharp.Comparers
{
    /// <summary>
    /// Compare <see cref="DirectoryInfo"/> using their file count.
    /// </summary>
    public class FileCountComparer : Comparer<DirectoryInfo>
    {
        public override int Compare(DirectoryInfo current, DirectoryInfo previous)
        {
            _ = current ?? throw new ArgumentNullException(nameof(current));
            _ = previous ?? throw new ArgumentNullException(nameof(previous));

            var currentLength = current.GetFiles().LongLength;
            var previousLength = previous.GetFiles().LongLength;

            if (currentLength > previousLength)
                return 1;
            else if (currentLength < previousLength)
                return -1;
            else
                return 0;
        }
    }
}
