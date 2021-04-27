using System;
using System.Net;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class SessionDataTransferred : DataTransferedEventArgs
    {
        //Constructors 
        public SessionDataTransferred(TcpSession session, byte[] buffer) : base(buffer)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        //Properties 
        public TcpSession Session { get; }
        public IPEndPoint ServerEndPoint => Session?.ServerEndPoint;
        public IPEndPoint RemoteEndPoint => Session?.RemoteEndPoint;
    }
}