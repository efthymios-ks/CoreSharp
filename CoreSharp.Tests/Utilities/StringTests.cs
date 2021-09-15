using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace CoreSharp.Utilities.Tests
{
    [TestFixture]
    public class StringTests
    {
        [Test]
        public void FirstNotEmpty_WhenCalled_ReturbFirstNotEmpty()
        {
            //Arrange 
            var emptyValues = new[] { null, string.Empty, "", " " };
            var expected = "1";
            var values = emptyValues.Append(expected).Concat(emptyValues);

            //Act 
            var result = String.FirstNotEmpty(values);

            //Assert 
            result.Should().Be(expected);
        }
    }
}