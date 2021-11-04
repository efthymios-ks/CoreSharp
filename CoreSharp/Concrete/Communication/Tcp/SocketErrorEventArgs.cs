﻿using System;
using System.Net.Sockets;

namespace CoreSharp.Concrete.Communication.Tcp
{
    public class SocketErrorEventArgs : EventArgs
    {
        //Constructors 
        public SocketErrorEventArgs(SocketError error)
        {
            Error = error;
        }

        //Properties 
        public SocketError Error { get; }
    }
}