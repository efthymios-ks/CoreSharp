using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Utilities.Tests
{
    [TestFixture]
    public class JsonTests
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
            //Assert
            var result = Json.IsEmpty(input);

            //Assert
            result.Should().Be(expected);
        }
    }
}
