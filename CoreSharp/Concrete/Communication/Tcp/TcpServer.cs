using CoreSharp.Abstracts;
using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CoreSharp.Concrete.Communication.Tcp
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class TcpServer : DisposableBase
    {
        //Fields 
        private Socket _socket;
        private SocketAsyncEventArgs _socketOperationArgs;
        private bool _isListening;
        private bool _isTerminated;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"'{EndPoint}', Sessions={ActiveSessions?.Count}";
        public IPEndPoint EndPoint { get; private set; }
        public int BufferSize { get; set; } = 8 * 1024;
        public IList<TcpSession> ActiveSessions { get; } = new List<TcpSession>();
        public bool IsListening
        {
            get { return _isListening; }
            private set
            {
                if (value == _isListening)
                    return;

                _isListening = value;

                if (!_isListening)
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

            _socketOperationArgs = new SocketAsyncEventArgs();
            _socketOperationArgs.Completed += AsyncOperationComplete;
            _socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = NoDelay,
                ReceiveBufferSize = BufferSize,
                SendBufferSize = BufferSize
            };
            if (_socket.AddressFamily == AddressFamily.InterNetworkV6)
                _socket.DualMode = DualMode;
            _socket.Bind(EndPoint);

            //Refresh EndPoint 
            EndPoint = (IPEndPoint)_socket.LocalEndPoint;

            //Start listening and accepting sockets
            _socket.Listen(Backlog);
            BeginAccept(_socketOperationArgs);
        }

        public void Stop()
        {
            if (!IsListening)
                return;
            IsListening = false;

            Terminate();
            _socketOperationArgs.Completed -= AsyncOperationComplete;
            _socketOperationArgs?.Dispose();
        }

        internal void RegisterSession(Socket socket)
        {
            _ = socket ?? throw new ArgumentNullException(nameof(socket));

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
            _ = session ?? throw new ArgumentNullException(nameof(session));

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
            _ = text ?? throw new ArgumentNullException(nameof(text));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            Multicast(data);
        }

        public void Multicast(IEnumerable<byte> data)
        {
            Multicast(data?.ToArray());
        }

        public void Multicast(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            foreach (var session in ActiveSessions)
                session.BeginSend(data);
        }

        private void BeginAccept(SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            if (!IsListening)
                throw new InvalidOperationException("Cannot accept sessions while not listening.");

            args.AcceptSocket = null;
            if (!_socket.AcceptAsync(args))
                HandleAccept(args);
        }

        private void HandleAccept(SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            if (args.SocketError == SocketError.Success)
                RegisterSession(args.AcceptSocket);
            else
                OnError(args.SocketError);

            BeginAccept(args);
        }

        private void Terminate()
        {
            if (_isTerminated)
                return;
            _isTerminated = true;

            try
            {
                _socket?.Shutdown(SocketShutdown.Both);
            }
            catch { }
            _socket?.Close();
            _socket?.Dispose();
        }

        private void OnSessionStarted(TcpSession session)
        {
            _ = session ?? throw new ArgumentNullException(nameof(session));

            var args = new SessionStartedEventArgs(session);
            SessionStarted?.Invoke(this, args);
        }

        private void OnSessionDropped(IPEndPoint serverEndPoint, IPEndPoint remoteEndPoint)
        {
            _ = serverEndPoint ?? throw new ArgumentNullException(nameof(serverEndPoint));
            _ = remoteEndPoint ?? throw new ArgumentNullException(nameof(remoteEndPoint));

            var args = new SessionEventArgs(serverEndPoint, remoteEndPoint);
            SessionDropped?.Invoke(this, args);
        }

        private void OnDataSent(TcpSession session, byte[] data)
        {
            _ = session ?? throw new ArgumentNullException(nameof(session));
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var args = new SessionDataTransferredEventArgs(session, data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(TcpSession session, byte[] data)
        {
            _ = session ?? throw new ArgumentNullException(nameof(session));
            _ = data ?? throw new ArgumentNullException(nameof(data));

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
            {
                return;
            }

            var args = new SocketErrorEventArgs(error);
            ErrorOccured?.Invoke(this, args);
        }

        private void AsyncOperationComplete(object sender, SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            HandleAccept(args);
        }

        private void Session_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            if (!args.IsConnected)
            {
                var serverEndPoint = session.ServerEndPoint;
                var sessionEndPoint = session.SessionEndPoint;
                UnregisterSession(session);
                OnSessionDropped(serverEndPoint, sessionEndPoint);
            }
        }

        private void Session_DataReceived(object sender, DataTransferredEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataReceived(session, args.Data);
        }

        private void Session_DataSent(object sender, DataTransferredEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var session = sender as TcpSession;
            OnDataSent(session, args.Data);
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            Stop();
            DisconnectAll();
            Terminate();
            _socketOperationArgs.Completed -= AsyncOperationComplete;
            _socketOperationArgs.Dispose();
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}
