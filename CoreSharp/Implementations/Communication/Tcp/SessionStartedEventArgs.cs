using System;

namespace CoreSharp.Implementations.Communication.Tcp
{
    public class SessionStartedEventArgs : SessionEventArgs
    {
        //Constructors 
        public SessionStartedEventArgs(TcpSession session) : base(session?.ServerEndPoint, session?.SessionEndPoint)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }

        //Properties 
        public TcpSession Session { get; }
    }
}