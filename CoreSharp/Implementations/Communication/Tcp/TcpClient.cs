using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using CoreSharp.Extensions;

namespace CoreSharp.Implementations.Communication.Tcp
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TcpClient : Disposable
    {
        //Fields 
        private Socket socket;
        private bool isConnected;
        private bool isConnecting;
        private bool isTerminated;
        private TimeSpan timeout = TimeSpan.FromSeconds(10);
        private readonly object timerLock = new object();
        private readonly Timer timeoutTimer = new Timer() { AutoReset = true };

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"Local='{LocalEndPoint}', Remote='{RemoteEndPoint}'";
        public IPEndPoint LocalEndPoint => socket?.LocalEndPoint as IPEndPoint;
        public IPEndPoint RemoteEndPoint { get; private set; }
        public int BufferSize { get; set; } = 8 * 1024;
        public TimeSpan Timeout
        {
            get { return timeout; }
            set
            {
                if (value.IsIn(TimeSpan.Zero, TimeSpan.MinValue, TimeSpan.MaxValue))
                    throw new ArgumentOutOfRangeException(nameof(Timeout), $"{nameof(Timeout)} has to has a valid, discrete value.");

                if (value == timeout)
                    return;

                timeout = value;

                bool enabledState = timeoutTimer.Enabled;
                timeoutTimer.Enabled = false;
                timeoutTimer.Interval = timeout.TotalMilliseconds;
                timeoutTimer.Enabled = enabledState;
            }
        }
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

                timeoutTimer.Enabled = isConnected;
                OnConnectionStatusChanged(isConnected);
            }
        }

        //Constructors 
        public TcpClient(int port) : this(IPAddress.Loopback, port)
        {

        }

        public TcpClient(string ip, int port) : this(IPAddress.Parse(ip), port)
        {

        }

        public TcpClient(IPAddress ip, int port)
        {
            ip = ip ?? throw new ArgumentNullException(nameof(ip));

            RemoteEndPoint = new IPEndPoint(ip, port);
            timeoutTimer.Interval = Timeout.TotalMilliseconds;
            timeoutTimer.Elapsed += TimeoutTimer_Elapsed;
        }

        ~TcpClient()
        {
            Dispose();
        }

        //Events 
        public EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public EventHandler<DataTransferedEventArgs> DataSent;
        public EventHandler<DataTransferedEventArgs> DataReceived;
        public EventHandler<ErrorOccuredEventArgs> ErrorOccured;

        //Methods 
        public void Connect()
        {
            if (isConnecting)
                return;
            else if (IsConnected)
                return;

            try
            {
                //Create new socket 
                socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = (int)Timeout.TotalMilliseconds,
                    ReceiveBufferSize = BufferSize,
                    SendTimeout = (int)Timeout.TotalMilliseconds,
                    SendBufferSize = BufferSize
                };
                socket.Connect(RemoteEndPoint);
                IsConnected = true;
            }
            catch (SocketException ex)
            {
                OnError(ex.SocketErrorCode);
                throw;
            }
            finally
            {
                Terminate();
                IsConnected = false;
            }
        }

        public void BeginConnecting()
        {
            //Check connection status 
            if (isConnecting)
                return;
            else if (IsConnected)
                return;

            //Status change 
            isConnecting = true;

            //Create new socket 
            socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = (int)Timeout.TotalMilliseconds,
                ReceiveBufferSize = BufferSize,
                SendTimeout = (int)Timeout.TotalMilliseconds,
                SendBufferSize = BufferSize
            };

            //Async connect 
            var args = new SocketAsyncEventArgs
            {
                RemoteEndPoint = RemoteEndPoint
            };
            args.Completed += AsyncOperationCompleted;

            if (!socket.ConnectAsync(args))
                HandleConnect(args);
        }

        public void Disconnect()
        {
            if (!(IsConnected || isConnecting))
                return;
            IsConnected = false;

            //Cancel connecting operation
            if (isConnecting)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                Socket.CancelConnectAsync(args);
            }

            Terminate();
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

            if (isConnecting || !IsConnected)
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

            if (isConnecting || !IsConnected)
                throw new InvalidOperationException($"Cannot send data while disconnected.");

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.Completed += AsyncOperationCompleted;
            if (!socket.SendAsync(args))
                HandleSend(args);
        }

        private void BeginReceiving()
        {
            if (isConnecting || !IsConnected)
                throw new InvalidOperationException($"Cannot receive data while disconnected.");

            var buffer = new byte[socket.ReceiveBufferSize];
            var args = new SocketAsyncEventArgs
            {
                RemoteEndPoint = RemoteEndPoint
            };
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += AsyncOperationCompleted;
            if (!socket.ReceiveAsync(args))
                HandleReceive(args);
        }

        private void HandleConnect(SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            try
            {
                if (args.SocketError == SocketError.Success)
                    IsConnected = true;
                else
                    OnError(args.SocketError);
                Disconnect();
            }
            finally
            {
                isConnecting = false;
                args.Completed -= AsyncOperationCompleted;
                args.Dispose();
            }
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
            args = args ?? throw new ArgumentNullException(nameof(args));

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

        private void OnDataSent(byte[] data)
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

            var args = new ErrorOccuredEventArgs(error);
            ErrorOccured?.Invoke(this, args);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args = args ?? throw new ArgumentNullException(nameof(args));

            //var socket = sender as Socket;
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    HandleConnect(args);
                    break;
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

        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs args)
        {
            lock (timerLock)
            {
                var timer = sender as System.Timers.Timer;
                timer.Stop();

                IsConnected = socket.IsConnected((int)Timeout.TotalMilliseconds);
                if (!IsConnected)
                    Terminate();

                timer.Start();
            }
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            try
            {
                lock (timerLock)
                {
                    timeoutTimer.Stop();
                    timeoutTimer.Dispose();
                }
            }
            catch { }
            Disconnect();
            Terminate();
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}