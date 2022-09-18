namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class StringXTests
{
    [Test]
    public void FirstNotEmpty_WhenCalled_ReturnFirstNotEmpty()
    {
        // Arrange 
        var emptyValues = new[] { null, string.Empty, "" };
        const string expected = "1";
        var values = emptyValues.Append(expected).Concat(emptyValues);

        // Act 
        var result = StringX.FirstNotEmpty(values);

        // Assert 
        result.Should().Be(expected);
    }
}
