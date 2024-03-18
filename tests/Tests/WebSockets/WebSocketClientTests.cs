using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using System.Net.WebSockets;
using System.Threading;

namespace CoreSharp.WebSockets.Tests;

[TestFixture]
public class WebSocketClientTests
{
    private const int HostPort = 8080;
    private const int ContainerPort = 3000;
    private readonly static Uri _serverUri = new($"ws://localhost:{HostPort}");
    private static WebSocketClient _client;
    private static IContainer _serverContainer;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        _serverContainer = new ContainerBuilder()
            .WithImage("elegantmonkeys/websockets-demo:latest")
            .WithPortBinding(HostPort, ContainerPort)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(ContainerPort))
            .Build();

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        await _serverContainer.StartAsync(cancellationTokenSource.Token);
    }

    [SetUp]
    public async Task SetUpAsync()
    {
        _client = new();
        if (_serverContainer.State != TestcontainersStates.Running)
        {
            await _serverContainer.StartAsync();
        }
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _client = null;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        if (_serverContainer is not null)
        {
            await _serverContainer.StopAsync();
            await _serverContainer.DisposeAsync();
            _serverContainer = null;
        }
    }

    [Test]
    public async Task ConnectAsync_WhenCalled_ShouldConnectToWebSocketServer()
    {
        // Act
        Func<Task> action = async () => await _client.ConnectAsync(_serverUri);

        // Assert
        await action.Should().NotThrowAsync<WebSocketException>();
        _client.IsConnected.Should().BeTrue();
    }

    [Test]
    public async Task ConnectAsync_WhenCancellationTokenIsCancelled_ShouldThrowTaskCancelledException()
    {
        // Arrange 
        var cancellationToken = new CancellationToken(canceled: true);

        // Act
        Func<Task> action = async () => await _client.ConnectAsync(_serverUri, cancellationToken);

        // Assert
        await action.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    public async Task CloseAsync_WhenCalled_ShouldCloseWebSocketConnection()
    {
        // Act
        await _client.ConnectAsync(_serverUri);
        var afterConnectState = _client.IsConnected;
        await _client.CloseAsync(WebSocketCloseStatus.NormalClosure);
        var afterCloseState = _client.IsConnected;

        // 
        afterConnectState.Should().BeTrue();
        afterCloseState.Should().BeFalse();
    }

    [Test]
    public async Task SendAsync_WhenBufferIsNull_ShouldThrowArgumentNullException()
    {
        // Act  
        Func<Task> sendTask = () => _client.SendAsync(buffer: null);

        // Assert 
        await sendTask.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Test]
    public async Task SendAsync_WhenBufferLengthIsLessThatOne_ShouldThrowArgumentArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = Array.Empty<byte>();
        var batchSize = 0;

        // Act  
        Func<Task> sendTask = () => _client.SendAsync(buffer, batchSize);

        // Assert 
        await sendTask.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task SendAsync_WhenCalled_ShouldSendMessageAndRaiseMessageSentEvent()
    {
        // Arrange 
        const string messageToSent = "Hello";
        var bufferToSend = Encoding.UTF8.GetBytes(messageToSent);
        string messageSent = null;
        var wait = new AutoResetEvent(false);

        // Act
        _client.MessageSent += buffer =>
        {
            messageSent = Encoding.UTF8.GetString(buffer);
            wait.Set();
        };

        await _client.ConnectAsync(_serverUri);
        await _client.SendAsync(bufferToSend);

        // Assert 
        var signaled = wait.WaitOne(millisecondsTimeout: 100);
        signaled.Should().BeTrue();
        messageSent.Should().Be(messageToSent);
    }

    [Test]
    public async Task SendAsync_WhenCancellationTokenIsCancelled_ShouldThrowTaskCancelledException()
    {
        // Arrange 
        const string message = "Hello";
        var buffer = Encoding.UTF8.GetBytes(message);
        var cancellationToken = new CancellationToken(canceled: true);

        // Act
        await _client.ConnectAsync(_serverUri);
        Func<Task> sendTask = () => _client.SendAsync(buffer, cancellationToken: cancellationToken);

        // Assert 
        await sendTask.Should().ThrowExactlyAsync<TaskCanceledException>();
    }

    [Test]
    public void StartListening_WhenBufferLengthIsLessThatOne_ShouldThrowArgumentArgumentOutOfRangeException()
    {
        // Act
        Action actiom = () => _client.StartListening(batchSize: 0);

        // Assert
        actiom.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task StartListening_WhenCalled_ShouldRaiseMessageReceivedEvent()
    {
        // Arrange 
        const string messageToSent = "Hello";
        var bufferToSend = Encoding.UTF8.GetBytes(messageToSent);
        string messageReceived = null;
        var wait = new AutoResetEvent(false);

        // Act
        _client.MessageReceived += buffer =>
        {
            messageReceived = Encoding.UTF8.GetString(buffer);
            wait.Set();
        };

        await _client.ConnectAsync(_serverUri);
        await _client.SendAsync(bufferToSend);
        _client.StartListening();

        // Assert 
        var signaled = wait.WaitOne(millisecondsTimeout: 100);
        signaled.Should().BeTrue();
        messageReceived.Should().Be(messageToSent);
    }

    [Test]
    public async Task StartListening_WhenCancellationTokenIsCancelled_ShouldRaiseListenerErrorOccuredWithTaskCancelledException()
    {
        // Arrange 
        var wait = new AutoResetEvent(false);
        Exception raisedError = null;

        // Act
        _client.ListenerErrorOccured += error =>
        {
            raisedError = error;
            wait.Set();
        };

        await _client.ConnectAsync(_serverUri);
        _client.StartListening();
        await _serverContainer.StopAsync();

        // Assert
        var signaled = wait.WaitOne(millisecondsTimeout: 100);
        signaled.Should().BeTrue();
        var webSocketError = raisedError.Should().BeOfType<WebSocketException>().Subject;
        webSocketError.WebSocketErrorCode.Should().Be(WebSocketError.ConnectionClosedPrematurely);
    }

    [Test]
    public async Task StartListening_WhenErrorOccures_ShouldRaiseMessageListenerErrorOccured()
    {
        // Arrange
        const string message = "Received";
        var buffer = Encoding.UTF8.GetBytes(message);
        var cancellationToken = new CancellationToken(canceled: true);
        var wait = new AutoResetEvent(false);
        Exception raisedError = null;

        // Act
        _client.ListenerErrorOccured += error =>
        {
            raisedError = error;
            wait.Set();
        };

        await _client.ConnectAsync(_serverUri);
        await _client.SendAsync(buffer);
        _client.StartListening(cancellationToken: cancellationToken);

        // Assert
        var signaled = wait.WaitOne(millisecondsTimeout: 100);
        signaled.Should().BeTrue();
        raisedError.Should().BeOfType<TaskCanceledException>();
    }

    [Test]
    public async Task Dispose_WhenNotAlreadyDisposed_ShouldDisposeInnerSocket()
    {
        // Act
        await _client.ConnectAsync(_serverUri);
        var afterConnectState = _client.IsConnected;
        Action action = _client.Dispose;

        // Assert
        action.Should().NotThrow();
        afterConnectState.Should().BeTrue();
        _client.IsConnected.Should().BeFalse();
    }

    [Test]
    public async Task Dispose_WhenAlreadyDisposed_ShouldNotThrowException()
    {
        // Act
        await _client.ConnectAsync(_serverUri);
        Action action = () =>
        {
            _client.Dispose();
#pragma warning disable S3966 // Objects should not be disposed more than once
            _client.Dispose();
#pragma warning restore S3966 // Objects should not be disposed more than once
        };

        // Assert
        action.Should().NotThrow();
        _client.IsConnected.Should().BeFalse();
    }
}