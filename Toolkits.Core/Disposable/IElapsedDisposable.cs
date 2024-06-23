using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="interface"/> of <see cref="IElapsedDisposable"/>
/// </summary>
public interface IElapsedDisposable : IDisposable { }

/// <summary>
/// a <see langword="class"/> of <see cref="ElapsedDisposable"/>
/// </summary>
internal class ElapsedDisposable : IElapsedDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Stopwatch stopwatch;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Action<TimeSpan> callback;

    /// <summary>
    ///
    /// </summary>
    /// <param name="callback"></param>
    public ElapsedDisposable(Action<TimeSpan> callback)
    {
        this.callback = callback;
        stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// dispose
    /// </summary>
    public void Dispose()
    {
        if (callback is not null)
        {
            stopwatch.Stop();
            callback?.Invoke(stopwatch.Elapsed);
        }
        stopwatch?.Stop();
        stopwatch = null!;
        callback = null!;
    }
}
