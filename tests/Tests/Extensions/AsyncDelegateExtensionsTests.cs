using CoreSharp.Delegates;
using System.Threading;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public class AsyncDelegateExtensionsTests
{
    [Test]
    public async Task InvokeAsync_WhenHandlerIsNull_ShouldNotThrowException()
    {
        // Arrange
        AsyncDelegate handler = null;

        // Act 
        Func<Task> func = () => handler.InvokeAsync();

        // Assert
        await func.Should().NotThrowAsync();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasSingleDelegate_ShouldInvokeDelegate()
    {
        // Arrange
        var invoked = false;
        AsyncDelegate handler = (cancellationToken) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        // Act
        await handler.InvokeAsync();

        // Assert
        invoked.Should().BeTrue();
    }

    [Test]
    public async Task InvokeAsync_WhenHandlerHasMultipleDelegates_ShouldInvokeAllDelegates()
    {
        // Arrange
        var invoked = new List<int>();
        AsyncDelegate handler = null;
        handler += (cancellationToken) =>
        {
            invoked.Add(1);
            return Task.CompletedTask;
        };
        handler += (cancellationToken) =>
        {
            invoked.Add(2);
            return Task.CompletedTask;
        };

        // Act
        await handler.InvokeAsync();

        // Assert
        invoked.Count.Should().Be(2);
        invoked.Should().ContainInOrder(1, 2);
    }

    [Test]
    public async Task InvokeAsync_WhenCancellationRequested_ShouldCancelInvocation()
    {
        // Arrange
        AsyncDelegate handler = (cancellationToken)
            => Task.Delay(1000, cancellationToken);
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(1);

        // Act & Assert
        Func<Task> func = () => handler.InvokeAsync(cancellationTokenSource.Token);

        await func.Should().ThrowExactlyAsync<TaskCanceledException>();
    }
}