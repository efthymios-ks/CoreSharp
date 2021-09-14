using System;

namespace CoreSharp.Implementations.Communication
{
    public class DataTransferredEventArgs : EventArgs
    {
        //Constructors 
        public DataTransferredEventArgs(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        //Properties 
        public byte[] Data { get; } = Array.Empty<byte>();
    }
}