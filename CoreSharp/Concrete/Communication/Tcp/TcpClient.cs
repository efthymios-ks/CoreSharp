using CoreSharp.Abstracts;
using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace CoreSharp.Concrete.Communication.Tcp
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class TcpClient : Disposable
    {
        //Fields 
        private Socket _socket;
        private bool _isConnected;
        private bool _isConnecting;
        private bool _isTerminated;
        private TimeSpan _timeout = TimeSpan.FromSeconds(10);
        private readonly object _timerLock = new();
        private readonly Timer _timeoutTimer = new() { AutoReset = true };

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"Local='{LocalEndPoint}', Remote='{RemoteEndPoint}'";
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; }
        public int BufferSize { get; set; } = 8 * 1024;
        public TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                if (value.IsIn(TimeSpan.Zero, TimeSpan.MinValue, TimeSpan.MaxValue))
                    throw new ArgumentOutOfRangeException(nameof(Timeout), $"{nameof(Timeout)} has to has a valid, discrete value.");

                if (value == _timeout)
                    return;

                _timeout = value;

                var enabledState = _timeoutTimer.Enabled;
                _timeoutTimer.Enabled = false;
                _timeoutTimer.Interval = _timeout.TotalMilliseconds;
                _timeoutTimer.Enabled = enabledState;
            }
        }
        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (value == _isConnected)
                    return;

                _isConnected = value;

                if (_isConnected)
                {
                    _isTerminated = false;
                    LocalEndPoint = _socket?.LocalEndPoint as IPEndPoint;
                    BeginReceive();
                }

                _timeoutTimer.Enabled = _isConnected;
                OnConnectionStatusChanged(_isConnected);
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
            _timeoutTimer.Interval = Timeout.TotalMilliseconds;
            _timeoutTimer.Elapsed += TimeoutTimer_Elapsed;
        }

        ~TcpClient()
        {
            Dispose();
        }

        //Events 
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<DataTransferredEventArgs> DataSent;
        public event EventHandler<DataTransferredEventArgs> DataReceived;
        public event EventHandler<SocketErrorEventArgs> ErrorOccured;

        //Methods 
        public void Connect()
        {
            if (_isConnecting || IsConnected)
                return;

            try
            {
                //Create new socket 
                _socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveTimeout = (int)Timeout.TotalMilliseconds,
                    ReceiveBufferSize = BufferSize,
                    SendTimeout = (int)Timeout.TotalMilliseconds,
                    SendBufferSize = BufferSize
                };
                _socket.Connect(RemoteEndPoint);
                IsConnected = true;
            }
            catch (SocketException ex)
            {
                IsConnected = false;
                Terminate();
                OnError(ex.SocketErrorCode);
            }
        }

        public void BeginConnecting()
        {
            //Check connection status 
            if (_isConnecting)
                return;
            else if (IsConnected)
                return;

            //Status change 
            _isConnecting = true;

            //Create new socket 
            _socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = (int)Timeout.TotalMilliseconds,
                ReceiveBufferSize = BufferSize,
                SendTimeout = (int)Timeout.TotalMilliseconds,
                SendBufferSize = BufferSize
            };

            //Async connect 
            var args = new SocketAsyncEventArgs()
            {
                RemoteEndPoint = RemoteEndPoint
            };
            args.Completed += AsyncOperationCompleted;

            if (!_socket.ConnectAsync(args))
                HandleConnect(args);
        }

        public void Disconnect()
        {
            if (!(IsConnected || _isConnecting))
                return;
            IsConnected = false;

            //Cancel connecting operation
            if (_isConnecting)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                Socket.CancelConnectAsync(args);
            }

            Terminate();
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

            if (_isConnecting || !IsConnected)
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

        public void BeginSend(string data)
        {
            BeginSend(data, Encoding.UTF8);
        }

        public void BeginSend(string data, Encoding encoding)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var buffer = encoding.GetBytes(data);
            BeginSend(buffer);
        }

        public void BeginSend(IEnumerable<byte> data)
        {
            BeginSend(data?.ToArray());
        }

        public void BeginSend(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            if (_isConnecting || !IsConnected)
                throw new InvalidOperationException("Cannot send data while disconnected.");

            var args = new SocketAsyncEventArgs();
            args.SetBuffer(data, 0, data.Length);
            args.Completed += AsyncOperationCompleted;
            if (!_socket.SendAsync(args))
                HandleSend(args);
        }

        private void BeginReceive()
        {
            if (_isConnecting || !IsConnected)
                throw new InvalidOperationException("Cannot receive data while disconnected.");

            var buffer = new byte[_socket.ReceiveBufferSize];
            var args = new SocketAsyncEventArgs()
            {
                RemoteEndPoint = RemoteEndPoint
            };
            args.SetBuffer(buffer, 0, buffer.Length);
            args.Completed += AsyncOperationCompleted;
            if (!_socket.ReceiveAsync(args))
                HandleReceive(args);
        }

        private void HandleConnect(SocketAsyncEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

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
                _isConnecting = false;
                args.Completed -= AsyncOperationCompleted;
                args.Dispose();
            }
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
            _ = args ?? throw new ArgumentNullException(nameof(args));

            try
            {
                //If Socket has disconnected, then BytesTransfered = 0 
                if (args.BytesTransferred <= 0)
                    args.SocketError = SocketError.NotConnected;

                if (args.SocketError == SocketError.Success)
                {
                    var bytesReceived = new byte[args.BytesTransferred];
                    Array.Copy(args.Buffer!, bytesReceived, args.BytesTransferred);
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

        private void OnDataSent(byte[] data)
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
            lock (_timerLock)
            {
                var timer = sender as Timer;
                timer?.Stop();

                IsConnected = _socket.IsConnected((int)Timeout.TotalMilliseconds);
                if (!IsConnected)
                    Terminate();

                timer?.Start();
            }
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            try
            {
                lock (_timerLock)
                {
                    _timeoutTimer.Stop();
                    _timeoutTimer.Dispose();
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
