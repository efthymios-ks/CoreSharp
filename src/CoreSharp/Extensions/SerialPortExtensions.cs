using CoreSharp.Models;
using System;
using System.IO.Ports;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="SerialPort"/> extensions.
/// </summary>
public static class SerialPortExtensions
{
    /// <summary>
    /// Extract <see cref="SerialPortSettings"/> from given <see cref="SerialPort"/>.
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
