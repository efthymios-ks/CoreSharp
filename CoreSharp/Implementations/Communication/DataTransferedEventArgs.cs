using System;

namespace CoreSharp.Implementations.Communication
{
    public class DataTransferedEventArgs : EventArgs
    {
        //Constructors 
        public DataTransferedEventArgs(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        //Properties 
        public byte[] Data { get; private set; } = Array.Empty<byte>();
    }
}