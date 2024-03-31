using System.Diagnostics;

namespace Toolkits;

/// <summary>
///
/// </summary>
[DebuggerDisplay("{value}")]
public sealed class Ref<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int locker;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private T? value;

    /// <summary>
    /// current value.
    /// </summary>
    public T Value
    {
        get
        {
            while (Interlocked.CompareExchange(ref locker, 0, 0) != 0) { }
            return value!;
        }
    }

    /// <summary>
    /// Swaps the specified new value.
    /// </summary>
    /// <param name="newValue">The new value.</param>
    public void Swap(T newValue)
    {
        try
        {
            while (Interlocked.CompareExchange(ref locker, 1, 0) != 0) { }
            value = newValue;
        }
        finally
        {
            Interlocked.Exchange(ref locker, 0);
        }
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="Ref{T}"/> to  <paramref name="r"/>.
    /// </summary>
    /// <param name="r">The r.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator T(Ref<T> r)
    {
        return r.Value;
    }
}
