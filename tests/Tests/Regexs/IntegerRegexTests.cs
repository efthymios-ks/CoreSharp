using FluentAssertions;
using NUnit.Framework;
using System.Globalization;

namespace CoreSharp.Regexs.Tests
{
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
        public void IntegerRegex_InputIsInteger_ReturnTrue(string input, string cultureName, bool isMatch)
        {
            //Arrange
            var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
            var integerRegex = new IntegerRegex(input, cultureInfo);

            //Act 
            var result = integerRegex.IsMatch;

            //Assert
            result.Should().Be(isMatch);
        }

        [Test]
        [TestCase("-1", "el-GR", '-')]
        [TestCase("1", "el-GR", null)]
        [TestCase("+1", "el-GR", '+')]
        public void IntegerRegex_WhenCalled_MatchGroupSign(string input, string cultureName, char? groupSign)
        {
            //Arrange
            var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
            var integerRegex = new IntegerRegex(input, cultureInfo);

            //Act 
            var result = integerRegex.Sign;

            //Assert
            result.Should().Be(groupSign);
        }

        [Test]
        [TestCase("-1", "el-GR", "1")]
        [TestCase("1", "el-GR", "1")]
        [TestCase("+1", "el-GR", "1")]
        public void IntegerRegex_WhenCalled_MatchGroupValue(string input, string cultureName, string groupValue)
        {
            //Arrange
            var cultureInfo = CultureInfo.GetCultureInfo(cultureName);
            var integerRegex = new IntegerRegex(input, cultureInfo);

            //Act 
            var result = integerRegex.Value;

            //Assert
            result.Should().Be(groupValue);
        }
    }
}
