using System;
using System.Linq.Expressions;
using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class LambaExpressionExtensionsTests
    {
        [Test]
        public void GetMemberName_ExpressionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Expression<Func<DummyClass, int>> exression = null;

            //Act 
            Action action = () => exression.GetMemberName();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetMemberName_WhenCalled_ReturnMemberName()
        {
            //Arrange
            Expression<Func<DummyClass, int>> exression = d => d.Id;
            string expected = nameof(DummyClass.Id);

            //Act 
            var result = exression.GetMemberName();

            //Assert
            result.Should().Be(expected);
        }
    }
}