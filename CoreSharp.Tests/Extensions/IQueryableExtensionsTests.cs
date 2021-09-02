using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IQueryableExtensionsTests
    {
        //Fields
        private readonly IQueryable<DummyClass> _sourceNull = null;
        private readonly IQueryable<DummyClass> _sourceEmpty = Enumerable.Empty<DummyClass>().AsQueryable();

        //Methods
        [Test]
        public void QueryPage_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            int pageIndex = 0;
            int pageSize = 0;

            //Act 
            Action action = () => _sourceNull.QueryPage(pageIndex, pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        [TestCase(5, -1, 1)]
        [TestCase(5, 1, -1)]
        public void QueryPage_PageArgsIsOutOfRange_ThrowArgumentNullException(int itemCount, int pageIndex, int pageSize)
        {
            //Arrange
            var source = new int[itemCount].AsQueryable();

            //Act 
            Action action = () => source.QueryPage(pageIndex, pageSize);

            //Assert 
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Test]
        public void QueryPage_WhenCalled_ReturnItemPage()
        {
            //Arrange
            var source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();
            var pageIndex = 1;
            var pageSize = 2;
            var expected = new[] { 3, 4 }.AsQueryable();

            //Act 
            var result = source.QueryPage(pageIndex, pageSize);

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void FilterFlexible_SourceIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceNull.FilterFlexible(i => i.Name, "a");

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FilterFlexible_PropertySelectorIsNull_ThrowArgumentNullException()
        {
            //Act 
            Action action = () => _sourceEmpty.FilterFlexible(null, "a");

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FilterFlexible_FilterIsNullOrEmpty_ReturnEmptyQueryable()
        {
            //Arrange
            var source = new[] { "a", "b" }.AsQueryable();
            var expected = Enumerable.Empty<string>().AsQueryable();

            //Act 
            var result = source.FilterFlexible(i => i, "");

            //Assert 
            result.Should().Equal(expected);
        }

        [Test]
        public void FilterFlexible_WhenCalled_ReturnFilteredItems()
        {
            //Arrange 
            var source = new[] { "a", "b", "ab", ".a.b.", " A B " }.AsQueryable();
            var expected = new[] { "ab", ".a.b.", " A B " }.AsQueryable();
            var filter = "ab";

            //Act 
            var result = source.FilterFlexible(i => i, filter);

            //Assert 
            result.Should().Equal(expected);
        }
    }
}