﻿namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class IComparableExtensionsTests
{
    [Test]
    [TestCase(1, 2, 3, false, true)]
    [TestCase(1, 0, 3, false, false)]
    [TestCase(1, 4, 3, false, false)]
    [TestCase(1, 1, 3, false, false)]
    [TestCase(1, 1, 3, true, true)]
    [TestCase(1, 3, 3, false, false)]
    [TestCase(1, 3, 3, true, true)]
    public void IsBetween_ValueIsInRange_ReturnTrue(int left, int value, int right, bool includeEnds, bool expected)
    {
        // Act 
        var result = value.IsBetween(left, right, includeEnds);

        // Assert
        result.Should().Be(expected);
    }
}
