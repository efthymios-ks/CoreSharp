using System;
using System.Net.Sockets;

namespace CoreSharp.Concrete.Communication.Tcp;

internal class SocketErrorEventArgs : EventArgs
{
    //Constructors 
    public SocketErrorEventArgs(SocketError error)
        => Error = error;

    //Properties 
    public SocketError Error { get; }
}
