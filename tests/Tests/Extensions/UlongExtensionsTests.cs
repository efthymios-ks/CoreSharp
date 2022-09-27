namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class UlongExtensionsTests
{
    [Test]
    [TestCase(0ul, "0B")]
    [TestCase(512ul, "512B")]
    [TestCase(1024ul, "1KB")]
    [TestCase(1536ul, "1.5KB")]
    [TestCase(1048576ul, "1MB")]
    [TestCase(1572864ul, "1.5MB")]
    [TestCase(1073741824ul, "1GB")]
    [TestCase(1610612736ul, "1.5GB")]
    public void ToComputerSize_WhenCalled_ReturnSiMetricString(ulong inputValue, string expectedValue)
    {
        // Action
        var value = inputValue.ToComputerSizeCI();

        // Assert
        value.Should().Be(expectedValue);
    }
}
