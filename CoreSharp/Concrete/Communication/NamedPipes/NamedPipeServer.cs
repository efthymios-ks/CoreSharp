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
    /// Server for NamedPipe communication.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class NamedPipeServer : DisposableBase
    {
        //Fields 
        private bool _isStarted;

        //Constructor 
        public NamedPipeServer(string pipeName, int maxAllowedServerInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            pipeName = pipeName
                .ToLower()
                .Truncate(256)
                .Replace("\\", string.Empty);

            PipeName = pipeName;
            MaxAllowedServerInstances = maxAllowedServerInstances;
        }

        ~NamedPipeServer()
            => Dispose();

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"Pipe='{PipeName}', Sessions={ConnectedPipes?.Count}";
        public string PipeName { get; }
        private IList<NamedPipeServerStream> ConnectedPipes { get; } = new List<NamedPipeServerStream>();
        public int MaxAllowedServerInstances { get; set; }
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                if (value == _isStarted)
                    return;

                _isStarted = value;

                if (_isStarted)
                {
                    IsTerminated = false;
                    WaitForNewConnection();
                }
            }
        }
        private bool IsTerminated { get; set; } = false;
        public int BufferSize { get; set; } = 8 * 1024;

        //Events 
        public EventHandler<PipeConnectedEventArgs> ClientConnected;
        public EventHandler<PipeDataTransferred> DataReceived;
        public EventHandler<PipeDataTransferred> DataSent;

        //Methods 
        public void Start()
        {
            if (IsStarted)
                return;
            IsStarted = true;
        }

        public void Stop()
        {
            if (!IsStarted)
                return;
            IsStarted = false;

            Terminate();
        }

        public void DisconnectAll()
        {
            foreach (var pipe in ConnectedPipes.Mutate())
            {
                ConnectedPipes.Remove(pipe);
                Terminate(pipe);
            }
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private void WaitForNewConnection()
        {
            if (!IsStarted)
                return;

            var pipe = new NamedPipeServerStream(
                PipeName,
                PipeDirection.InOut,
                MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous,
                BufferSize,
                BufferSize);

            var result = pipe.BeginWaitForConnection(WaitForNewConnectionCallback, pipe);
        }

        private void WaitForNewConnectionCallback(IAsyncResult result)
        {
            var pipe = result.AsyncState as NamedPipeServerStream;
            pipe?.EndWaitForConnection(result);

            if (!IsStarted)
                return;

            ConnectedPipes.Add(pipe);
            BeginRead(pipe);
            OnClientConnected(pipe);
            WaitForNewConnection();
        }

        public void Multicast(string text)
            => Multicast(text, Encoding.UTF8);

        public void Multicast(string text, Encoding encoding)
            => MulticastAsync(text, encoding).GetAwaiter().GetResult();

        public void Multicast(IEnumerable<byte> data)
            => Multicast(data?.ToArray());

        public void Multicast(params byte[] data)
            => MulticastAsync(data).GetAwaiter().GetResult();

        public async Task MulticastAsync(string text)
            => await MulticastAsync(text, Encoding.UTF8);

        public async Task MulticastAsync(string text, Encoding encoding)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));
            _ = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            await MulticastAsync(data);
        }

        public async Task MulticastAsync(IEnumerable<byte> data)
            => await MulticastAsync(data?.ToArray());

        public async Task MulticastAsync(params byte[] data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));
            if (!IsStarted)
                throw new InvalidOperationException("Cannot send data while disconnected.");

            foreach (var pipe in ConnectedPipes)
            {
                await pipe.WriteAsync(data.AsMemory(0, data.Length));
                await pipe.FlushAsync();
                OnDataSent(pipe, data);
            }
        }

        private void BeginRead(NamedPipeServerStream pipe)
        {
            _ = pipe ?? throw new ArgumentNullException(nameof(pipe));
            if (!IsStarted)
                throw new InvalidOperationException("Cannot read data while disconnected.");
            else if (!pipe.CanRead)
                throw new NotSupportedException($"Given {nameof(pipe)} does not support read operations.");

            var buffer = new byte[pipe.InBufferSize];
            var state = new
            {
                Pipe = pipe,
                Buffer = buffer
            };
            var result = pipe.BeginRead(buffer, 0, buffer.Length, BeginReadCallback, state);
        }

        private void BeginReadCallback(IAsyncResult result)
        {
            _ = result ?? throw new ArgumentNullException(nameof(result));

            var state = result.AsyncState as dynamic;
            var pipe = state?.Pipe as NamedPipeServerStream;
            var buffer = state?.Buffer as byte[];

            var bytesRead = pipe.EndRead(result);

            if (bytesRead <= 0)
            {
                ConnectedPipes?.Remove(pipe);
                Terminate(pipe);
            }
            else
            {
                Array.Resize(ref buffer, bytesRead);
                OnDataReceived(pipe, buffer);
                BeginRead(pipe);
            }
        }

        private void Terminate()
        {
            if (IsTerminated)
                return;
            IsTerminated = true;

            DisconnectAll();
            try
            {
                //Dummy for ending last BeginWaitForConnection call
                using var dummy = new NamedPipeClientStream(PipeName);
                dummy?.Connect(100);
            }
            catch { }
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private static async void Terminate(NamedPipeServerStream pipe)
        {
            _ = pipe ?? throw new ArgumentNullException(nameof(pipe));

            try
            {
                await pipe?.FlushAsync();
                pipe?.WaitForPipeDrain();
                if (pipe.IsConnected)
                    pipe?.Disconnect();
                pipe?.Close();
                pipe?.Dispose();
            }
            catch { }
        }

        private void OnClientConnected(NamedPipeServerStream clientPipe)
        {
            _ = clientPipe ?? throw new ArgumentNullException(nameof(clientPipe));

            var args = new PipeConnectedEventArgs(clientPipe);
            ClientConnected?.Invoke(this, args);
        }

        private void OnDataSent(NamedPipeServerStream clientPipe, byte[] data)
        {
            _ = clientPipe ?? throw new ArgumentNullException(nameof(clientPipe));
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var args = new PipeDataTransferred(clientPipe, data);
            DataSent?.Invoke(this, args);
        }

        private void OnDataReceived(NamedPipeServerStream clientPipe, byte[] data)
        {
            _ = clientPipe ?? throw new ArgumentNullException(nameof(clientPipe));
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var args = new PipeDataTransferred(clientPipe, data);
            DataReceived?.Invoke(this, args);
        }

        #region Dispose 
        protected override void CleanUpManagedResources()
        {
            Stop();
            DisconnectAll();
            Terminate();
        }

        protected override void CleanUpNativeResources()
        {
        }
        #endregion
    }
}
