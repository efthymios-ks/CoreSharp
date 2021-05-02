using System;
using System.IO.Pipes;

namespace CoreSharp.Implementations.Communication.NamedPipes
{
    public class PipeConnectedEventArgs : EventArgs
    {
        //Constructors 
        public PipeConnectedEventArgs(NamedPipeServerStream pipe)
        {
            Pipe = pipe ?? throw new ArgumentNullException(nameof(pipe));
        }

        //Properties 
        public NamedPipeServerStream Pipe { get; }
    }
}