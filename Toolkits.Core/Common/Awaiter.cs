using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
/// <seealso cref="System.IDisposable" />
[DebuggerDisplay("[ count : {currentCounter} ]  [ max : {maxCount} ]")]
public class Awaiter : IDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private SemaphoreSlim semaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int currentCounter;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isDisposabled;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int maxCount;

    /// <summary>
    ///  counter.
    /// </summary>
    public int Counter => currentCounter;

    /// <summary>
    /// Initializes a new instance of the <see cref="Awaiter"/> class.
    /// </summary>
    /// <param name="initialCount">The initial count.</param>
    /// <param name="maxCount">The maximum count.</param>
    public Awaiter(int initialCount, int maxCount = int.MaxValue)
    {
        _ = (initialCount < 0 || initialCount > maxCount) ? throw new ArgumentOutOfRangeException(nameof(initialCount)) : 0;

        semaphoreSlim = new(initialCount, maxCount);
        currentCounter = initialCount;
        this.maxCount = maxCount;
    }

    /// <summary>
    /// Releases this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">Awaiter</exception>
    public void Release()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(Awaiter)) : 0;

        if (currentCounter < maxCount)
        {
            Interlocked.Increment(ref currentCounter);
            semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// Releases this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">Awaiter</exception>
    public void ReleaseAll()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(Awaiter)) : 0;

        while (currentCounter < maxCount)
        {
            Interlocked.Increment(ref currentCounter);
            semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// Waits this instance.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">Awaiter</exception>
    public void Wait()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(Awaiter)) : 0;

        Interlocked.Decrement(ref currentCounter);
        semaphoreSlim.Wait();
    }

    /// <summary>
    /// Waits the asynchronous.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException">Awaiter</exception>
    public async Task WaitAsync()
    {
        _ = isDisposabled ? throw new ObjectDisposedException(nameof(Awaiter)) : 0;

        Interlocked.Decrement(ref currentCounter);
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

            if (currentCounter != 0)
            {
                semaphoreSlim?.Release(currentCounter);
            }

            semaphoreSlim?.Dispose();
            semaphoreSlim = null!;
            currentCounter = 0;
            maxCount = 0;
        }
    }
}

/// <summary>
///
/// </summary>
public static class AsyncLockerExtensions
{
    /// <summary>
    /// Locks the invoke.
    /// </summary>
    /// <param name="asyncLocker">The asynchronous locker.</param>
    /// <param name="action">The action.</param>
    public static void LockInvoke(this Awaiter asyncLocker, Action action)
    {
        asyncLocker.Wait();

        try
        {
            action();
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    /// <summary>
    /// Locks the invoke.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="asyncLocker">The asynchronous locker.</param>
    /// <param name="func">The function.</param>
    /// <returns></returns>
    public static T LockInvoke<T>(this Awaiter asyncLocker, Func<T> func)
    {
        asyncLocker.Wait();

        try
        {
            return func();
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    /// <summary>
    /// Locks the invoke asynchronous.
    /// </summary>
    /// <param name="asyncLocker">The asynchronous locker.</param>
    /// <param name="func">The function.</param>
    public static async Task LockInvokeAsync(this Awaiter asyncLocker, Func<Task> func)
    {
        await asyncLocker.WaitAsync();

        try
        {
            await func();
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    /// <summary>
    /// Locks the invoke asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="asyncLocker">The asynchronous locker.</param>
    /// <param name="func">The function.</param>
    /// <returns></returns>
    public static async Task<T> LockInvokeAsync<T>(this Awaiter asyncLocker, Func<Task<T>> func)
    {
        await asyncLocker.WaitAsync();

        try
        {
            return await func();
        }
        finally
        {
            asyncLocker.Release();
        }
    }
}
