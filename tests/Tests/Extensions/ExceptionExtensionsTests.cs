using System.IO;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class ExceptionExtensionsTests
{
    // Methods
    [Test]
    public void UnwrapMessages_ExceptionIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Exception exception = null;

        // Act
        Action action = () => exception.UnwrapMessages();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void UnwrapMessages_WhenCalled_ReturnAllExceptionMessages()
    {
        // Arrange
        var level3Exception = new Exception("Level 3");
        var level2Exception = new AggregateException("Level 2", level3Exception);
        var level1Exception = new Exception("Level 1", level2Exception);
        var messages = new[] { level1Exception.Message, level2Exception.Message, level3Exception.Message };
        var expected = string.Join(Environment.NewLine, messages);

        // Act
        var result = level1Exception.UnwrapMessages();

        // Assert 
        result.Should().Be(expected);
    }

    [Test]
    public void Unwrap_ExceptionIsNull_ReturnEmptyEnumerable()
    {
        // Arrange
        Exception exception = null;

        // Act
        Action action = () => exception.Unwrap();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Unwrap_WhenCalled_ReturnAllExceptionMessages()
    {
        // Arrange
        var ex3 = new Exception("Level 3");
        var ex2 = new AggregateException("Level 2", ex3);
        var ex1 = new Exception("Level 1", ex2);
        var expected = new[] { ex1, ex2, ex3 };

        // Act
        var result = ex1.Unwrap();

        // Assert 
        result.Should().Equal(expected);
    }

    [Test]
    public void GetInnermostException_ExceptionIsNull_ThrowArgumentNullException()
    {
        // Arrange
        Exception exception = null;

        // Act
        Action action = () => exception.GetInnermostException();

        // Assert 
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void GetInnermostException_ExceptionHasNoInnerValue_ReturnItself()
    {
        // Arrange 
        var exception = new Exception();

        // Act
        var result = exception.GetInnermostException();

        // Assert 
        result.Should().BeSameAs(exception);
    }

    [Test]
    public void GetInnermostException_ExceptionHasInnerValue_ReturnItself()
    {
        // Arrange 
        var ex1 = new Exception("1");
        var ex2 = new Exception("2", ex1);
        var ex3_1 = new KeyNotFoundException("3.1", ex2);
        var ex3_2 = new InvalidDataException("3.2");
        var ex4 = new AggregateException(ex3_1, ex3_2);

        // Act
        var result = ex4.GetInnermostException();

        // Assert 
        result.Should().BeSameAs(ex1);
    }
}
