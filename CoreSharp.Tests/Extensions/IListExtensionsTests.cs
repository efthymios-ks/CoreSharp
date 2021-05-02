using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IListExtensionsTests
    {
        //Fields
        private readonly IList<int> sourceNull = null;
        private readonly IList<int> sourceEmpty = new List<int>();

        //Methods 
        [Test]
        public void Fill_SourceIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => sourceNull.Fill(0);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Fill_WhenCalled_FillListWithSameValue()
        {
            //Arrange
            IList<int> source = new List<int>() { 1, 2, 3 };
            IList<int> expected = new List<int> { 0, 0, 0 };

            //Act
            source.Fill(0);

            //Assert
            source.Should().Equal(expected);
        }

        [Test]
        public void Remove_SourceIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => sourceNull.Remove(i => i > 0);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Remove_ExpressionIsNull_ThrowArgumentNullException()
        {
            //Arrange 
            Func<int, bool> expression = null;

            //Act
            Action action = () => sourceEmpty.Remove(expression);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Remove_WhenCalled_RemoveMatchingItemsAndReturnCount()
        {
            //Arrange  
            var source = new List<int> { 0, 1, 0, 2, 0, 3 };
            var expectedSource = new List<int> { 1, 2, 3 };
            int expectedCount = 3;

            //Act
            var removedCount = source.Remove(i => i == 0);

            //Assert
            removedCount.Should().Be(expectedCount);
            source.Should().Equal(expectedSource);
        }

        [Test]
        public void InsertRange_SourceIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => sourceNull.InsertRange(0, 0, 1, 2);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void InsertRange_ValuesIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => sourceEmpty.InsertRange(0, values: null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(1)]
        public void InsertRange_IndexInvalid_ThrowArgumentOutOfRangeException(int index)
        {
            //Arrange
            var source = new List<int>();

            //Act
            Action action = () => source.InsertRange(index, 0, 1, 2);

            //Assert
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void InsertRange_WhenCalled_InsertValuesToGivenPosition()
        {
            //Arrange
            var source = new List<int> { 0, 1, 4, 5 };
            var expected = new List<int> { 0, 1, 2, 3, 4, 5 };

            //Act
            source.InsertRange(2, 2, 3);

            //Assert
            source.Should().Equal(expected);
        }
    }
}