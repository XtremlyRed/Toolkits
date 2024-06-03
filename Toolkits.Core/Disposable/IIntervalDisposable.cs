using System;
using System.ComponentModel;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="interface"/> of <see cref="IIntervalDisposable"/>
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IIntervalDisposable : IDisposable
{
    /// <summary>
    /// Intervals the specified time span.
    /// </summary>
    /// <param name="timeSpan">The time span.</param>
    /// <returns></returns>
    public IIntervalDisposable Interval(TimeSpan timeSpan);

    /// <summary>
    /// Counters the specified counter.
    /// </summary>
    /// <param name="counter">The counter.</param>
    /// <returns></returns>
    public IIntervalDisposable Counter(int counter);

    /// <summary>
    /// Subscribes the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    public IDisposable Subscribe(Action<int> callback);

    /// <summary>
    /// Subscribes the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="errorCallback">The error callback.</param>
    /// <returns></returns>
    public IDisposable Subscribe(Action<int> callback, Action<Exception, IDisposable> errorCallback);

    /// <summary>
    /// Subscribes the specified callback.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <param name="errorCallback">The error callback.</param>
    /// <param name="completedCallback">The completed callback.</param>
    /// <returns></returns>
    public IDisposable Subscribe(Action<int> callback, Action<Exception, IDisposable> errorCallback, Action<IDisposable> completedCallback);
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Core.IIntervalDisposable" />
[EditorBrowsable(EditorBrowsableState.Never)]
public class IntervalDisposable : IIntervalDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Action<int> callback = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Action<Exception, IDisposable> errorCallback = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Action<IDisposable> completedCallback = default!;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int counter = int.MaxValue;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TimeSpan timeSpan = TimeSpan.FromSeconds(1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int currentCounter;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isDisposed;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Timer? timer;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int indexer;

    public IIntervalDisposable Counter(int counter)
    {
        if (counter <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(counter));
        }
        this.counter = counter;
        return this;
    }

    public IIntervalDisposable Interval(TimeSpan timeSpan)
    {
        if (timeSpan <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeSpan));
        }
        this.timeSpan = timeSpan;
        return this;
    }

    public IDisposable Subscribe(Action<int> callback)
    {
        this.callback = callback;
        this.errorCallback = default!;
        this.completedCallback = default!;

        timer = CreateTimer();

        return this;
    }

    public IDisposable Subscribe(Action<int> callback, Action<Exception, IDisposable> errorCallback)
    {
        this.callback = callback;
        this.errorCallback = errorCallback;
        this.completedCallback = default!;

        timer = CreateTimer();

        return this;
    }

    public IDisposable Subscribe(Action<int> callback, Action<Exception, IDisposable> errorCallback, Action<IDisposable> completedCallback)
    {
        this.callback = callback;
        this.errorCallback = errorCallback;
        this.completedCallback = completedCallback;

        timer = CreateTimer();

        return this;
    }

    private Timer CreateTimer()
    {
        timer = new Timer((int)timeSpan.TotalMilliseconds);
        timer.AutoReset = true;
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
        return timer;
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            callback?.Invoke(++indexer);

            Interlocked.Increment(ref currentCounter);

            if (currentCounter >= counter)
            {
                this.Dispose();
                completedCallback?.Invoke(this);
            }
        }
        catch (Exception ex)
        {
            errorCallback?.Invoke(ex, this);
        }
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        if (timer is not null)
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Dispose();
        }

        timer = null!;
        isDisposed = true;
    }
}
