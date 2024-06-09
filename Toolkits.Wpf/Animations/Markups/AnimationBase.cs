using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="AnimationBase"/>
/// </summary>
public abstract partial class AnimationBase
{
    /// <summary>
    /// animation begin time
    /// </summary>
    public virtual TimeSpan BeginTime
    {
        get { return (TimeSpan)GetValue(BeginTimeProperty); }
        set { SetValue(BeginTimeProperty, value); }
    }

    /// <summary>
    /// animation begin time
    /// </summary>
    public static readonly DependencyProperty BeginTimeProperty = DependencyProperty.Register(
        "BeginTime",
        typeof(TimeSpan),
        typeof(AnimationBase),
        new PropertyMetadata(TimeSpan.Zero)
    );

    /// <summary>
    ///  duration time.
    /// </summary>
    public virtual Duration Duration
    {
        get { return (Duration)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    /// <summary>
    ///  duration time.
    /// </summary>
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        "Duration",
        typeof(Duration),
        typeof(AnimationBase),
        new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1)))
    );

    /// <summary>
    /// the type of the animation easing.
    /// </summary>
    public virtual EasingType EasingType
    {
        get { return (EasingType)GetValue(EasingTypeProperty); }
        set { SetValue(EasingTypeProperty, value); }
    }

    /// <summary>
    /// the type of the animation easing.
    /// </summary>
    public static readonly DependencyProperty EasingTypeProperty = DependencyProperty.Register(
        "EasingType",
        typeof(EasingType),
        typeof(AnimationBase),
        new PropertyMetadata(EasingType.None)
    );

    /// <summary>
    /// animation easing mode.
    /// </summary>
    public virtual EasingMode EasingMode
    {
        get { return (EasingMode)GetValue(EasingModeProperty); }
        set { SetValue(EasingModeProperty, value); }
    }

    /// <summary>
    /// animation easing mode.
    /// </summary>
    public static readonly DependencyProperty EasingModeProperty = DependencyProperty.Register(
        "EasingMode",
        typeof(EasingMode),
        typeof(AnimationBase),
        new PropertyMetadata(EasingMode.EaseOut)
    );

    /// <summary>
    /// Gets the easing function.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    /// <returns></returns>
    protected IEasingFunction? GetEasingFunction(FrameworkElement frameworkElement)
    {
        EasingFunctionBase? easingFunctionBase = EasingType switch
        {
            EasingType.None => null,
            EasingType.Back => new BackEase(),
            EasingType.Bounce => new BounceEase(),
            EasingType.Circle => new CircleEase(),
            EasingType.Cubic => new CubicEase(),
            EasingType.Elastic => new ElasticEase(),
            EasingType.Exponential => new ElasticEase(),
            EasingType.Quadratic => new QuadraticEase(),
            EasingType.Quartic => new QuarticEase(),
            EasingType.Quintic => new QuinticEase(),
            EasingType.Sine => new SineEase(),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (easingFunctionBase is not null)
            easingFunctionBase.EasingMode = EasingMode;

        return easingFunctionBase;
    }

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal AnimationTimeline CreateAnimation(FrameworkElement element, out object propertyOwner)
    {
        var animation = AnimationBuild(element, out propertyOwner);

        const string propertyName = nameof(DoubleAnimation.EasingFunction);

        if (animation?.GetType()?.GetProperty(propertyName, Instance | Public | NonPublic) is PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetValue(animation) is null)
            {
                propertyInfo.SetValue(animation, GetEasingFunction(element));
            }
        }

        return animation!;
    }

    /// <summary>
    /// Animations the builder.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected virtual AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = default!;
        return default!;
    }
}
