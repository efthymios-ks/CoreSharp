namespace CoreSharp.EqualityComparers.Tests;

[TestFixture]
public class KeyEqualityComparerTests
{
    // Fields
    private readonly IEqualityComparer<int?> _equalityComparer = new KeyEqualityComparer<int?, int?>(i => i);

    // Methods
    [Test]
    public void Equals_OnlyLeftIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(null, 0);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_OnlyRightIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(0, null);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_BothAreNull_ReturnTrue()
    {
        // Act
        var equals = _equalityComparer.Equals(null, null);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void Equals_DifferentKeyValues_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(0, 1);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_SameKeyValues_ReturnTrue()
    {
        // Act
        var equals = _equalityComparer.Equals(0, 0);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void GetHashCode_SourceIsNull_ReturnDefaultHashCode()
    {
        // Arrange 
        var expected = default(int?).GetHashCode();

        // Act
        var hashCode = _equalityComparer.GetHashCode(null);

        // Assert 
        hashCode.Should().Be(expected);
    }
}
