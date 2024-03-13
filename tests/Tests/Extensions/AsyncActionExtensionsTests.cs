using CoreSharp.Delegates;
using System.Threading;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class AsyncActionExtensionsTests
{
    [Test]
    public async Task InvokeAsync_WhenHandlerIsNull_ShouldNotThrowException()
    {
        // Arrange
        AsyncAction<object> handler = null;
        var args = new object();

        // Act 
        Func<Task> func = () => handler.InvokeAsync(args);

        // Assert
        await func.Should().NotThrowAsync();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasSingleDelegate_ShouldInvokeDelegate()
    {
        // Arrange
        var invoked = false;
        AsyncAction<object> handler = (_, cancellationToken) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        // Act
        await handler.InvokeAsync(null);

        // Assert
        invoked.Should().BeTrue();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasMultipleDelegates_ShouldInvokeAllDelegates()
    {
        // Arrange
        var invoked = new List<int>();
        AsyncAction<int> handler = null;
        handler += (_, cancellationToken) =>
        {
            invoked.Add(1);
            return Task.CompletedTask;
        };
        handler += (arg, cancellationToken) =>
        {
            invoked.Add(2);
            return Task.CompletedTask;
        };

        // Act
        await handler.InvokeAsync(default);

        // Assert
        invoked.Count.Should().Be(2);
        invoked.Should().ContainInOrder(1, 2);
    }

    [Test]
    public async Task InvokeAsync_WhenCancellationRequested_ShouldCancelInvocation()
    {
        // Arrange
        AsyncAction<object> handler = (_, cancellationToken)
            => Task.Delay(1000, cancellationToken);
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(1);

        // Act & Assert
        Func<Task> func = () => handler.InvokeAsync(null, cancellationTokenSource.Token);

        await func.Should().ThrowExactlyAsync<TaskCanceledException>();
    }
}
