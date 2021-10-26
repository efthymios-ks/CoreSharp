using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ICollectionExtensionsTests
    {
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
            ICollection<int> source = new Collection<int> { 1, 2, 3 };
            var items = new[] { 4, 5, 6 };
            var expected = new Collection<int> { 1, 2, 3, 4, 5, 6 };

            //Act
            source.AddRange(items);

            //Assert
            source.Should().Equal(expected);
        }
    }
}
