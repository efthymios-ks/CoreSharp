using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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
        public void Flatten_ExceptionIsNull_ReturnEmptyEnumerable()
        {
            //Arrange
            Exception exception = null;

            //Act
            Action action = () => exception.Flatten();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Flatten_WhenCalled_ReturnAllExceptionMessages()
        {
            //Arrange
            var level3Exception = new Exception("Level 3");
            var level2Exception = new AggregateException("Level 2", level3Exception);
            var level1Exception = new Exception("Level 1", level2Exception);
            var excepted = new[] { level1Exception, level2Exception, level3Exception };

            //Act
            var result = level1Exception.Flatten();

            //Assert 
            result.Should().Equal(excepted);
        }

        [Test]
        public void GetInnermostException_ExceptionIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Exception exception = null;

            //Act
            Action action = () => exception.GetInnermostException();

            //Assert 
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void GetInnermostException_ExceptionHasNoInnerValue_ReturnItself()
        {
            //Arrange
            const string expectedMessage = "1";
            var exception = new Exception(expectedMessage);

            //Act
            var result = exception.GetInnermostException();

            //Assert 
            result.Message.Should().Be(expectedMessage);
        }

        [Test]
        public void GetInnermostException_ExceptionHasInnerValue_ReturnItself()
        {
            //Arrange
            const string expectedMessage = "1";
            var ex1 = new Exception(expectedMessage);
            var ex2 = new Exception("2", ex1);
            var ex3_1 = new KeyNotFoundException("3.1", ex2);
            var ex3_2 = new InvalidDataException("3.2");
            var ex4 = new AggregateException(ex3_1, ex3_2);

            //Act
            var result = ex4.GetInnermostException();

            //Assert 
            result.Message.Should().Be(expectedMessage);
        }
    }
}
