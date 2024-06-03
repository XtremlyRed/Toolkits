using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
///  a class of <see cref="Disposable"/>
/// </summary>
public static class Disposable
{
    /// <summary>
    /// Intervals the specified time span.
    /// </summary>
    /// <param name="timeSpan">The time span.</param>
    /// <returns></returns>
    public static IIntervalDisposable Interval(TimeSpan timeSpan)
    {
        var dispose = new IntervalDisposable();
        dispose.Interval(timeSpan);
        return dispose;
    }

    /// <summary>
    /// Uses the specified begin value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="beginValue">The begin value.</param>
    /// <param name="endValue">The end value.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    public static IShadowDisposable Use<T>(T beginValue, T endValue, Action<T> callback)
    {
        var use = new UsingDisposable<T>();
        use.Begin(beginValue);
        use.End(endValue);
        use.Using(callback);
        return new ShadowDisposable(use);
    }

    /// <summary>
    /// Uses the true.
    /// </summary>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    public static IShadowDisposable Use(bool beginValue, Action<bool> callback)
    {
        var use = new UsingDisposable<bool>();
        use.Begin(beginValue);
        use.End(!beginValue);
        use.Using(callback);

        return new ShadowDisposable(use);
    }
}
