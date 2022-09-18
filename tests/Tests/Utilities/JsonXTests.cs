namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class JsonXTests
{
    [Test]
    [TestCase(null, true)]
    [TestCase(" ", true)]
    [TestCase(" { } ", true)]
    [TestCase(" [ ]", true)]
    [TestCase(" [ { } ] ", true)]
    [TestCase("{id=1}", false)]
    public void IsEmptyJsonTest(string input, bool expected)
    {
        // Assert
        var result = JsonX.IsEmpty(input);

        // Assert
        result.Should().Be(expected);
    }
}
