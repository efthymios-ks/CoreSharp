using System;
using System.Net;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class SessionEventArgs : EventArgs
    {
        //Constructor
        public SessionEventArgs(IPEndPoint serverEndPoint, IPEndPoint sessionEndPoint)
        {
            ServerEndPoint = serverEndPoint;
            SessionEndPoint = sessionEndPoint;
        }

        //Properties 
        public IPEndPoint ServerEndPoint { get; }
        public IPEndPoint SessionEndPoint { get; }
    }
}