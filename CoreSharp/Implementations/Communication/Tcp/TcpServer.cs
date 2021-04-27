using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CoreSharp.Extensions;

namespace CoreSharp.Implementations.Communication.Tcp
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TcpServer : Disposable
    {
        //Fields 
        private Socket socket;
        private bool isListening;
        private bool isTerminated;
        private SocketAsyncEventArgs socketOperationArgs;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"'{LocalEndPoint}', Sessions={ActiveSessions.Count}";
        public IPEndPoint LocalEndPoint { get; private set; }
        public int BufferSize { get; set; } = 8 * 1024;
        public IList<TcpSession> ActiveSessions { get; private set; } = new List<TcpSession>();
        public bool IsListening
        {
            get { return isListening; }
            private set
            {
                if (value == isListening)
                    return;

                isListening = value;

                if (!isListening)
                    DisconnectAll();
            }
        }
        /// <summary>
        /// Specify whether the Socket is using the Nagle algorithm.
        /// </summary>
        public bool NoDelay { get; set; }
        /// <summary>
        /// Allow both IPv4 and IPv6. 
        /// </summary>
        public bool DualMode { get; set; }
        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        public int Backlog { get; set; } = 1024;

        //Constructors 
        public TcpServer(int port)
        {
            LocalEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        ~TcpServer()
        {
            Dispose();
        }

        //Events  
        public EventHandler<SessionStartedEventArgs> SessionStarted;
        public EventHandler<SessionEventArgs> SessionDropped;
        public EventHandler<SessionDataTransferred> DataSent;
        public EventHandler<SessionDataTransferred> DataReceive;
        public EventHandler<ErrorOccuredEventArgs> ErrorOccured;

        //Methods 
        public void Start()
        {
            if (IsListening)
                return;
            IsListening = true;

            socketOperationArgs = new SocketAsyncEventArgs();
            socketOperationArgs.Completed += AsyncOperationComplete;
            socket = new Socket(LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = NoDelay,
                ReceiveBufferSize = BufferSize,
                SendBufferSize = BufferSize
            };
            if (socket.AddressFamily == AddressFamily.InterNetworkV6)
                socket.DualMode = DualMode;
            socket.Bind(LocalEndPoint);

            //Refresh EndPoint 
            LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;

            //Start listening and accepting sockets
            socket.Listen(Backlog);
            BeginAccepting(socketOperationArgs);
        }

        public void Stop()
        {
            if (!IsListening)
                return;
            IsListening = false;

            Terminate();
            socketOperationArgs.Completed -= AsyncOperationComplete;
            socketOperationArgs?.Dispose();
        }

        internal void RegisterSession(Socket socket)
        {
            //TODO: Disallow duplicate IP 
            socket = socket ?? throw new ArgumentNullException(nameof(socket));

            var session = new TcpSession(this);
            session.ConnectionStatusChanged += Session_ConnectionStatusChanged;
            session.DataReceived += Session_DataReceived;
            session.DataSent += Session_DataSent;

            session?.Connect(socket);
            ActiveSessions?.Add(session);
            OnSessionStarted(session);
        }

        public void UnregisterSession(TcpSession session)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));

            ActiveSessions?.Remove(session);
            session?.Disconnect();
            session?.Dispose();
        }

        public void DisconnectAll()
        {
            foreach (var session in ActiveSessions.Mutate())
                UnregisterSession(session);
        }

        public void Multicast(string data)
        {
            Multicast(data, Encoding.UTF8);
        }

        public void Multicast(string data, Encoding encoding)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));
            encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var buffer = encoding.GetBytes(data);
            Multicast(buffer);
        }

        public void Multicast(IEnumerable<byte> data)
        {
            Multicast(data?.ToArray());
        }

        public void Multicast(params byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));

            foreach (var client in ActiveSessions)
                client.BeginSending(data);
        }

        private void BeginAccepting(SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            if (!IsListening)
                throw new InvalidOperationException($"Cannot accept sessions while not listening.");

            if (!socket.AcceptAsync(args))
                HandleAccept(args);
        }

        private void HandleAccept(SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            if (args.SocketError == SocketError.Success)
                RegisterSession(args.AcceptSocket);
            else
                OnError(args.SocketError);

            BeginAccepting(args);
        }

        private void Terminate()
        {
            if (isTerminated)
                return;
            isTerminated = true;

            try
            {
                socket?.Shutdown(SocketShutdown.Both);
            }
            catch { }
            socket?.Close();
            socket?.Dispose();
        }

        private void OnSessionStarted(TcpSession session)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));

            var args = new SessionStartedEventArgs(session);
            SessionStarted?.Invoke(this, args);
        }

        private void OnSessionDropped(IPEndPoint serverEndPoint, IPEndPoint remoteEndPoint)
        {
            serverEndPoint = serverEndPoint ?? throw new ArgumentNullException(nameof(serverEndPoint));
            remoteEndPoint = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));

            var args = new SessionEventArgs(serverEndPoint, remoteEndPoint);
            SessionDropped?.Invoke(this, args);
        }

        private void OnDataSent(TcpSession session, byte[] buffer)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));
            buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

            var args = new SessionDataTransferred(session, buffer);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(TcpSession session, byte[] buffer)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));
            buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

            var args = new SessionDataTransferred(session, buffer);
            DataReceive?.Invoke(this, args);
        }

        private void OnError(SocketError error)
        {
            //Skip disconnect error
            if (error.IsIn(
                SocketError.ConnectionAborted,
                SocketError.ConnectionRefused,
                SocketError.ConnectionReset,
                SocketError.OperationAborted,
                SocketError.Shutdown))
                return;

            var args = new ErrorOccuredEventArgs(error);
            ErrorOccured?.Invoke(this, args);
        }

        private void AsyncOperationComplete(object sender, SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            HandleAccept(args);
        }

        private void Session_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            if (!args.IsConnected)
            {
                UnregisterSession(session);
                OnSessionDropped(session.ServerEndPoint, session.RemoteEndPoint);
            }
        }

        private void Session_DataReceived(object sender, DataTransferedEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataReceived(session, args.Buffer);
        }

        private void Session_DataSent(object sender, DataTransferedEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataSent(session, args.Buffer);
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            Stop();
            DisconnectAll();
            Terminate();
            socketOperationArgs.Completed -= AsyncOperationComplete;
            socketOperationArgs.Dispose();
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}