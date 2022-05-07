using CoreSharp.Abstracts;
using CoreSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharp.Concrete.Communication.NamedPipes
{
    /// <summary>
    /// Client for NamedPipe communication.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NamedPipeClient : DisposableBase
    {
        //Fields 
        private readonly NamedPipeClientStream _pipe;
        private bool _isConnected;
        private bool _isTerminated;

        //Constructor 
        public NamedPipeClient(string serverName, string pipeName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                throw new ArgumentNullException(nameof(serverName));
            else if (string.IsNullOrWhiteSpace(pipeName))
                throw new ArgumentNullException(nameof(pipeName));

            pipeName = pipeName.ToLower()
                               .Truncate(256)
                               .Erase("\\");

            ServerName = serverName;
            PipeName = pipeName;

            _pipe = new(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
            => $"Server='{ServerName}', Pipe='{PipeName}'";
        public string ServerName { get; }
        public string PipeName { get; }
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (value == _isConnected)
                    return;

                _isConnected = value;

                if (_isConnected)
                {
                    _isTerminated = false;
                    BeginRead();
                }

                OnConnectionStatusChanged(_isConnected);
            }
        }
        public int BufferSize { get; set; }
            = 8 * 1024;

        //Events 
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<DataTransferredEventArgs> DataReceived;
        public event EventHandler<DataTransferredEventArgs> DataSent;

        //Methods

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public void Connect(int millis = 500)
        {
            if (IsConnected)
                return;

            _pipe.Connect(millis);
            _pipe.ReadMode = PipeTransmissionMode.Message;
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;
            IsConnected = false;

            Terminate();
        }

        public async Task<int> SendAsync(string text)
            => await SendAsync(text, Encoding.UTF8);

        public async Task<int> SendAsync(string text, Encoding encoding)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            return await SendAsync(data);
        }

        public async Task<int> SendAsync(IEnumerable<byte> data)
            => await SendAsync(data?.ToArray());

        public async Task<int> SendAsync(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            if (!IsConnected)
                throw new InvalidOperationException("Cannot send data while disconnected.");
            else if (!_pipe.CanWrite)
                throw new NotSupportedException("Current pipe does not support write operations.");

            await _pipe.WriteAsync(data.AsMemory(0, data.Length));
            await _pipe.FlushAsync();
            OnDataSent(data);
            return data.Length;
        }

        private void BeginRead()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Cannot read data while disconnected.");
            else if (!_pipe.CanRead)
                throw new NotSupportedException("Current pipe does not support read operations.");

            var buffer = new byte[_pipe.InBufferSize];
            var state = new
            {
                Pipe = _pipe,
                Buffer = buffer
            };
            var result = _pipe.BeginRead(buffer, 0, buffer.Length, BeginReadCallback, state);
        }

        private void BeginReadCallback(IAsyncResult result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            var state = result.AsyncState as dynamic;
            var pipe = state?.Pipe as NamedPipeClientStream;
            var buffer = state?.Buffer as byte[];

            var bytesRead = pipe.EndRead(result);

            if (bytesRead <= 0)
            {
                Terminate();
                IsConnected = false;
            }
            else
            {
                Array.Resize(ref buffer, bytesRead);
                OnDataReceived(buffer);
                BeginRead();
            }
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private async void Terminate()
        {
            if (_isTerminated)
                return;
            _isTerminated = true;

            try
            {
                await _pipe?.FlushAsync();
                _pipe?.WaitForPipeDrain();
                _pipe?.Close();
            }
            catch { }
        }

        private void OnConnectionStatusChanged(bool isConnected)
        {
            var args = new ConnectionStatusChangedEventArgs(isConnected);
            ConnectionStatusChanged?.Invoke(this, args);
        }

        private void OnDataSent(byte[] data)
        {
            var args = new DataTransferredEventArgs(data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(byte[] data)
        {
            var args = new DataTransferredEventArgs(data);
            DataReceived?.Invoke(this, args);
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            Disconnect();
            Terminate();
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}
