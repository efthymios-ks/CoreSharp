using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="HttpClient"/> extensions.
/// </summary>
public static class HttpClientExtensions
{
    /// <inheritdoc cref="SendAsync(HttpClient, HttpRequestMessage, HttpCompletionOption, TimeSpan)"/>
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        TimeSpan timeout)
        => await httpClient.SendAsync(httpRequestMessage, default, timeout);

    /// <inheritdoc cref="SendAsync(HttpClient, HttpRequestMessage, HttpCompletionOption, TimeSpan, CancellationToken)"/>
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        HttpCompletionOption httpCompletionOption,
        TimeSpan timeout)
        => await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, timeout, default);

    /// <inheritdoc cref="SendAsync(HttpClient, HttpRequestMessage, HttpCompletionOption, TimeSpan, CancellationToken)"/>
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        TimeSpan timeout,
        CancellationToken cancellationToken)
        => await httpClient.SendAsync(httpRequestMessage, default, timeout, cancellationToken);

    /// <summary>
    /// Send an HTTP request as an asynchronous operation
    /// with given <see cref="TimeSpan"/> timeout.
    /// </summary>
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        HttpCompletionOption httpCompletionOption,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

        using var cancellationTokenSource = cancellationToken.ToTimeoutCancellationTokenSource(timeout);
        try
        {
            return await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationTokenSource.Token);
        }
        // Cancelled by user 
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException($"The request was cancelled from user-provided `{nameof(CancellationToken)}`.");
        }
        // HttpClient.Timeout 
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new TimeoutException(message: ex.Message, innerException: ex);
        }
        // IRequest.Timeout 
        catch (TaskCanceledException ex) when (ex.InnerException is IOException)
        {
            throw new TimeoutException(message: $"The request was cancelled due to the configured timeout of `{timeout.ToStringReadable()}` elapsing.", innerException: ex);
        }
        // Timeout on really small TimeSpans 
        catch (TaskCanceledException)
        {
            throw new TaskCanceledException("The request was cancelled.");
        }
    }
}
