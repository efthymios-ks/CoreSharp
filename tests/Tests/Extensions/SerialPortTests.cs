using System.IO.Ports;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class SerialPortTests
{
    [Test]
    public void GetSettings_PortIsNull_ThrowArgumentNullException()
    {
        //Arrange 
        SerialPort port = null;

        //Act
        Action action = () => port.GetSettings();

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetSettings_WhenCalled_ReturnMatchingSettings()
    {
        //Arrange 
        var port = new SerialPort("COM1", 4800, Parity.Even, 7, StopBits.One)
        {
            Encoding = Encoding.UTF8,
            ReadTimeout = TimeSpan.FromMilliseconds(500).Milliseconds
        };

        //Act
        var result = port.GetSettings();

        //Assert
        result.PortName.Should().Be(port.PortName);
        result.BaudRate.Should().Be(port.BaudRate);
        result.Parity.Should().Be(port.Parity);
        result.DataBits.Should().Be(port.DataBits);
        result.StopBits.Should().Be(port.StopBits);
        result.TextEncoding.CodePage.Should().Be(port.Encoding.CodePage);
        result.Timeout.Milliseconds.Should().BeOneOf(port.ReadTimeout, port.WriteTimeout);
    }
}
