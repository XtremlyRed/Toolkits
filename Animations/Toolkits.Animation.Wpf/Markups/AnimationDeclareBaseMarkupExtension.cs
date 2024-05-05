using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Toolkits.Animation;

/// <summary>
///
/// </summary>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class AnimationDeclareBaseMarkupExtension : MarkupExtension
{
    /// <summary>
    /// animation begin time
    /// </summary>
    public TimeSpan BeginTime { get; set; } = TimeSpan.Zero;

    /// <summary>
    ///  duration time.
    /// </summary>
    public Duration Duration { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///  event mode.
    /// </summary>
    /// <value>
    public EventMode EventMode { get; set; } = EventMode.Loaded;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget? service =
            serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (
            service?.TargetProperty is not DependencyProperty property
            || service?.TargetObject is not FrameworkElement element
        )
        {
            return null!;
        }

        object currentValue = element.GetValue(property);

        AnimationTimeline animation = CreateAnimation(element, property!);

        animation.BeginTime = BeginTime;

        Collection<IAnimationInfo> collection = AnimationExtensions.GetAnimations(element);

        collection.Add(
            new AnimationInfo_1()
            {
                EventMode = EventMode,
                Animation = animation,
                Property = property!,
                ElementRef = new WeakReference(element)
            }
        );

        AnimationExtensions.RegisterEvent(element, EventMode);

        return null!;
    }

    /// <summary>
    /// creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="property">The property.</param>
    /// <returns></returns>
    protected abstract AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    );
}

/// <summary>
///
/// </summary>
internal class AnimationInfo_1 : IAnimationInfo
{
    public MethodInfo Method = default!;
    public PropertyInfo PropertyInfo = default!;
    public AnimationTimeline Animation = default!;
    public DependencyProperty Property = default!;
    public WeakReference ElementRef = default!;
    public EventMode EventMode { get; set; }

    /// <summary>
    /// The paramter types
    /// </summary>
    private static readonly Type[] paramterTypes = new Type[]
    {
        typeof(DependencyProperty),
        typeof(AnimationTimeline)
    };

    public void Invoke()
    {
        if (ElementRef.Target is not FrameworkElement element)
        {
            return;
        }

        object currentValue = element.GetValue(Property);

        PropertyInfo? property = PropertyInfo ??= Animation.GetType().GetProperty("From")!;

        property?.SetValue(Animation, currentValue);

        Method ??= ElementRef
            .Target?.GetType()
            .GetMethod(nameof(UIElement.BeginAnimation), paramterTypes)!;

        Method?.Invoke(ElementRef.Target, new object[] { Property, Animation });
    }
}

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
public abstract class AnimationDeclareBaseMarkupExtension<T> : AnimationDeclareBaseMarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget? service =
            serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (service?.TargetObject is ICollection<AnimationDeclareBaseMarkupExtension<T>>)
        {
            return this;
        }

        if (base.ProvideValue(serviceProvider) is object @obj)
        {
            return @obj!;
        }

        return From!;
    }

    public virtual T From { get; set; } = default!;

    public virtual T To { get; set; } = default!;
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Color&gt;" />
public class BrushAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Color>
{
    public BrushAnimationDeclareExtension()
    {
        From = Colors.Transparent;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget? service =
            serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (
            service?.TargetProperty is not DependencyProperty property
            || service?.TargetObject is not FrameworkElement element
        )
        {
            return null!;
        }

        SolidColorBrush brush = new SolidColorBrush(From);

        AnimationTimeline animation = CreateAnimation(element, SolidColorBrush.ColorProperty);

        Collection<IAnimationInfo> collection = AnimationExtensions.GetAnimations(element);

        collection.Add(
            new AnimationInfo_1()
            {
                EventMode = EventMode,
                Animation = animation,
                Property = SolidColorBrush.ColorProperty!,
                ElementRef = new WeakReference(brush)
            }
        );

        AnimationExtensions.RegisterEvent(element, EventMode);

        return brush;
    }

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        ColorAnimation animation = element.BuildAnimation(
            property!.Name,
            From,
            To,
            TimeSpan.Zero,
            Duration.TimeSpan
        );

        return animation;
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Thickness&gt;" />
public class ThicknessAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Thickness>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Int32&gt;" />
public class Int32AnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<int>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Double&gt;" />
public class DoubleAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<double>
{
    public EasingType EasingType { get; set; } = EasingType.Cubic;

    public EasingMode EasingMode { get; set; } = EasingMode.EaseOut;

    protected EasingFunctionBase? GetFunctionBase()
    {
        EasingFunctionBase @base = EasingType switch
        {
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

        @base.EasingMode = EasingMode;

        return @base;
    }

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement frameworkElement,
        DependencyProperty dependencyProperty
    )
    {
        return frameworkElement.BuildAnimation(
            dependencyProperty!.Name,
            From,
            To,
            TimeSpan.Zero,
            Duration.TimeSpan,
            GetFunctionBase()
        );
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Color&gt;" />
public class ColorAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Color>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Point&gt;" />
public class PointAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Point>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Media3D.Point3D&gt;" />
public class Point3DAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Point3D>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Media3D.Quaternion&gt;" />
public class QuaternionAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Quaternion>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Media3D.Rotation3D&gt;" />
public class Rotation3DAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Rotation3D>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Vector&gt;" />
public class VectorAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Vector>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Media.Media3D.Vector3D&gt;" />
public class Vector3DAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Vector3D>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationBase&lt;System.Windows.Size&gt;" />
public class SizeAnimationDeclareExtension : AnimationDeclareBaseMarkupExtension<Size>
{
    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        return element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}
