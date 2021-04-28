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
        private readonly TcpServer server;
        private Socket socket;
        private bool isConnected;
        private bool isTerminated;

        //Properties 
        private string DebuggerDisplay => $"Server='{ServerEndPoint}', Session='{SessionEndPoint}'";
        public IPEndPoint ServerEndPoint => socket?.LocalEndPoint as IPEndPoint;
        public IPEndPoint SessionEndPoint { get; private set; }
        public bool IsConnected
        {
            get { return isConnected; }
            private set
            {
                if (value == isConnected)
                    return;

                isConnected = value;

                if (isConnected)
                {
                    isTerminated = false;
                    BeginReceiving();
                }

                OnConnectionStatusChanged(isConnected);
            }
        }

        //Constructors 
        public TcpSession(TcpServer server)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
        }

        ~TcpSession()
        {
            Dispose();
        }

        //Events 
        public EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public EventHandler<DataTransferedEventArgs> DataSent;
        public EventHandler<DataTransferedEventArgs> DataReceived;
        public EventHandler<SocketErrorEventArgs> ErrorOccured;

        //Methods 
        public void Connect(Socket socket)
        {
            socket = socket ?? throw new ArgumentNullException(nameof(socket));

            socket.NoDelay = server.NoDelay;
            this.socket = socket;
            SessionEndPoint = this.socket?.RemoteEndPoint as IPEndPoint;

            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;
            IsConnected = false;

            Terminate();
            server?.UnregisterSession(this);
        }

        public int Send(string data)
        {
            return Send(data, Encoding.UTF8);
        }

        public int Send(string data, Encoding encoding)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));
            encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var buffer = encoding.GetBytes(data);
            return Send(buffer);
        }

        public int Send(IEnumerable<byte> data)
        {
            return Send(data?.ToArray());
        }

        public int Send(params byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));

            if (!IsConnected)
                throw new InvalidOperationException($"Cannot send data while disconnected.");

            int count = socket.Send(data, 0, data.Length, SocketFlags.None, out var error);
            if (count > 0)
                OnDataSent(data.Take(count));

            if (error != SocketError.Success)
            {
                OnError(error);
                Disconnect();
            }

            return count;
        }

        public void BeginSending(string data)
        {
            BeginSending(data, Encoding.UTF8);
        }

        public void BeginSending(string data, Encoding encoding)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));
            encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var buffer = encoding.GetBytes(data);
            BeginSending(buffer);
        }

        public void BeginSending(IEnumerable<byte> data)
        {
            BeginSending(data?.ToArray());
        }

        public void BeginSending(params byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));

            if (!IsConnected)
                throw new InvalidOperationException($"Cannot send data while disconnected");

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.Completed += AsyncOperationCompleted;
            if (!socket.SendAsync(args))
                HandleSend(args);
        }

        private void BeginReceiving()
        {
            if (!IsConnected)
                throw new InvalidOperationException($"Cannot receive data while disconnected");

            var buffer = new byte[socket.ReceiveBufferSize];
            var args = new SocketAsyncEventArgs();
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += AsyncOperationCompleted;
            if (!socket.ReceiveAsync(args))
                HandleReceive(args);
        }

        private void HandleSend(SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            try
            {
                if (args.SocketError == SocketError.Success)
                    OnDataSent(args.Buffer);
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
                    BeginReceiving();
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
            data = data ?? throw new ArgumentNullException(nameof(data));

            var args = new DataTransferedEventArgs(data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));

            var args = new DataTransferedEventArgs(data);
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
                return;

            var args = new SocketErrorEventArgs(error);
            ErrorOccured?.Invoke(this, args);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

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