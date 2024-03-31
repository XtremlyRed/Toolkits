using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits;

/// <summary>
///
/// </summary>
/// <seealso cref="System.IDisposable" />
[DebuggerDisplay("{currentCounter}")]
public class AsyncLocker : IDisposable
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
        semaphoreSlim = new SemaphoreSlim(initialCount, maxCount);
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
    /// Gets a value indicating whether this instance is idle.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is idle; otherwise, <c>false</c>.
    /// </value>
    public bool IsIdle => currentCounter == 0;

    /// <summary>
    /// Releases this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public void ReleaseAll()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        lock (syncRoot)
        {
            int count = currentCounter;

            while (count > 0)
            {
                count = Interlocked.Decrement(ref currentCounter);
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
    /// Waits the specified milliseconds timeout.
    /// </summary>
    /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public bool Wait(int millisecondsTimeout)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return semaphoreSlim.Wait(millisecondsTimeout);
    }

    /// <summary>
    /// Waits the specified milliseconds timeout.
    /// </summary>
    /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return semaphoreSlim.Wait(millisecondsTimeout, cancellationToken);
    }

    /// <summary>
    /// Waits the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public bool Wait(TimeSpan timeout)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return semaphoreSlim.Wait(timeout);
    }

    /// <summary>
    /// Waits the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return semaphoreSlim.Wait(timeout, cancellationToken);
    }

    /// <summary>
    /// Waits the specified cancellation token.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public void Wait(CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        semaphoreSlim.Wait(cancellationToken);
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
    /// Waits the asynchronous.
    /// </summary>
    /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task<bool> WaitAsync(int millisecondsTimeout)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return await semaphoreSlim.WaitAsync(millisecondsTimeout);
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken);
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task WaitAsync(CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        await semaphoreSlim.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task<bool> WaitAsync(TimeSpan timeout)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return await semaphoreSlim.WaitAsync(timeout);
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ObjectDisposedException">AsyncLocker</exception>
    public async Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(AsyncLocker)) : 0;

        Interlocked.Increment(ref currentCounter);
        return await semaphoreSlim.WaitAsync(timeout, cancellationToken);
    }

    /// <summary>
    /// dispose
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Dispose()
    {
        if (isDisposabled)
        {
            return;
        }

        isDisposabled = true;

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
