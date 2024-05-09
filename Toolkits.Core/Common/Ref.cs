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
    /// Performs an implicit conversion from <see cref="Ref{T}"/> to  <paramref name="refObject"/>.
    /// </summary>
    /// <param name="refObject">The refObject.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator T(Ref<T> refObject)
    {
        _ = refObject ?? throw new ArgumentNullException(nameof(refObject));
        return refObject.Value;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="T"/> to <see cref="Ref{T}"/>.
    /// </summary>
    /// <param name="targetValue">The target value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    /// <exception cref="ArgumentNullException">targetValue</exception>
    public static implicit operator Ref<T>(T targetValue)
    {
        _ = targetValue ?? throw new ArgumentNullException(nameof(targetValue));

        return new Ref<T> { value = targetValue };
    }
}
