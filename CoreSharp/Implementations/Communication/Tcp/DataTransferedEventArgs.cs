using System;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class DataTransferedEventArgs : EventArgs
    {
        //Constructors 
        public DataTransferedEventArgs(byte[] buffer)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        //Properties 
        public byte[] Buffer { get; private set; } = Array.Empty<byte>();
    }
}