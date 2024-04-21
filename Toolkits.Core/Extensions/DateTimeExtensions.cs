using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the minute begin.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetMinuteBegin(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
    }

    /// <summary>
    /// Gets the minute end.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetMinuteEnd(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0)
            .AddMinutes(1)
            .AddMilliseconds(-1);
    }

    /// <summary>
    /// Gets the hour begin.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetHourBegin(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
    }

    /// <summary>
    /// Gets the hour end.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetHourEnd(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0).AddHours(1).AddMilliseconds(-1);
    }

    /// <summary>
    /// Gets the day begin.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetDayBegin(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day);
    }

    /// <summary>
    /// Gets the day end.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetDayEnd(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, d.Day).AddDays(1).AddMilliseconds(-1);
    }

    /// <summary>
    /// Gets the month begin.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetMonthBegin(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, 1, 0, 0, 0);
    }

    /// <summary>
    /// Gets the month end.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetMonthEnd(this DateTime d)
    {
        return new DateTime(d.Year, d.Month, 1, 0, 0, 0).AddMonths(1).AddMilliseconds(-1);
    }

    /// <summary>
    /// Gets the year begin.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetYearBegin(this DateTime d)
    {
        return new DateTime(d.Year, 1, 1, 0, 0, 0);
    }

    /// <summary>
    /// Gets the year end.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <returns></returns>
    public static DateTime GetYearEnd(this DateTime d)
    {
        return new DateTime(d.Year, 1, 1, 0, 0, 0).AddYears(1).AddMilliseconds(-1);
    }
}

/// <summary>
///
/// </summary>
/// 2024/1/29 14:00
public static class TimeMeasureExtensions
{
    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static void TimeMeasure(Action invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static T TimeMeasure<T>(this Func<T> invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            return invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static async Task TimeMeasureAsync(Func<Task> invoker, Action<int> timerCallback)
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();

        try
        {
            await invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// a stopwatch is used to obtain the execution time of a program
    /// </summary>
    /// <param name="invoker"></param>
    /// <param name="timerCallback"></param>
    public static async Task<T> TimeMeasureAsync<T>(
        Func<Task<T>> invoker,
        Action<int> timerCallback
    )
    {
        _ = timerCallback ?? throw new ArgumentNullException(nameof(timerCallback));
        _ = invoker ?? throw new ArgumentNullException(nameof(invoker));

        Stopwatch stop = Stopwatch.StartNew();
        try
        {
            return await invoker.Invoke();
        }
        finally
        {
            stop.Stop();
            timerCallback.Invoke((int)stop.ElapsedMilliseconds);
        }
    }
}
