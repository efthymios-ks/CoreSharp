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
    public sealed class TcpSession : Disposable
    {
        //Fields 
        private readonly TcpServer _server;
        private Socket _socket;
        private bool _isConnected;
        private bool _isTerminated;

        //Properties 
        private string DebuggerDisplay => $"Server='{ServerEndPoint}', Session='{SessionEndPoint}'";
        public IPEndPoint ServerEndPoint => _socket?.LocalEndPoint as IPEndPoint;
        public IPEndPoint SessionEndPoint { get; private set; }
        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                if (value == _isConnected)
                    return;

                _isConnected = value;

                if (_isConnected)
                {
                    _isTerminated = false;
                    BeginReceive();
                }

                OnConnectionStatusChanged(_isConnected);
            }
        }

        //Constructors 
        public TcpSession(TcpServer server)
        {
            _server = server ?? throw new ArgumentNullException(nameof(server));
        }

        ~TcpSession()
        {
            Dispose();
        }

        //Events 
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<DataTransferredEventArgs> DataSent;
        public event EventHandler<DataTransferredEventArgs> DataReceived;
        public event EventHandler<SocketErrorEventArgs> ErrorOccured;

        //Methods 
        public void Connect(Socket socket)
        {
            _ = socket ?? throw new ArgumentNullException(nameof(socket));

            socket.NoDelay = _server.NoDelay;
            _socket = socket;
            SessionEndPoint = _socket?.RemoteEndPoint as IPEndPoint;

            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;
            IsConnected = false;

            Terminate();
            _server?.UnregisterSession(this);
        }

        public int Send(string text)
        {
            return Send(text, Encoding.UTF8);
        }

        public int Send(string text, Encoding encoding)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            return Send(data);
        }

        public int Send(IEnumerable<byte> data)
        {
            return Send(data?.ToArray());
        }

        public int Send(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            if (!IsConnected)
                throw new InvalidOperationException("Cannot send data while disconnected.");

            var count = _socket.Send(data, 0, data.Length, SocketFlags.None, out var error);
            if (count > 0)
                OnDataSent(data.Take(count));

            if (error != SocketError.Success)
            {
                OnError(error);
                Disconnect();
            }

            return count;
        }

        public void BeginSend(string text)
        {
            BeginSend(text, Encoding.UTF8);
        }

        public void BeginSend(string text, Encoding encoding)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var buffer = encoding.GetBytes(text);
            BeginSend(buffer);
        }

        public void BeginSend(IEnumerable<byte> data)
        {
            BeginSend(data?.ToArray());
        }

        public void BeginSend(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            if (!IsConnected)
                throw new InvalidOperationException("Cannot send data while disconnected");

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.Completed += AsyncOperationCompleted;
            if (!_socket.SendAsync(args))
                HandleSend(args);
        }

        private void BeginReceive()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Cannot receive data while disconnected");

            var buffer = new byte[_socket.ReceiveBufferSize];
            var args = new SocketAsyncEventArgs();
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += AsyncOperationCompleted;
            if (!_socket.ReceiveAsync(args))
                HandleReceive(args);
        }

        private void HandleSend(SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            try
            {
                if (args.SocketError == SocketError.Success)
                {
                    OnDataSent(args.Buffer);
                }
                else
                {
                    OnError(args.SocketError);
                    Disconnect();
                }
            }
            finally
            {
                args.Completed -= AsyncOperationCompleted;
                args.Dispose();
            }
        }

        private void HandleReceive(SocketAsyncEventArgs args)
        {
            try
            {
                //If Socket has disconnected, then BytesTransfered = 0 
                if (args.BytesTransferred <= 0)
                    args.SocketError = SocketError.NotConnected;

                if (args.SocketError == SocketError.Success)
                {
                    var bytesReceived = new byte[args.BytesTransferred];
                    Array.Copy(args.Buffer, bytesReceived, args.BytesTransferred);
                    OnDataReceived(bytesReceived);
                    BeginReceive();
                }
                else
                {
                    OnError(args.SocketError);
                    Disconnect();
                }
            }
            finally
            {
                args.Completed -= AsyncOperationCompleted;
                args.Dispose();
            }
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

        private void OnConnectionStatusChanged(bool isConnected)
        {
            var args = new ConnectionStatusChangedEventArgs(isConnected);
            ConnectionStatusChanged?.Invoke(this, args);
        }

        private void OnDataSent(IEnumerable<byte> data)
        {
            OnDataSent(data?.ToArray());
        }

        private void OnDataSent(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var args = new DataTransferredEventArgs(data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var args = new DataTransferredEventArgs(data);
            DataReceived?.Invoke(this, args);
        }

        private void OnError(SocketError error)
        {
            //Skip disconnect error
            if (error.IsIn(
                SocketError.ConnectionAborted,
                SocketError.ConnectionRefused,
                SocketError.ConnectionReset,
                SocketError.NotConnected,
                SocketError.OperationAborted,
                SocketError.Shutdown))
            {
                return;
            }

            var args = new SocketErrorEventArgs(error);
            ErrorOccured?.Invoke(this, args);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            //var socket = sender as Socket;
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    HandleSend(args);
                    break;
                case SocketAsyncOperation.Receive:
                    HandleReceive(args);
                    break;
                default:
                    OnError(args.SocketError);
                    break;
            }
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            Disconnect();
            Terminate();
            IsConnected = false;
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}