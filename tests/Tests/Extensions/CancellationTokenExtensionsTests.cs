using System.Threading;

namespace CoreSharp.Extensions.Tests;

[TestFixture]
public sealed class YourClassTests
{
    [Test]
    public void ToTimeoutCancellationTokenSource_WhenTimeoutIsZero_ShouldNotCancelAfterTimeout()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var timeout = TimeSpan.Zero;

        // Act
        var linkedTokenSource = cancellationToken.ToTimeoutCancellationTokenSource(timeout);

        // Assert
        linkedTokenSource.Token.IsCancellationRequested.Should().BeFalse();
    }

    [Test]
    public void ToTimeoutCancellationTokenSource_WhenTimeoutIsInfinite_ShouldNotCancelAfterTimeout()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var timeout = Timeout.InfiniteTimeSpan;

        // Act
        var linkedTokenSource = cancellationToken.ToTimeoutCancellationTokenSource(timeout);

        // Assert
        linkedTokenSource.Token.IsCancellationRequested.Should().BeFalse();
    }
}
