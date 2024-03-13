namespace CoreSharp.EqualityComparers.Tests;

[TestFixture]
public class PrimitiveEqualityComparerTests
{
    [Test]
    public void Equals_WhenBothObjectsAreNull_ShouldReturnTrue()
    {
        // Arrange
        var comparer = new PrimitiveEqualityComparer<object>();
        object x = null;
        object y = null;

        // Act
        var result = comparer.Equals(x, y);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_WhenOneObjectIsNull_ShouldReturnFalse()
    {
        // Arrange
        var comparer = new PrimitiveEqualityComparer<object>();
        object x = new { Prop = "value" };
        object y = null;

        // Act
        var result = comparer.Equals(x, y);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Equals_WhenBothObjectsAreSameInstance_ShouldReturnTrue()
    {
        // Arrange
        var obj = new { Prop = "value" };
        var comparer = new PrimitiveEqualityComparer<object>();

        // Act
        var result = comparer.Equals(obj, obj);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_WhenObjectsAreEqual_ShouldReturnTrue()
    {
        // Arrange
        var obj1 = new { Prop = "value" };
        var obj2 = new { Prop = "value" };
        var comparer = new PrimitiveEqualityComparer<object>();

        // Act
        var result = comparer.Equals(obj1, obj2);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_WhenObjectsAreNotEqual_ShouldReturnFalse()
    {
        // Arrange
        var obj1 = new { Prop = "value1" };
        var obj2 = new { Prop = "value2" };
        var comparer = new PrimitiveEqualityComparer<object>();

        // Act
        var result = comparer.Equals(obj1, obj2);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void GetHashCode_WhenObjectIsNull_ShouldReturnZero()
    {
        // Arrange
        var comparer = new PrimitiveEqualityComparer<object>();
        object obj = null;

        // Act
        var result = comparer.GetHashCode(obj);

        // Assert
        result.Should().Be(0);
    }

    [Test]
    public void GetHashCode_WhenObjectIsEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var obj1 = new { Prop = "value" };
        var obj2 = new { Prop = "value" };
        var comparer = new PrimitiveEqualityComparer<object>();

        // Act
        var hash1 = comparer.GetHashCode(obj1);
        var hash2 = comparer.GetHashCode(obj2);

        // Assert
        hash1.Should().Be(hash2);
    }
}
