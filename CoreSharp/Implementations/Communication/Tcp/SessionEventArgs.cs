using System;
using System.Net;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class SessionEventArgs : EventArgs
    {
        //Constructor
        public SessionEventArgs(IPEndPoint serverEndPoint, IPEndPoint remoteEndPoint)
        {
            ServerEndPoint = serverEndPoint;
            RemoteEndPoint = remoteEndPoint;
        }

        //Properties 
        public IPEndPoint ServerEndPoint { get; }
        public IPEndPoint RemoteEndPoint { get; }
    }
}