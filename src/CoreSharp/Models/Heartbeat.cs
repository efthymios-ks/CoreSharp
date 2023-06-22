using System;
using System.Timers;

namespace CoreSharp.Models;

public sealed class Heartbeat : IDisposable
{
    // Fields
    private readonly object _lockObject = new();
    private readonly Timer _timer = new();
    private bool _isHeartbeatReceived;
    private bool _isDisposed;

    public Heartbeat(TimeSpan heartbeatInterval)
    {
        _timer.AutoReset = true;
        _timer.Interval = heartbeatInterval.TotalMicroseconds;
        _timer.Elapsed += TimerElapsedCallback;
    }

    private bool IsRunning
        => _timer?.Enabled ?? false;

    public Action Tick { get; set; }
    public Action Expired { get; set; }

    public void Start()
    {
        lock (_lockObject)
        {
            if (IsRunning)
            {
                return;
            }

            _timer.Start();
        }
    }

    public void Stop()
    {
        lock (_lockObject)
        {
            if (!IsRunning)
            {
                return;
            }

            _timer.Stop();
        }
    }

    public void Reset()
    {
        lock (_lockObject)
        {
            _isHeartbeatReceived = true;
        }
    }

    private void TimerElapsedCallback(object sender, ElapsedEventArgs e)
    {
        lock (_lockObject)
        {
            if (!IsRunning)
            {
                return;
            }

            Tick?.Invoke();

            if (_isHeartbeatReceived)
            {
                return;
            }

            Expired?.Invoke();
            _isHeartbeatReceived = false;
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        Stop();
        _timer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
