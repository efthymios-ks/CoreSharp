﻿using System;

namespace CoreSharp.Concrete.Communication
{
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        //Constructors 
        public ConnectionStatusChangedEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }

        //Properties 
        public bool IsConnected { get; }
    }
}