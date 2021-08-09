using CoreSharp.Extensions;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        //Fields
        private const string StringNull = null;
        private const string StringEmpty = "";

        //Methods 
        [Test]
        public void Truncate_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.Truncate(3);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Truncate_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => StringEmpty.Truncate(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Truncate_WhenCalled_ReturnTruncatedString()
        {
            //Arrange
            string input = "12345";
            string expected = "123";

            //Act
            var result = input.Truncate(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatAsciiControls_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.FormatAsciiControls();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatAsciiControls_WhenCalled_ReplaceAsciiControlCharsAndReturn()
        {
            //Arrange
            char SOH = Convert.ToChar(1);
            char EOT = Convert.ToChar(4);
            string input = $"{SOH}_Hello_{EOT}";
            string expected = "<SOH>_Hello_<EOT>";

            //Act
            var result = input.FormatAsciiControls('<', '>');

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void SplitChunks_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.SplitChunks(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void SplitChunks_ChunckSizeIsZeroOrLess_ThrowArgumentOutOfRangeException(int chunkSize)
        {
            //Act
            Action action = () => StringEmpty.SplitChunks(chunkSize);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void SplitChunks_WhenCalled_SplitInputInChunks()
        {
            //Arrange 
            string input = "12345";
            var expected = new[] { "12", "34", "5" };

            //Act
            var result = input.SplitChunks(2);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void PadCenter_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.PadCenter(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void PadCenter_TotalWidthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => StringEmpty.PadCenter(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void PadCenter_WhenCalled_PadCenterAndReturnInput()
        {
            //Arrange
            string input = "123";
            char padCharacter = ' ';
            int totalWidth = 7;
            string expected = "  123  ";


            //Act
            var result = input.PadCenter(totalWidth, padCharacter);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RemoveFirst_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.RemoveFirst("1");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveFirst_ValueIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.RemoveFirst(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveFirst_WhenCalled_RemoveFirstValueOccurence()
        {
            //Arrange
            string input = "1 A 2 A 3 A";
            string value = "A";
            string expected = "1  2 A 3 A";

            //Act
            var result = input.RemoveFirst(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RemoveLast_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.RemoveLast("1");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveLast_ValueIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.RemoveLast(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveLast_WhenCalled_RemoveFirstValueOccurence()
        {
            //Arrange
            string input = "1 A 2 A 3 A";
            string value = "A";
            string expected = "1 A 2 A 3 ";

            //Act
            var result = input.RemoveLast(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RemoveAll_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.RemoveAll("1");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveAll_ValueIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.RemoveAll(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void RemoveAll_WhenCalled_RemoveFirstValueOccurence()
        {
            //Arrange
            string input = "1 A 2 A 3 A";
            string value = "A";
            string expected = "1  2  3 ";

            //Act
            var result = input.RemoveAll(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Left_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.Left(3);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Left_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => StringEmpty.Left(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Left_WhenCalled_ReturnLeftSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "123";

            //Act
            var result = input.Left(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Right_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.Right(3);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Right_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => StringEmpty.Right(-1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Right_WhenCalled_ReturnRightSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "345";

            //Act
            var result = input.Right(3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Mid_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.Mid(2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("123", -1)]
        [TestCase("123", 4)]
        public void Mid_IndexIsOutOfRange_ThrowArgumentOutOfRangeException(string input, int index)
        {
            //Act
            Action action = () => input.Mid(index, 2);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Mid_LengthIsNegative_ThrowArgumentOutOfRangeException()
        {
            //Act
            Action action = () => StringEmpty.Mid(0, -1);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Mid_WhenCalled_ReturnMidSubstring()
        {
            //Arrange
            string input = "12345";
            string expected = "234";

            //Act
            var result = input.Mid(1, 3);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatWith_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.FormatWith(1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.FormatWith(formatProvider: null, arguments: 1);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_ParametersIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.FormatWith(CultureInfo.InvariantCulture, arguments: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FormatWith_WhenCalled_ReturnFormattedString()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            var culture = CultureInfo.CurrentCulture;
            string expected = string.Format(culture, format, value);

            //Act
            var result = format.FormatWith(culture, value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void FormatWithCI_WhenCalled_FormatStringWithCultureInvariant()
        {
            //Arrange
            string format = "{0}";
            int value = 1000;
            var culture = CultureInfo.InvariantCulture;
            string expected = "1000";

            //Act
            var result = format.FormatWith(culture, value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void EqualsAnyCI_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.EqualsAnyCI("a");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void EqualsAnyCI_ValuesIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.EqualsAnyCI(values: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("Ab", "Ac", false)]
        [TestCase("Ab", "ab", true)]
        [TestCase("Ab", "AB", true)]
        public void EqualsAnyCI_WhenCalled_ReturnTrueIfEqualsAnyIgnoringCase(string input, string value, bool expected)
        {
            //Act
            var result = input.EqualsAnyCI(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void StartsWithAnyCI_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.StartsWithAnyCI("a");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StartsWithAnyCI_ValuesIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.StartsWithAnyCI(values: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("Ab", "b", false)]
        [TestCase("Ab", "a", true)]
        [TestCase("Ab", "A", true)]
        public void StartsWithAnyCI_WhenCalled_ReturnTrueIfStartsWithAnyIgnoringCase(string input, string value, bool expected)
        {
            //Act
            var result = input.StartsWithAnyCI(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void EndsWithAnyCI_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.EndsWithAnyCI("a");

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void EndsWithAnyCI_ValuesIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.EndsWithAnyCI(values: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("Ab", "a", false)]
        [TestCase("Ab", "b", true)]
        [TestCase("Ab", "B", true)]
        public void EndsWithAnyCI_WhenCalled_ReturnTrueIfStartsWithAnyIgnoringCase(string input, string value, bool expected)
        {
            //Act
            var result = input.EndsWithAnyCI(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(null, true)]
        [TestCase(StringEmpty, true)]
        [TestCase(" ", false)]
        [TestCase("1", false)]
        public void IsNullOrEmpty_WhenCalled_ReturnTrueIfNullOrEmpty(string value, bool expected)
        {
            //Act 
            var result = value.IsNullOrEmpty();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(null, true)]
        [TestCase(StringEmpty, true)]
        [TestCase(" ", true)]
        [TestCase("1", false)]
        public void IsNullOrWhiteSpace_WhenCalled_ReturnTrueIfNullOrWhiteSpace(string value, bool expected)
        {
            //Act 
            var result = value.IsNullOrWhiteSpace();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Reverse_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.Reverse();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("12", "21")]
        [TestCase("123", "321")]
        public void Reverse_WhenCalled_ReturnReversedInput(string input, string expected)
        {
            //Act
            var result = input.Reverse();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        [TestCase(null, StringEmpty)]
        [TestCase(StringEmpty, null)]
        public void Erase_AnyArgumentIsNull_ThrowArgumentNullException(string input, string value)
        {
            //Act 
            Action action = () => input.Erase(value);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("1-2-3", "-", "123")]
        public void Erase_WhenCalled_EraseAllOccurencesOfGivenValue(string input, string value, string expected)
        {
            //Act 
            var result = input.Erase(value);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void SafeTrim_TrimCharsIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.SafeTrim(trimChars: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(null, StringEmpty)]
        [TestCase(StringEmpty, StringEmpty)]
        [TestCase(StringEmpty, StringEmpty)]
        [TestCase(" ", StringEmpty)]
        [TestCase(" a ", "a")]
        public void SafeTrim_WhenCalled_ReplaceNullWithStringEmptyThenTrimAndReturn(string input, string expected)
        {
            //Act
            var result = input.SafeTrim();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToEntity_OptionsIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToEntity<DummyClass>(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToEntity_WhenCalled_MapItemPropertiesAndReturnTrue()
        {
            //Arrange
            int id = 1;
            string name = "Efthymios";
            string json = "{\"id\": {id}, \"name\": \"{name}\"}";
            json = json
                .Replace("{id}", $"{id}")
                .Replace("{name}", name);

            //Act
            var result = json.ToEntity<DummyClass>();

            //Assert  
            result.Id.Should().Be(id);
            result.Name.Should().Be(name);
        }

        [Test]
        public void ToExpandoObject_WhenJsonIsJObject_ReturnExpandoObject()
        {
            //Arrange
            int id = 1;
            string name = "Efthymios";
            string json = "{\"id\": {id}, \"name\": \"{name}\"}";
            json = json
                .Replace("{id}", $"{id}")
                .Replace("{name}", name);

            //Act
            dynamic result = json.ToExpandoObject();


            //Assert  
            ((int)result.id).Should().Be(id);
            ((string)result.name).Should().Be(name);
        }

        [Test]
        public void ToExpandoObject_WhenJsonIsJArray_ReturnIEnumerableExpandoObject()
        {
            //Arrange
            int id = 1;
            string name = "Efthymios";
            string json = "[{\"id\": {id}, \"name\": \"{name}\"}]";
            json = json
                .Replace("{id}", $"{id}")
                .Replace("{name}", name);

            //Act
            dynamic result = json.ToExpandoObject();

            //Assert 
            ((int)result[0].id).Should().Be(id);
            ((string)result[0].name).Should().Be(name);
        }

        [Test]
        public void GetLines_InputIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringNull.GetLines();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetLines_WhenCalled_SplitAndReturnLines()
        {
            //Arrange
            var lines = new[]
            {
                "Line 1",
                "Line 2",
                "Line 3"
            };
            string joined = string.Join(Environment.NewLine, lines);

            //Act
            var result = joined.GetLines();

            //Assert
            result.Should().Equal(lines);
        }

        [Test]
        public void Replace_InputIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var dictionary = new Dictionary<string, string>();

            //Act
            Action action = () => StringNull.Replace(dictionary);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Replace_DictionaryIsNull_ThrowArgumentNulLException()
        {
            //Arrange
            Dictionary<string, string> dictionary = null;

            //Act
            Action action = () => StringEmpty.Replace(dictionary);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Replace_WhenCalled_ReplaceDictionaryValuesAndReturnString()
        {
            //Arrange
            string input = "Key1, Key2, Key3";
            var dictionary = new Dictionary<string, int>
            {
                { "Key1", 1 },
                { "Key2", 2 },
                { "Key3", 3 }
            };
            var expected = "1, 2, 3";

            //Act
            var result = input.Replace(dictionary);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToInt_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToInt(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        [TestCase("1.1", null)]
        public void ToInt_InputValid_ReturnInt(string input, int? expected)
        {
            //Act
            var result = input.ToInt(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToLong_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToLong(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("1.1", null)]
        public void ToLong_InputValid_ReturnLong(string input, long? expected)
        {
            //Act
            var result = input.ToLong(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToShort_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToShort(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("1.1", null)]
        public void ToShort_InputValid_ReturnShort(string input, short? expected)
        {
            //Act
            var result = input.ToShort(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToFloat_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToFloat(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("-1", -1f)]
        [TestCase("1", 1f)]
        [TestCase("1.1", 1.1f)]
        public void ToFloat_InputValid_ReturnFloat(string input, float? expected)
        {
            //Act
            var result = input.ToFloat(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToDouble_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToDouble(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("1.1", 1.1)]
        public void ToDouble_InputValid_ReturnDouble(string input, double? expected)
        {
            //Act
            var result = input.ToDouble(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToDecimal_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToDecimal(null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("-1", -1)]
        [TestCase("1", 1)]
        [TestCase("1.1", 1.1)]
        public void ToDecimal_InputValid_ReturnDecimal(string input, double? expected)
        {
            //Arrange
            var expectedDecimal = (decimal?)expected;

            //Act
            var result = input.ToDecimal(NumberStyles.Any, CultureInfo.InvariantCulture);

            //Assert
            result.Should().Be(expectedDecimal);
        }

        [Test]
        [TestCase("A", null)]
        [TestCase("false", false)]
        [TestCase("true", true)]
        [TestCase("-1", null)]
        [TestCase("0", false)]
        [TestCase("1", true)]
        [TestCase("no", false)]
        [TestCase("yes", true)]
        public void ToBool_InputIsValid_ReturnBool(string input, bool? expected)
        {
            //Assert
            var result = input.ToBool();

            //Assert
            result.Should().Be(expected);
        }

        [Test]
        public void ToDateTime_DateTimeFormatIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToDateTime(StringNull);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToDateTime_FormatProviderIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => StringEmpty.ToDateTime(StringEmpty, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ToDateTime_InputValid_ReturnDateTime()
        {
            //Arrange 
            var expected = new DateTime(2021, 5, 4, 11, 17, 45);
            var dateTimeFormat = "u";
            var formatProvider = CultureInfo.InvariantCulture;
            var input = expected.ToString(dateTimeFormat, formatProvider);

            //Act
            var result = input.ToDateTime(dateTimeFormat, formatProvider);

            //Assert
            result.Should().Be(expected);
        }
    }
}