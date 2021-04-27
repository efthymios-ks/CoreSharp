using System;
using System.Net.Sockets;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class ErrorOccuredEventArgs : EventArgs
    {
        //Constructors 
        public ErrorOccuredEventArgs(SocketError error)
        {
            Error = error;
        }

        //Properties 
        public SocketError Error { get; private set; } = SocketError.SocketError;
    }
}