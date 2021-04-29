using System;

namespace CoreSharp.Implementations.Communication
{
    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        //Constructors 
        public ConnectionStatusChangedEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }

        //Properties 
        public bool IsConnected { get; private set; } = false;
    }
}