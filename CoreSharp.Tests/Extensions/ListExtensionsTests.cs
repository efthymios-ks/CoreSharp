using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ListExtensionsTests
    {
        //Fields
        private readonly List<DummyClass> _sourceNull = null;
        private readonly List<DummyClass> _sourceEmpty = new();

        //Methods 
        [Test]
        public void Sort_SourceIsNull_ThrowArgumentNullException()
        {
            //Act
            Action action = () => _sourceNull.Sort(i => i.Id);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Sort_KeySelectorIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Func<DummyClass, int> keySelector = null;

            //Act
            Action action = () => _sourceEmpty.Sort(keySelector);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Sort_WhenCalled_SortByGivenKey()
        {
            //Arrange
            var source = new List<DummyClass>
            {
                new DummyClass(3),
                new DummyClass(1),
                new DummyClass(2)
            };
            var expected = source.ToList().OrderBy(i => i.Id);

            //Act
            source.Sort(i => i.Id);

            //Assert
            source.Should().Equal(expected);
        }
    }
}