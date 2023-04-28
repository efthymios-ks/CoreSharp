using static CoreSharp.Utilities.FormattabeStringX;

namespace CoreSharp.Utilities.Tests;

[TestFixture]
public class FormattableStringsXTests
{
    [Test]
    public void ShowNulls_FormatIsNull_ThrowArgumentNullExcepion()
    {
        // Arrange
        FormattableString formattableString = null;

        // Act 
        Action action = () => ShowNulls(formattableString);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(null, "{null}")]
    [TestCase("", "{empty}")]
    [TestCase(" ", "{empty}")]
    [TestCase("1", "1")]
    public void ShowNulls_WhenCalled_PrintNullOrEmpty(string argument, string expected)
    {
        // Act 
        var result = ShowNulls($"{argument}");

        // Assert
        result.Should().Be(expected);
    }
}
