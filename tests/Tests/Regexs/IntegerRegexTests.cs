namespace CoreSharp.Regexs.Tests;

[TestFixture]
public class IntegerRegexTests
{
    [Test]
    [TestCase("-1", "el-GR", true)]
    [TestCase("1", "el-GR", true)]
    [TestCase("+1", "el-GR", true)]
    [TestCase("-1.0", "el-GR", false)]
    [TestCase("1.0", "el-GR", false)]
    [TestCase("+1.0", "el-GR", false)]
    [TestCase("-1,0", "el-GR", false)]
    [TestCase("1,0", "el-GR", false)]
    [TestCase("+1,0", "el-GR", false)]
    [TestCase("+1.000.000", "el-GR", true)]
    [TestCase("1.000.000", "el-GR", true)]
    [TestCase("+1.000.000", "el-GR", true)]
    [TestCase("+1,000,000", "el-GR", false)]
    [TestCase("1,000,000", "el-GR", false)]
    [TestCase("+1,000,000", "el-GR", false)]
    [TestCase("+1.000.000,0", "el-GR", false)]
    [TestCase("1.000.000,0", "el-GR", false)]
    [TestCase("+1.000.000,0", "el-GR", false)]
    [TestCase("+1,000,000.0", "el-GR", false)]
    [TestCase("1,000,000.0", "el-GR", false)]
    [TestCase("+1,000,000.0", "el-GR", false)]
    public void IntegerRegex_InputIsInteger_ReturnTrue(string input, string cultureName, bool expectedIsMatch)
    {
        // Arrange
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        var integerRegex = new IntegerRegex(input, cultureInfo);

        // Act 
        var isMatch = integerRegex.IsMatch;

        // Assert
        isMatch.Should().Be(expectedIsMatch);
    }

    [Test]
    [TestCase("-1", "el-GR", '-')]
    [TestCase("1", "el-GR", null)]
    [TestCase("+1", "el-GR", '+')]
    public void IntegerRegex_WhenCalled_MatchGroupSign(string input, string cultureName, char? exppectedGroupSign)
    {
        // Arrange
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        var integerRegex = new IntegerRegex(input, cultureInfo);

        // Act 
        var sign = integerRegex.Sign;

        // Assert
        sign.Should().Be(exppectedGroupSign);
    }

    [Test]
    [TestCase("-1", "el-GR", "1")]
    [TestCase("1", "el-GR", "1")]
    [TestCase("+1", "el-GR", "1")]
    public void IntegerRegex_WhenCalled_MatchGroupValue(string input, string cultureName, string expecteGroupValue)
    {
        // Arrange
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
        var integerRegex = new IntegerRegex(input, cultureInfo);

        // Act 
        var value = integerRegex.Value;

        // Assert
        value.Should().Be(expecteGroupValue);
    }
}
