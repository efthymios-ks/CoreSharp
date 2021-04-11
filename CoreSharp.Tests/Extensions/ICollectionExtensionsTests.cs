﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
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
            action.Should().Throw<ArgumentNullException>();
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
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AddRange_WhenCalled_AddItemsToCollection()
        {
            //Arrange
            ICollection<int> source = new Collection<int>();
            IEnumerable<int> items = new[] { 1, 2, 3, 4 };
            int expectedCount = source.Count() + items.Count();

            //Act
            source.AddRange(items);

            //Assert
            source.Should().HaveCount(expectedCount);
            source.Should().EndWith(items);
        }
    }
}