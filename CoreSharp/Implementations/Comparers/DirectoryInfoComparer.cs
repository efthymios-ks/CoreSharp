﻿using System.Collections.Generic;
using System.IO;

namespace CoreSharp.Implementations.Comparers
{
    /// <summary>
    /// Compare DirectoryInfo using their file count. 
    /// </summary>
    public class DirectoryInfoComparer : IComparer<DirectoryInfo>
    {
        public int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            var xLength = x.GetFiles().LongLength;
            var yLength = y.GetFiles().LongLength;

            if (xLength > yLength)
                return 1;
            else if (xLength < yLength)
                return -1;
            else
                return 0;
        }
    }
}