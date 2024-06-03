using System.ComponentModel;

namespace Toolkits.Core;

/// <summary>
/// a class of <see cref="IUsingDisposable{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IUsingDisposable<T>
{
    /// <summary>
    /// Begins the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public IUsingDisposable<T> Begin(T value);

    /// <summary>
    /// Ends the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public IUsingDisposable<T> End(T value);

    /// <summary>
    /// Usings the specified action.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public IShadowDisposable Using(Action<T> action);
}

/// <summary>
/// a interface of <see cref="IShadowDisposable"/>
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IShadowDisposable : IDisposable
{
    /// <summary>
    /// Uses the shadow.
    /// </summary>
    /// <returns></returns>
    IDisposable Shadow();
}

/// <summary>
/// a class of <see cref="ShadowDisposable"/>
/// </summary>
/// <seealso cref="Toolkits.Core.IShadowDisposable" />
public class ShadowDisposable : IShadowDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    IUseShadowDisposable useShadowDisposable;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShadowDisposable"/> class.
    /// </summary>
    /// <param name="useShadowDisposable">The use shadow disposable.</param>
    internal ShadowDisposable(IUseShadowDisposable useShadowDisposable)
    {
        this.useShadowDisposable = useShadowDisposable;
    }

    /// <summary>
    /// Dispose。
    /// </summary>
    public void Dispose()
    {
        useShadowDisposable?.Dispose();
        useShadowDisposable = null!;
    }

    /// <summary>
    /// Uses the shadow.
    /// </summary>
    /// <returns></returns>
    public IDisposable Shadow()
    {
        useShadowDisposable.OnUseShadow();
        return this;
    }
}

internal interface IUseShadowDisposable : IDisposable
{
    void OnUseShadow();
}

[EditorBrowsable(EditorBrowsableState.Never)]
internal class UsingDisposable<T> : IUsingDisposable<T>, IUseShadowDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    T beginValue = default!;

    [EditorBrowsable(EditorBrowsableState.Never)]
    T endValue = default!;

    [EditorBrowsable(EditorBrowsableState.Never)]
    Action<T> callback = default!;

    public UsingDisposable() { }

    public IUsingDisposable<T> Begin(T beginValue)
    {
        this.beginValue = beginValue;
        return this;
    }

    public IUsingDisposable<T> End(T endValue)
    {
        this.endValue = endValue;
        return this;
    }

    public IShadowDisposable Using(Action<T> callback)
    {
        this.callback = callback ?? throw new ArgumentNullException(nameof(callback));

        return new ShadowDisposable(this);
    }

    public void Dispose()
    {
        this.callback?.Invoke(endValue);

        this.callback = null!;
    }

    public void OnUseShadow()
    {
        this.callback?.Invoke(beginValue);
    }
}
