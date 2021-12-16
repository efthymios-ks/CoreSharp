﻿using System;

namespace CoreSharp.Concrete.Communication
{
    internal class DataTransferredEventArgs : EventArgs
    {
        //Constructors 
        public DataTransferredEventArgs(byte[] data)
            => Data = data ?? throw new ArgumentNullException(nameof(data));

        //Properties 
        public byte[] Data { get; } = Array.Empty<byte>();
    }
}
