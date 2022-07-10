﻿using CoreSharp.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Tests.Extensions;

[TestFixture]
public class ArrayExtensionsTests
{
    //Fields
    private readonly int[,] _sourceNull = null;

    [Test]
    public void GetRow_SourceIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _sourceNull.GetRow(0);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(0, 0, -1)]
    [TestCase(0, 0, 1)]
    public void GetRow_RowIndexInvalid_ThrowArgumentOutOfRangeException(int rows, int columns, int row)
    {
        //Arrange
        var source = new int[rows, columns];

        //Act
        Action action = () => source.GetRow(row);

        //Assert
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GetRow_WhenCalled_ReturnRow()
    {
        //Arrange
        var source = new[,]
        {
            { 1, 2 },
            { 3, 4 }
        };
        var expected = new[] { 3, 4 };

        //Act
        var result = source.GetRow(1);

        //Assert
        result.Should().Equal(expected);
    }

    [Test]
    public void GetColumn_SourceIsNull_ThrowArgumentNullException()
    {
        //Act
        Action action = () => _sourceNull.GetColumn(0);

        //Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    [TestCase(0, 0, -1)]
    [TestCase(0, 0, 1)]
    public void GetColumn_RowIndexInvalid_ThrowArgumentOutOfRangeException(int rows, int columns, int row)
    {
        //Arrange
        var source = new int[rows, columns];

        //Act
        Action action = () => source.GetColumn(row);

        //Assert
        action.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GetColumn_WhenCalled_ReturnColumn()
    {
        //Arrange
        var source = new[,]
        {
            { 1, 2 },
            { 3, 4 }
        };
        var expected = new[] { 2, 4 };

        //Act
        var result = source.GetColumn(1);

        //Assert
        result.Should().Equal(expected);
    }
}
