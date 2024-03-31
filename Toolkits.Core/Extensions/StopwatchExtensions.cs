using System.Diagnostics;

namespace Toolkits;

/// <summary>
///
/// </summary>
public static class StopwatchExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="stopwatch"></param>
    /// <param name="action"></param>
    /// <param name="stopwatchRestart"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TimeSpan Measure(
        this Stopwatch stopwatch,
        Action action,
        bool stopwatchRestart = true
    )
    {
        _ = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        if (stopwatchRestart)
        {
            stopwatch.Reset();
            stopwatch.Restart();
        }

        try
        {
            action();
        }
        finally
        {
            stopwatch.Stop();
        }

        return stopwatch.Elapsed;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stopwatch"></param>
    /// <param name="action"></param>
    /// <param name="stopwatchRestart"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static (T result, TimeSpan TimeSpan) Measure<T>(
        this Stopwatch stopwatch,
        Func<T> action,
        bool stopwatchRestart = true
    )
    {
        _ = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        if (stopwatchRestart)
        {
            stopwatch.Reset();
            stopwatch.Restart();
        }

        try
        {
            return (action(), stopwatch.Elapsed);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stopwatch"></param>
    /// <param name="action"></param>
    /// <param name="stopwatchRestart"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<(T result, TimeSpan timeSpan)> MeasureAsync<T>(
        this Stopwatch stopwatch,
        Func<Task<T>> action,
        bool stopwatchRestart = true
    )
    {
        _ = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        if (stopwatchRestart)
        {
            stopwatch.Reset();
            stopwatch.Restart();
        }

        try
        {
            return (await action(), stopwatch.Elapsed);
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stopwatch"></param>
    /// <param name="action"></param>
    /// <param name="stopwatchRestart"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<TimeSpan> MeasureAsync(
        this Stopwatch stopwatch,
        Func<Task> action,
        bool stopwatchRestart = true
    )
    {
        _ = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _ = action ?? throw new ArgumentNullException(nameof(action));

        if (stopwatchRestart)
        {
            stopwatch.Reset();
            stopwatch.Restart();
        }

        try
        {
            await action();

            return (stopwatch.Elapsed);
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}
