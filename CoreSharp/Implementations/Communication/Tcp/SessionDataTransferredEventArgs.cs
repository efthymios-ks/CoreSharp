using System;
using System.Net;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class SessionDataTransferredEventArgs : DataTransferredEventArgs
    {
        //Constructors 
        public SessionDataTransferredEventArgs(TcpSession session, byte[] data) : base(data)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        //Properties 
        public TcpSession Session { get; }
        public IPEndPoint ServerEndPoint => Session?.ServerEndPoint;
        public IPEndPoint SessionEndPoint => Session?.SessionEndPoint;
    }
}