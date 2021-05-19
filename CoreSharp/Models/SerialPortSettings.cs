using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace CoreSharp.Models
{
    /// <summary>
    /// Short SerialPort settings class. 
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class SerialPortSettings
    {
        // Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => $"{nameof(PortName)}={PortName}, {nameof(BaudRate)}={BaudRate}, {nameof(Parity)}={Parity}";

        public string PortName { get; set; } = string.Empty;

        public int BaudRate { get; set; } = 9600;

        public Parity Parity { get; set; } = Parity.None;

        public int DataBits { get; set; } = 8;

        public StopBits StopBits { get; set; } = StopBits.None;

        public Encoding TextEncoding { get; set; } = Encoding.ASCII;

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(15);

        //Methods 
        public override string ToString()
        {
            return $"{nameof(PortName)}={PortName}, {nameof(BaudRate)}={BaudRate:F0}, {nameof(Parity)}={Parity}, {nameof(DataBits)}={DataBits:F0}, {nameof(StopBits)}={StopBits}";
        }
    }
}
