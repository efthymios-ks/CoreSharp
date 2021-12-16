using System;
using System.Net;

namespace CoreSharp.Concrete.Communication.Tcp
{
    internal class SessionEventArgs : EventArgs
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
