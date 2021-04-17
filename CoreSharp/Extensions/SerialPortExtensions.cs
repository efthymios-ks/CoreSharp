using CoreSharp.Models;
using System;
using System.IO.Ports;

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
            port = port ?? throw new ArgumentNullException();


            return new SerialPortSettings()
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