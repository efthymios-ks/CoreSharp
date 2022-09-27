namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class StringXTests
{
    [Test]
    public void FirstNotEmpty_WhenCalled_ReturnFirstNotEmpty()
    {
        // Arrange 
        const string expectedValue = "1";
        var values = new[] { null, string.Empty, "", expectedValue, null, string.Empty, "" };

        // Act 
        var value = StringX.FirstNotEmpty(values);

        // Assert 
        value.Should().Be(expectedValue);
    }
}
