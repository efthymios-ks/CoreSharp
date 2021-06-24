using System;
using System.IO.Ports;
using CoreSharp.Models;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// SerialPort extensions. 
    /// </summary>
    public static partial class SerialPortExtensions
    {
        /// <summary>
        /// Extract SerialPortSettings from SerialPort. 
        /// </summary>
        public static SerialPortSettings GetSettings(this SerialPort port)
        {
            _ = port ?? throw new ArgumentNullException(nameof(port));

            return new()
            {
                PortName = port.PortName,
                BaudRate = port.BaudRate,
                Parity = port.Parity,
                DataBits = port.DataBits,
                StopBits = port.StopBits,
                TextEncoding = port.Encoding,
                Timeout = TimeSpan.FromMilliseconds(Math.Min(port.ReadTimeout, port.WriteTimeout))
            };
        }
    }
}