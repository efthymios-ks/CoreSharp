namespace CoreSharp.EqualityComparers.Tests;

[TestFixture]
public class EnumerableEqualityComparerTests
{
    // Fields
    private readonly IEqualityComparer<IEnumerable<int>> _equalityComparer = new EnumerableEqualityComparer<int>();
    private readonly IEnumerable<int> _sourceNull;
    private readonly IEnumerable<int> _sourceEmpty = Enumerable.Empty<int>();

    // Methods
    [Test]
    public void Equals_OnlyLeftIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(_sourceNull, _sourceEmpty);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_OnlyRightIsNull_ReturnFalse()
    {
        // Act
        var equals = _equalityComparer.Equals(_sourceEmpty, _sourceNull);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_BothAreNull_ReturnTrue()
    {
        // Act
        var equals = _equalityComparer.Equals(_sourceNull, _sourceNull);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void Equals_DifferentValues_ReturnFalse()
    {
        // Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 2, 3 };

        // Act
        var equals = _equalityComparer.Equals(left, right);

        // Assert 
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_SameValuesSameOrder_ReturnTrue()
    {
        // Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 1, 2 };

        // Act
        var equals = _equalityComparer.Equals(left, right);

        // Assert 
        equals.Should().BeTrue();
    }

    [Test]
    public void Equals_SameValuesDifferentOrder_ReturnTrue()
    {
        // Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 2, 1 };

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
        var hashCode = _equalityComparer.GetHashCode(_sourceNull);

        // Assert 
        hashCode.Should().Be(expected);
    }
}
