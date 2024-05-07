using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toolkits.Animation;

/// <summary>
/// a class of <see cref="AnimationDeclareBase"/>
/// </summary>
/// <seealso cref="System.Windows.Freezable" />
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class AnimationDeclareBase : Animatable
{
    /// <summary>
    /// </summary>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override Freezable? CreateInstanceCore()
    {
        return Activator.CreateInstance(this.GetType()) as Freezable;
    }

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal abstract AnimationTimeline CreateAnimation(FrameworkElement element, out object propertyOwner);

    /// <summary>
    ///  event mode.
    /// </summary>
    public virtual EventMode EventMode
    {
        get { return (EventMode)GetValue(EventModeProperty); }
        set { SetValue(EventModeProperty, value); }
    }

    /// <summary>
    ///  event mode.
    /// </summary>
    public static readonly DependencyProperty EventModeProperty = DependencyProperty.Register(
        "EventMode",
        typeof(EventMode),
        typeof(AnimationDeclareBase),
        new PropertyMetadata(EventMode.Loaded)
    );
}

/// <summary>
/// a class of <see cref="AnimationDeclareGeneric{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Windows.Freezable" />
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class AnimationDeclareGeneric<T> : AnimationDeclare
{
    /// <summary>
    /// to value
    /// </summary>
    public T? To
    {
        get { return (T?)GetValue(ToProperty); }
        set { SetValue(ToProperty, value); }
    }

    /// <summary>
    /// to value
    /// </summary>
    public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
        "To",
        typeof(T?),
        typeof(AnimationDeclareGeneric<T>),
        new PropertyMetadata(null)
    );

    /// <summary>
    ///  from value.
    /// </summary>
    public T? From
    {
        get { return (T?)GetValue(FromProperty); }
        set { SetValue(FromProperty, value); }
    }

    /// <summary>
    ///  from value.
    /// </summary>
    public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
        "From",
        typeof(T?),
        typeof(AnimationDeclareGeneric<T>),
        new PropertyMetadata(null)
    );
}
