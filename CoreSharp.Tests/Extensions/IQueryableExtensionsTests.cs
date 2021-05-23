using System;
using System.Linq;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IQueryableExtensionsTests
    {
        //Fields
        private readonly IQueryable<DummyClass> sourceNull = null;

        //Methods
        [Test]
        public void QueryPage_SourceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            int pageIndex = 0;
            int pageSize = 0;

            //Act 
            Action action = () => sourceNull.QueryPage(pageIndex, pageSize);

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
    }
}