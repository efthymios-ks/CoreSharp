namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class StringUtilsTests
{
    [Test]
    public void FirstNotEmpty_WhenCalled_ReturnFirstNotEmpty()
    {
        // Arrange 
        const string expectedValue = "1";
        var values = new[]
        {
            null,
            string.Empty,
            "",
            expectedValue,
            null,
            string.Empty,
            ""
        };

        // Act 
        var value = StringUtils.FirstNotEmpty(values);

        // Assert 
        value.Should().Be(expectedValue);
    }
}
