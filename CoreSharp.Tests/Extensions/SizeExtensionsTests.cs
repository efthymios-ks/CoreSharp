using FluentAssertions;
using NUnit.Framework;
using System.Drawing;
using System.IO.Ports;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture()]
    public class SizeExtensionsTests
    {
        [Test()]
        public void ToSizeF_WhenCalled_ReturnSizeFWithSameValues()
        {
            //Arrange 
            var size = new Size(10, 20);

            //Act
            var result = size.ToSizeF();

            //Assert
            result.Width.Should().Be(size.Width);
            result.Height.Should().Be(size.Height);
        }

        [Test]
        [TestCase(10, 10, 20, 20, 20, 20)]
        [TestCase(5, 10, 20, 20, 10, 20)]
        [TestCase(10, 5, 20, 20, 20, 10)]
        [TestCase(10, 20, 20, 20, 10, 20)]
        [TestCase(20, 10, 20, 20, 20, 10)]
        public void Scale_WhenCalled_ReturnSizeScaledProportionally(int fromWidth, int fromHeight, int toWidth, int toHeight, int expectWidth, int expectedHeight)
        {
            //Arrange
            var from = new Size(fromWidth, fromHeight);
            var to = new Size(toWidth, toHeight);
            var expected = new Size(expectWidth, expectedHeight);

            //Act 
            var result = from.Scale(to);

            //Assert
            result.Width.Should().Be(expected.Width);
            result.Height.Should().Be(expected.Height);
            result.Width.Should().BeLessOrEqualTo(to.Width);
            result.Height.Should().BeLessOrEqualTo(to.Height);
        }
    }
}