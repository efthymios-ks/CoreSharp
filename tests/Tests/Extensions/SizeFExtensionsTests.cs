using System.Drawing;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class SizeFExtensionsTests
{
    [Test]
    public void ToSize_WhenCalled_ReturnSizeWithSameValues()
    {
        //Arrange 
        var size = new SizeF(10, 20);

        //Act
        var result = size.ToSize();

        //Assert
        result.Width.Should().Be((int)size.Width);
        result.Height.Should().Be((int)size.Height);
    }

    [Test]
    [TestCase(10, 10, 20, 20, 20, 20)]
    [TestCase(5, 10, 20, 20, 10, 20)]
    [TestCase(10, 5, 20, 20, 20, 10)]
    [TestCase(10, 20, 20, 20, 10, 20)]
    [TestCase(20, 10, 20, 20, 20, 10)]
    public void Scale_WhenCalled_ReturnSizeFScaledProportionally(float fromWidth, float fromHeight, float toWidth, float toHeight, float expectWidth, float expectedHeight)
    {
        //Arrange
        var from = new SizeF(fromWidth, fromHeight);
        var to = new SizeF(toWidth, toHeight);
        var expected = new SizeF(expectWidth, expectedHeight);

        //Act 
        var result = from.Scale(to);

        //Assert
        result.Width.Should().Be(expected.Width);
        result.Height.Should().Be(expected.Height);
        result.Width.Should().BeLessOrEqualTo(to.Width);
        result.Height.Should().BeLessOrEqualTo(to.Height);
    }
}
