using CoreSharp.EqualityComparers;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests.EqualityComparers;

[TestFixture]
public class EnumerableEqualityComparerTests
{
    //Fields
    private readonly IEqualityComparer<IEnumerable<int>> _equalityComparer = new EnumerableEqualityComparer<int>();
    private readonly IEnumerable<int> _sourceNull = null;
    private readonly IEnumerable<int> _sourceEmpty = Enumerable.Empty<int>();

    //Methods
    [Test]
    public void Equals_OnlyLeftIsNull_ReturnFalse()
    {
        //Act
        var result = _equalityComparer.Equals(_sourceNull, _sourceEmpty);

        //Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void Equals_OnlyRightIsNull_ReturnFalse()
    {
        //Act
        var result = _equalityComparer.Equals(_sourceEmpty, _sourceNull);

        //Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void Equals_BothAreNull_ReturnTrue()
    {
        //Act
        var result = _equalityComparer.Equals(_sourceNull, _sourceNull);

        //Assert 
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_DifferentValues_ReturnFalse()
    {
        //Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 2, 3 };

        //Act
        var result = _equalityComparer.Equals(left, right);

        //Assert 
        result.Should().BeFalse();
    }

    [Test]
    public void Equals_SameValuesSameOrder_ReturnTrue()
    {
        //Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 1, 2 };

        //Act
        var result = _equalityComparer.Equals(left, right);

        //Assert 
        result.Should().BeTrue();
    }

    [Test]
    public void Equals_SameValuesDifferentOrder_ReturnTrue()
    {
        //Arrange
        var left = new[] { 1, 2 };
        var right = new[] { 2, 1 };

        //Act
        var result = _equalityComparer.Equals(left, right);

        //Assert 
        result.Should().BeTrue();
    }

    [Test]
    public void GetHashCode_SourceIsNull_ReturnDefaultHashCode()
    {
        //Arrange 
        var expected = default(int).GetHashCode();

        //Act
        var result = _equalityComparer.GetHashCode(_sourceNull);

        //Assert 
        result.Should().Be(expected);
    }
}
