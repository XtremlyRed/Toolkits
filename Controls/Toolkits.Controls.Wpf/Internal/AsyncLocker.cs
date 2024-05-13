using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Wpf.Internal;

/// <summary>
///
/// </summary>
/// <seealso cref="System.IDisposable" />
[EditorBrowsable(EditorBrowsableState.Never)]
internal class AsyncLocker : IDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private object syncRoot = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private SemaphoreSlim semaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int currentCounter = 0;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isDisposabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncLocker"/> class.
    /// </summary>
    /// <param name="initialCount">The initial count.</param>
    /// <param name="maxCount">The maximum count.</param>
    public AsyncLocker(int initialCount, int maxCount)
    {
        semaphoreSlim = new(initialCount, maxCount);
    }

    /// <summary>
    /// Releases this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public void Release()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        lock (syncRoot)
        {
            if (currentCounter > 0)
            {
                Interlocked.Decrement(ref currentCounter);
                semaphoreSlim.Release();
            }
        }
    }

    /// <summary>
    /// Waits this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public void Wait()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        semaphoreSlim.Wait();
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task WaitAsync()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        await semaphoreSlim.WaitAsync();
    }

    /// <summary>
    /// dispose
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Dispose()
    {
        if (isDisposabled == false)
        {
            isDisposabled = true;

            Dispose(true);
        }
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (currentCounter != 0)
            {
                semaphoreSlim?.Release(currentCounter);
            }

            semaphoreSlim?.Dispose();
            semaphoreSlim = null!;
            syncRoot = null!;
            currentCounter = 0;
        }
    }
}
