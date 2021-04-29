﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSharp.Extensions;

namespace CoreSharp.Implementations.Communication.NamedPipes
{
    /// <summary>
    /// Client for NamedPipe communication.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class NamedPipeClient : Disposable
    {
        //Fields 
        private readonly NamedPipeClientStream pipe;
        private bool isConnected;
        private bool isTerminated;

        //Constructor 
        public NamedPipeClient(string serverName, string pipeName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                throw new ArgumentNullException(nameof(serverName));
            else if (string.IsNullOrWhiteSpace(pipeName))
                throw new ArgumentNullException(nameof(pipeName));

            pipeName = pipeName
                .ToLower()
                .Truncate(256)
                .Erase("\\");

            ServerName = serverName;
            PipeName = pipeName;

            pipe = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"Server='{ServerName}', Pipe='{PipeName}'";
        public string ServerName { get; }
        public string PipeName { get; }
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (value == isConnected)
                    return;

                isConnected = value;

                if (isConnected)
                {
                    isTerminated = false;
                    BeginRead();
                }

                OnConnectionStatusChanged(isConnected);
            }
        }
        public int BufferSize { get; set; } = 8 * 1024;

        //Events 
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<DataTransferedEventArgs> DataReceived;
        public event EventHandler<DataTransferedEventArgs> DataSent;

        //Methods
        public void Connect()
        {
            Connect(500);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public void Connect(int millis)
        {
            if (IsConnected)
                return;

            pipe.Connect(millis);
            pipe.ReadMode = PipeTransmissionMode.Message;
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;
            IsConnected = false;

            Terminate();
        }

        public int Send(string data)
        {
            return Send(data, Encoding.UTF8);
        }

        public int Send(string text, Encoding encoding)
        {
            return SendAsync(text, encoding).GetAwaiter().GetResult();
        }

        public int Send(IEnumerable<byte> data)
        {
            return Send(data?.ToArray());
        }

        public int Send(params byte[] data)
        {
            return SendAsync(data).GetAwaiter().GetResult();
        }

        public async Task<int> SendAsync(string text)
        {
            return await SendAsync(text, Encoding.UTF8);
        }

        public async Task<int> SendAsync(string text, Encoding encoding)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));

            var data = encoding.GetBytes(text);
            return await SendAsync(data);
        }

        public async Task<int> SendAsync(IEnumerable<byte> data)
        {
            return await SendAsync(data?.ToArray());
        }

        public async Task<int> SendAsync(params byte[] data)
        {
            data = data ?? throw new ArgumentNullException(nameof(data));
            if (!IsConnected)
                throw new InvalidOperationException($"Cannot send data while disconnected.");
            else if (!pipe.CanWrite)
                throw new NotSupportedException($"Current pipe does not support write operations.");

            await pipe.WriteAsync(data.AsMemory(0, data.Length));
            await pipe.FlushAsync();
            OnDataSent(data);
            return data.Length;
        }

        private void BeginRead()
        {
            if (!IsConnected)
                throw new InvalidOperationException($"Cannot read data while disconnected.");
            else if (!pipe.CanRead)
                throw new NotSupportedException($"Current pipe does not support read operations.");

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
            result = result ?? throw new ArgumentNullException(nameof(result));

            var state = result.AsyncState as dynamic;
            var pipe = state.Pipe as NamedPipeClientStream;
            var buffer = state.Buffer as byte[];

            int bytesRead = pipe.EndRead(result);

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
            if (isTerminated)
                return;
            isTerminated = true;

            try
            {
                await pipe?.FlushAsync();
                pipe?.WaitForPipeDrain();
                pipe?.Close();
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