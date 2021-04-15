using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class IListExtensionsTests
    {
        //Fields
        private readonly IList<int> sourceNull = null;

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
    }
}