namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class TimeSpanExtensionsTests
{
    [Test]
    [TestCase(1, 1, 1, 1, 1, "1d 1h 1m 1s 1ms")]
    [TestCase(1, 0, 1, 0, 1, "1d 1m 1ms")]
    public void ToStringReadable_WhenCalled_DivideAndReturnTimeSpanValuesWithUnits(int days, int hours, int minutes, int seconds, int millis, string expected)
    {
        // Arrange
        var time = new TimeSpan(days, hours, minutes, seconds, millis);

        // Act
        var result = time.ToStringReadable();

        // Result
        result.Should().Be(expected);
    }
}
