using CoreSharp.Delegates;
using System.Threading;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class AsyncEventHandlerExtensionsTests
{
    [Test]
    public async Task InvokeAsync_WhenHandlerIsNull_ShouldNotThrowException()
    {
        // Arrange
        AsyncEventHandler<object> handler = null;
        var sender = new object();
        var args = new object();

        // Act 
        Func<Task> func = () => handler.InvokeAsync(sender, args);

        // Assert
        await func.Should().NotThrowAsync();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasSingleDelegate_ShouldInvokeDelegate()
    {
        // Arrange
        var invoked = false;
        AsyncEventHandler<object> handler = (_, args, _) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };
        var sender = new object();
        var args = new object();

        // Act
        await handler.InvokeAsync(sender, args);

        // Assert
        invoked.Should().BeTrue();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasMultipleDelegates_ShouldInvokeAllDelegates()
    {
        // Arrange
        var invoked = new List<int>();
        AsyncEventHandler<int> handler = null;
        handler += (_, args, _) =>
        {
            invoked.Add(args);
            return Task.CompletedTask;
        };
        handler += (_, args, _) =>
        {
            invoked.Add(args * 2);
            return Task.CompletedTask;
        };
        var sender = new object();
        var arg = 1;

        // Act
        await handler.InvokeAsync(sender, arg);

        // Assert
        invoked.Count.Should().Be(2);
        invoked.Should().ContainInOrder(1, 2);
    }

    [Test]
    public async Task InvokeAsync_WhenCancellationRequested_ShouldCancelInvocation()
    {
        // Arrange
        AsyncEventHandler<object> handler = (_, _, cancellationToken)
            => Task.Delay(1000, cancellationToken);
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(1);

        // Act & Assert
        Func<Task> func = () => handler.InvokeAsync(null, null, cancellationTokenSource.Token);

        await func.Should().ThrowExactlyAsync<TaskCanceledException>();
    }
}