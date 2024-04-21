using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
[DebuggerDisplay("{value}")]
public sealed class Ref<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private T? value;

    /// <summary>
    /// current value.
    /// </summary>
    public T Value
    {
        get
        {
            lock (this)
            {
                return value!;
            }
        }
    }

    /// <summary>
    /// Swaps the specified new value.
    /// </summary>
    /// <param name="newValue">The new value.</param>
    public void Swap(T newValue)
    {
        lock (this)
        {
            value = newValue;
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
