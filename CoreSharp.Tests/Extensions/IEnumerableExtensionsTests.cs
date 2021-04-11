using CoreSharp.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        //Fields
        private readonly IEnumerable<DummyClass> sourceNull = null;
        private readonly IEnumerable<DummyClass> sourceEmpty = Enumerable.Empty<DummyClass>();

        //Methods 
        [Test]
        public void IsEmpty_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.IsEmpty();

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void IsEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = sourceEmpty.IsEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsNull_ReturnTrue()
        {
            //Act 
            var result = sourceNull.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void IsNullOrEmpty_SourceIsEmpty_ReturnTrue()
        {
            //Act 
            var result = sourceEmpty.IsNullOrEmpty();

            //Assert 
            result.Should().BeTrue();
        }

        [Test]
        public void NullToEmpty_SourceIsNull_ReturnEmpty()
        {
            //Act
            var result = sourceNull.NullToEmpty();

            //Assert
            result.Should().Equal(sourceEmpty);
        }

        [Test]
        public void ConvertTo_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.ConvertTo<string>();

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ConvertTo_WhenCalled_ReturnConvertedValues()
        {
            //Arrange
            var source = new int[]
            {
                1,
                2,
                3
            };
            var expected = new double[]
            {
                1,
                2,
                3
            };

            //Act 
            var result = source.ConvertTo<double>();

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void Exclude_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => sourceNull.Exclude(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Exclude_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Predicate<DummyClass> filter = null;
            Action action = () => sourceEmpty.Exclude(filter);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Exclude_WhenCalled_ReturnSourceExceptFilteredItems()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item2, item3 };

            //Act  
            var result = source.Exclude(i => i.Id == 1);

            //Assert
            result.Should().Equal(expected);
        }

        [Test]
        public void DistinctBy_SourceIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceNull.DistinctBy<DummyClass, int>(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DistinctBy_FilterIsNull_ThrowArgumentNullException()
        {
            //Act  
            Action action = () => sourceEmpty.DistinctBy<DummyClass, int>(null);

            //Assert 
            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void DistinctBy_WhenCalled_ReturnDistinceByGivenKey()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(1);
            var item3 = new DummyClass(2);
            var source = new[] { item1, item2, item3 };
            var expected = new[] { item1, item3 };

            //Act 
            var result = source.DistinctBy(i => i.Id);

            //Assert
            result.Should().Equal(expected);
        }

        public void ExceptTest()
        {
            //Arrange
            var item1 = new DummyClass(1);
            var item2 = new DummyClass(2);
            var item3 = new DummyClass(3);
            var left = new[] { item1, item2, item3 };
            var right = new[] { item1, item2 };
            var expected = new[] { item3 };

            //Act 
            var result = left.ExceptBy(right, i => i.Id);

            //Assert
            result.Should().Equal(expected);
        }

        //Nested
        private class DummyClass
        {
            //Constructors 
            public DummyClass(int id)
            {
                Id = id;
            }

            //Properties 
            public int Id { get; set; }

            public override string ToString()
            {
                return $"{Id}";
            }
        }
    }
}