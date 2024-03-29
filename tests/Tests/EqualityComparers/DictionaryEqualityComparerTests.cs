﻿namespace CoreSharp.EqualityComparers.Tests;

[TestFixture]
public class DictionaryEqualityComparerTests
{
    // Fields
    private readonly IEqualityComparer<IDictionary<string, int>> _equalityComparer = new DictionaryEqualityComparer<string, int>();
    private readonly IDictionary<string, int> _dictionaryNull;
    private readonly IDictionary<string, int> _dictionaryEmpty = new Dictionary<string, int>();

    // Methods
    [Test]
    public void Equals_OnlyLeftIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(_dictionaryNull, _dictionaryEmpty);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_OnlyRightIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(_dictionaryEmpty, _dictionaryNull);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_BothAreNull_ReturnTrue()
    {
        // Act
        var equals = _equalityComparer.Equals(_dictionaryNull, _dictionaryNull);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void Equals_DifferentValues_ReturnFalse()
    {
        // Arrange
        var left = new Dictionary<string, int>
        {
            { "1", 1 },
            { "2", 2 }
        };
        var right = new Dictionary<string, int>
        {
            { "2", 2 },
            { "3", 3 }
        };

        // Act
        var equals = _equalityComparer.Equals(left, right);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_SameValuesSameOrder_ReturnTrue()
    {
        // Arrange
        var left = new Dictionary<string, int>
        {
            { "1", 1 },
            { "2", 2 }
        };
        var right = new Dictionary<string, int>
        {
            { "1", 1 },
            { "2", 2 }
        };

        // Act
        var equals = _equalityComparer.Equals(left, right);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void Equals_SameValuesDifferentOrder_ReturnTrue()
    {
        // Arrange
        var left = new Dictionary<string, int>
        {
            { "1", 1 },
            { "2", 2 }
        };
        var right = new Dictionary<string, int>
        {
            { "2", 2 },
            { "1", 1 }
        };

        // Act
        var equals = _equalityComparer.Equals(left, right);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void GetHashCode_SourceIsNull_ReturnDefaultHashCode()
    {
        // Arrange
        var expected = default(int).GetHashCode();

        // Act
        var hashCode = _equalityComparer.GetHashCode(_dictionaryNull);

        // Assert 
        hashCode.Should().Be(expected);
    }
}
