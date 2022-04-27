using CoreSharp.Tests.Dummies;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class LambdaExpressionExtensionsTests
    {
        [Test]
        public void GetMemberName_ExpressionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Expression<Func<DummyClass, int>> expression = null;

            //Act 
            Action action = () => expression.GetMemberName();

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetMemberName_WhenCalled_ReturnMemberName()
        {
            //Arrange
            Expression<Func<DummyClass, int>> expression = d => d.Id;
            const string expected = nameof(DummyClass.Id);

            //Act 
            var result = expression.GetMemberName();

            //Assert
            result.Should().Be(expected);
        }
    }
}
