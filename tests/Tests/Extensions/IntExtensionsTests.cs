namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class IntExtensionsTests
{
    [Test]
    [TestCase(1, 1, 3, 1, 10, 1)]
    [TestCase(2, 1, 3, 1, 10, 5)]
    [TestCase(3, 1, 3, 1, 10, 10)]
    public void Map_WhenCalled_MapAndReturnValueToNewRange(int value, int fromLow, int fromHigh, int toLow, int toHigh, int expected)
    {
        // Act
        var result = value.Map(fromLow, fromHigh, toLow, toHigh);

        // Assert
        result.Should().Be(expected);
    }
}
