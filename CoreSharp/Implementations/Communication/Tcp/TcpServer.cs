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
    public sealed class TcpServer : Disposable
    {
        //Fields 
        private Socket socket;
        private SocketAsyncEventArgs socketOperationArgs;
        private bool isListening;
        private bool isTerminated;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"'{EndPoint}', Sessions={ActiveSessions?.Count}";
        public IPEndPoint EndPoint { get; private set; }
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
            EndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        ~TcpServer()
        {
            Dispose();
        }

        //Events  
        public EventHandler<SessionStartedEventArgs> SessionStarted;
        public EventHandler<SessionEventArgs> SessionDropped;
        public EventHandler<SessionDataTransferredEventArgs> DataSent;
        public EventHandler<SessionDataTransferredEventArgs> DataReceived;
        public EventHandler<SocketErrorEventArgs> ErrorOccured;

        //Methods 
        public void Start()
        {
            if (IsListening)
                return;
            IsListening = true;

            socketOperationArgs = new SocketAsyncEventArgs();
            socketOperationArgs.Completed += AsyncOperationComplete;
            socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = NoDelay,
                ReceiveBufferSize = BufferSize,
                SendBufferSize = BufferSize
            };
            if (socket.AddressFamily == AddressFamily.InterNetworkV6)
                socket.DualMode = DualMode;
            socket.Bind(EndPoint);

            //Refresh EndPoint 
            EndPoint = (IPEndPoint)socket.LocalEndPoint;

            //Start listening and accepting sockets
            socket.Listen(Backlog);
            BeginAccept(socketOperationArgs);
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

        public void Multicast(string text)
        {
            Multicast(text, Encoding.UTF8);
        }

        public void Multicast(string text, Encoding encoding)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            Multicast(data);
        }

        public void Multicast(IEnumerable<byte> data)
        {
            Multicast(data?.ToArray());
        }

        public void Multicast(params byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));

            foreach (var session in ActiveSessions)
                session.BeginSend(data);
        }

        private void BeginAccept(SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            if (!IsListening)
                throw new InvalidOperationException($"Cannot accept sessions while not listening.");

            args.AcceptSocket = null;
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

            BeginAccept(args);
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

        private void OnDataSent(TcpSession session, byte[] data)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));
            data = data ?? throw new ArgumentNullException(nameof(data));

            var args = new SessionDataTransferredEventArgs(session, data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(TcpSession session, byte[] data)
        {
            session = session ?? throw new ArgumentNullException(nameof(session));
            data = data ?? throw new ArgumentNullException(nameof(data));

            var args = new SessionDataTransferredEventArgs(session, data);
            DataReceived?.Invoke(this, args);
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

            var args = new SocketErrorEventArgs(error);
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
                var serverEndPoint = session.ServerEndPoint;
                var sessionEndPoint = session.SessionEndPoint;
                UnregisterSession(session);
                OnSessionDropped(serverEndPoint, sessionEndPoint);
            }
        }

        private void Session_DataReceived(object sender, DataTransferedEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataReceived(session, args.Data);
        }

        private void Session_DataSent(object sender, DataTransferedEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataSent(session, args.Data);
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