using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ICollectionExtensionsTests
    {
        //Fields
        private readonly IList<int> sourceNull = null;
        private readonly IList<int> sourceEmpty = new List<int>();

        //Methods
        [Test]
        public void AddRange_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            ICollection<int> source = null;

            //Act
            Action action = () => source.AddRange();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddRange_ItemsIsNull_ThrowArgumentNullException()
        {
            //Arrange
            ICollection<int> source = new List<int>();
            IEnumerable<int> items = null;

            //Act
            Action action = () => source.AddRange(items);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void AddRange_WhenCalled_AddItemsToCollection()
        {
            //Arrange
            ICollection<int> source = new Collection<int>() { 1, 2, 3 };
            var items = new[] { 4, 5, 6 };
            var expected = new Collection<int>() { 1, 2, 3, 4, 5, 6 };

            //Act
            source.AddRange(items);

            //Assert
            source.Should().Equal(expected);
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