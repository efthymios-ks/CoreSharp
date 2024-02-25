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
    public void IsEmptyJsonTest(string input, bool expectedIsEmpty)
    {
        // Assert
        var isEmpty = JsonUtils.IsEmpty(input);

        // Assert
        isEmpty.Should().Be(expectedIsEmpty);
    }
}
