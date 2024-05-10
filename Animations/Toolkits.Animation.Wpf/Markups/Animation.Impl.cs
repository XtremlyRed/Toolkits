using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="Animation"/>
/// </summary>
/// <seealso cref="System.Windows.Freezable" />
public abstract partial class Animation : Freezable
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
        typeof(Animation),
        new PropertyMetadata(EventMode.Loaded)
    );

    /// <summary>
    /// the play.
    /// </summary>
    public bool? Play
    {
        get { return (bool?)GetValue(PlayProperty); }
        set { SetValue(PlayProperty, value); }
    }

    /// <summary>
    ///   play property
    /// </summary>
    public static readonly DependencyProperty PlayProperty = DependencyProperty.Register(
        "Play",
        typeof(bool?),
        typeof(Animation),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (e.NewValue is not true || s is not Animation animation)
                {
                    return;
                }

                animation.AnimationPlay();
            },
            null,
            true,
            System.Windows.Data.UpdateSourceTrigger.Explicit
        )
    );

    protected virtual void AnimationPlay()
    {
        AnimationExtensions.GetAnimationInfo(this).Invoke();
        this.SetCurrentValue(PlayProperty, false);
    }
}

/// <summary>
/// a class of <see cref="AnimationGeneric{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Windows.Freezable" />
public abstract class AnimationGeneric<T> : AnimationBase
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
        typeof(AnimationGeneric<T>),
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
        typeof(AnimationGeneric<T>),
        new PropertyMetadata(null)
    );
}
