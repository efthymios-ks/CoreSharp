using NUnit.Framework;
using System;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class ExceptionExtensionsTests
    {
        //Methods
        [Test]
        public void FlattenMessages_ExceptionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Exception exception = null;

            //Act
            Action action = () => exception.FlattenMessages();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void FlattenMessages_WhenCalled_ReturnAllExceptionMessages()
        {
            //Arrange
            var level3Exception = new Exception("Level 3");
            var level2Exception = new AggregateException("Level 2", level3Exception);
            var level1Exception = new Exception("Level 1", level2Exception);
            var messages = new[] { level1Exception.Message, level2Exception.Message, level3Exception.Message };
            var expected = string.Join(Environment.NewLine, messages);

            //Act
            var result = level1Exception.FlattenMessages();

            //Assert 
            result.Should().Be(expected);
        }

        [Test]
        public void GetExceptions_ExceptionUsNull_ReturnEmptyEnumerable()
        {
            //Arrange
            Exception exception = null;

            //Act
            Action action = () => exception.FlattenMessages();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetExceptions_WhenCalled_ReturnAllExceptionMessages()
        {
            //Arrange
            var level3Exception = new Exception("Level 3");
            var level2Exception = new AggregateException("Level 2", level3Exception);
            var level1Exception = new Exception("Level 1", level2Exception);
            var excepted = new[] { level1Exception, level2Exception, level3Exception };

            //Act
            var result = level1Exception.GetExceptions();

            //Assert 
            result.Should().Equal(excepted);
        }
    }
}