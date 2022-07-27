namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class TaskExtensionsTests
{
    //Methods
    private static Task<TValue> GetValueAsync<TValue>(TValue value)
        => Task.FromResult(value);

    private static Task GetExceptionAsync(Exception exception)
        => Task.FromException(exception);

    [Test]
    public async Task WithAggregateException_WhenAllSuccessfull_ReturnResponses()
    {
        //Arrange
        const int value1 = 1;
        const int value2 = 2;
        var task1 = GetValueAsync(value1);
        var task2 = GetValueAsync(value2);

        //Act
        await Task.WhenAll(task1, task2).WithAggregateException();

        //Assert 
        task1.Result.Should().Be(value1);
        task2.Result.Should().Be(value2);
    }

    [Test]
    public async Task WithAggregateException_WhenExceptionsOccure_ReturnAggregateException()
    {
        //Arrange 
        var exception1 = new ArgumentException("1");
        var exception2 = new ArgumentException("2");
        var task1 = GetExceptionAsync(exception1);
        var task2 = GetExceptionAsync(exception2);

        //Act
        Func<Task> action = async () => await Task.WhenAll(task1, task2).WithAggregateException();

        //Assert 
        var assertion = await action.Should().ThrowExactlyAsync<AggregateException>();
        var aggregateException = assertion.Which;
        aggregateException.InnerExceptions.Count.Should().Be(2);
        aggregateException.InnerExceptions.Should().Contain(exception1);
        aggregateException.InnerExceptions.Should().Contain(exception2);
    }

    [Test]
    public async Task IgnoreError_TaskIsNull_ThrowArgumentNullException()
    {
        //Arrange
        Task task = null;

        //Act
        Func<Task> action = () => task.IgnoreError();

        //Assert 
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    public async Task IgnoreError_NoExceptionTypeProvided_IgnoreAllExceptions()
    {
        //Arrange 
        var task = Task.FromException(new InvalidCastException());

        //Act
        Func<Task> action = () => task.IgnoreError();

        //Assert 
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task IgnoreError_ExceptionTypeProvidedAndThrowsSame_IgnoreException()
    {
        //Arrange 
        var task = Task.FromException(new InvalidCastException());

        //Act
        Func<Task> action = () => task.IgnoreError<InvalidCastException>();

        //Assert 
        await action.Should().NotThrowAsync<InvalidCastException>();
    }

    [Test]
    public async Task IgnoreError_ExceptionTypeProvidedAndThrowsDifferent_ThrowException()
    {
        //Arrange 
        var exceptionToThrow = new InvalidCastException();
        var task = Task.FromException(exceptionToThrow);

        //Act
        Func<Task> action = () => task.IgnoreError<InvalidOperationException>();

        //Assert 
        var assertion = await action.Should().ThrowExactlyAsync<InvalidCastException>();
        var exceptionCaught = assertion.Which;
        exceptionCaught.Should().BeSameAs(exceptionToThrow);
    }

    [Test]
    public async Task IgnoreError_WithResult_TaskIsNull_ThrowArgumentNullException()
    {
        //Arrange
        Task<int> task = null;

        //Act
        Func<Task> action = () => task.IgnoreError();

        //Assert 
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    public async Task IgnoreError_WithResult_NoExceptionTypeProvided_IgnoreAllExceptions()
    {
        //Arrange 
        var task = Task.FromException<int>(new InvalidCastException());

        //Act
        Func<Task> action = () => task.IgnoreError();

        //Assert 
        await action.Should().NotThrowAsync();
    }

    [Test]
    public async Task IgnoreError_WithResult_ExceptionTypeProvidedAndThrowsSame_IgnoreException()
    {
        //Arrange 
        var task = Task.FromException<int>(new InvalidCastException());

        //Act
        Func<Task> action = () => task.IgnoreError<InvalidCastException>();

        //Assert 
        await action.Should().NotThrowAsync<InvalidCastException>();
    }

    [Test]
    public async Task IgnoreError_WithResult_ExceptionTypeProvidedAndThrowsDifferent_ThrowException()
    {
        //Arrange 
        var exceptionToThrow = new InvalidCastException();
        var task = Task.FromException<int>(exceptionToThrow);

        //Act
        Func<Task> action = () => task.IgnoreError<InvalidOperationException>();

        //Assert 
        var assertion = await action.Should().ThrowExactlyAsync<InvalidCastException>();
        var exceptionCaught = assertion.Which;
        exceptionCaught.Should().BeSameAs(exceptionToThrow);
    }

    [Test]
    public async Task IgnoreError_WithResult_NoExceptionIsThrown_GetResult()
    {
        //Arrange 
        const int expected = 1;
        var task = Task.FromResult(expected);

        //Act
        Func<Task<int>> action = () => task.IgnoreError();

        //Assert 
        var assertion = await action.Should().NotThrowAsync();
        var result = assertion.Which;
        result.Should().Be(expected);
    }
}
