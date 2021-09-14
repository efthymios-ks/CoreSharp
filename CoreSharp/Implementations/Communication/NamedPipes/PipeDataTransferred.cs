using System;
using System.IO.Pipes;

namespace CoreSharp.Implementations.Communication.NamedPipes
{
    public class PipeDataTransferred : DataTransferredEventArgs
    {
        //Constructors 
        public PipeDataTransferred(NamedPipeServerStream pipe, byte[] data) : base(data)
        {
            Pipe = pipe ?? throw new ArgumentNullException(nameof(pipe));
        }

        //Properties
        public NamedPipeServerStream Pipe { get; }
    }
}