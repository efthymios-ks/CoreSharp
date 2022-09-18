namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class FloatExtensionsTests
{
    [Test]
    [TestCase(1, 1, 3, 1, 10, 1)]
    [TestCase(2, 1, 3, 1, 10, 5.5f)]
    [TestCase(3, 1, 3, 1, 10, 10)]
    public void Map_WhenCalled_MapAndReturnValueToNewRange(float value, float fromLow, float fromHigh, float toLow, float toHigh, float expected)
    {
        // Act
        var result = value.Map(fromLow, fromHigh, toLow, toHigh);

        // Assert
        result.Should().Be(expected);
    }
}
