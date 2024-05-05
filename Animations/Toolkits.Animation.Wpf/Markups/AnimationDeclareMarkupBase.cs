using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Toolkits.Animation;

namespace Toolkits.Animation;

public abstract class AnimationDeclareMarkupBase : AnimationDeclareBaseMarkupExtension
{
    /// <summary>
    /// animation from value.
    /// </summary>
    public double? From { get; set; }

    /// <summary>
    ///  animation to value.
    /// </summary>
    public double? To { get; set; }

    /// <summary>
    /// the type of the animation easing.
    /// </summary>
    public EasingType EasingType { get; set; } = EasingType.Cubic;

    /// <summary>
    /// animation easing mode.
    /// </summary>
    public EasingMode EasingMode { get; set; }

    /// <summary>
    ///  animation property.
    /// </summary>
    protected virtual DependencyProperty AnimationProperty { get; } = default!;

    /// <summary>
    ///  animation path.
    /// </summary>
    protected abstract string AnimationPath { get; }

    /// <summary>
    /// Gets the easing function.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    /// <returns></returns>
    protected IEasingFunction GetEasingFunction(FrameworkElement frameworkElement)
    {
        EasingFunctionBase easingFunctionBase = EasingType switch
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

        easingFunctionBase.EasingMode = EasingMode;

        return easingFunctionBase;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget? service =
            serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (service?.TargetObject is not FrameworkElement element)
        {
            return null!;
        }

        ApplyTransformGroup(element);

        var animation = CreateAnimation(element, null!, out var owner);

        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, new PropertyPath(AnimationPath));

        AnimationExtensions
            .GetAnimations(element)
            .Add(
                new AnimationInfo_2()
                {
                    EventMode = this.EventMode,
                    Animation = animation,
                    ElementRef = new WeakReference(owner),
                    Property = AnimationProperty!
                }
            );

        AnimationExtensions.RegisterEvent(element, EventMode);

        return this;
    }

    protected sealed override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property
    )
    {
        throw new NotImplementedException();
    }

    protected virtual AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property,
        out object owner
    )
    {
        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(element)
        };

        if (this.From.HasValue)
        {
            animation.From = this.From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = this.To.Value;
        }

        owner = element;

        return animation;
    }

    #region Transform

    protected const int ScaleIndex = 0;
    protected const int RotateIndex = 1;
    protected const int TranslateIndex = 2;

    private void ApplyTransformGroup(FrameworkElement frameworkElement)
    {
        frameworkElement.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

        Transform? exist = null;

        if (frameworkElement.RenderTransform is not TransformGroup transformGroup)
        {
            exist = frameworkElement.RenderTransform;
            frameworkElement.RenderTransform = transformGroup = new TransformGroup();
        }

        List<Transform> exists = new List<Transform>();

        if (
            transformGroup.Children.Count == 0
            || transformGroup.Children[ScaleIndex] is not ScaleTransform
        )
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(ScaleIndex)!);

            Add(
                ScaleIndex,
                (exist as ScaleTransform)
                    ?? exists.FirstOrDefault(i => i is not null and ScaleTransform)
                    ?? new ScaleTransform(1, 1)
            );
        }

        if (
            transformGroup.Children.Count < 2
            || transformGroup.Children[RotateIndex] is not RotateTransform
        )
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(RotateIndex)!);
            Add(
                RotateIndex,
                (exist as RotateTransform)
                    ?? exists.FirstOrDefault(i => i is not null and RotateTransform)
                    ?? new RotateTransform(0)
            );
        }

        if (
            transformGroup.Children.Count < 3
            || transformGroup.Children[TranslateIndex] is not TranslateTransform
        )
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(TranslateIndex)!);
            Add(
                TranslateIndex,
                (exist as TranslateTransform)
                    ?? exists.FirstOrDefault(i => i is not null and TranslateTransform)
                    ?? new TranslateTransform(0, 0)
            );
        }

        exists.Add(exist!);

        exists
            .Where(i => i != null)
            .Where(i => i is not ScaleTransform)
            .Where(i => i is not RotateTransform)
            .Where(i => i is not TranslateTransform)
            .ForEach(transformGroup.Children.Add);

        void Add(int index, Transform transform)
        {
            if (transformGroup.Children.Count > index)
            {
                transformGroup.Children[index] = transform;
            }
            else
            {
                transformGroup.Children.Add(transform);
            }
        }
    }

    #endregion
}

internal class AnimationInfo_2 : IAnimationInfo
{
    private MethodInfo Method = default!;
    private PropertyInfo PropertyInfo = default!;

    public AnimationTimeline Animation = default!;
    public WeakReference ElementRef = default!;
    public DependencyProperty Property = default!;
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
        if (ElementRef.Target is null)
        {
            return;
        }

        //object currentValue = element.GetValue(Property);

        //PropertyInfo? property = PropertyInfo ??= Animation.GetType().GetProperty("From")!;

        //property?.SetValue(Animation, currentValue);

        Method ??= ElementRef
            .Target?.GetType()
            .GetMethod(nameof(UIElement.BeginAnimation), paramterTypes)!;

        Method?.Invoke(ElementRef.Target, new object[] { Property, Animation });
    }
}

public class FadeDeclareExtension : AnimationDeclareMarkupBase
{
    protected override DependencyProperty AnimationProperty => FrameworkElement.OpacityProperty;
    protected override string AnimationPath => "(FrameworkElement.Opacity)";
}

public class RotateDeclareExtension : AnimationDeclareMarkupBase
{
    protected override DependencyProperty AnimationProperty => RotateTransform.AngleProperty;
    protected override string AnimationPath =>
        string.Format(
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(RotateTransform.Angle)",
            RotateIndex
        );

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement element,
        DependencyProperty property,
        out object owner
    )
    {
        owner = ((TransformGroup)element.RenderTransform).Children[RotateIndex];

        return base.CreateAnimation(element, property, out _);
    }
}

public class SlideXDeclareExtension : AnimationDeclareMarkupBase
{
    public enum SlideXMode
    {
        Left,
        Right
    }

    public SlideXMode SlideMode { get; set; }

    protected override DependencyProperty AnimationProperty => TranslateTransform.XProperty;
    protected override string AnimationPath =>
        string.Format(
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.X)",
            TranslateIndex
        );

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement targetObject,
        DependencyProperty property,
        out object owner
    )
    {
        owner = ((TransformGroup)targetObject.RenderTransform).Children[TranslateIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        int dir = SlideMode == SlideXMode.Left ? -1 : 1;

        animation.From = (From!.HasValue ? From.Value : targetObject.ActualWidth) * dir;

        animation.To = (To!.HasValue ? To.Value : 0);

        return animation;
    }
}

public class SlideYDeclareExtension : AnimationDeclareMarkupBase
{
    public enum SlideYMode
    {
        Top,
        Bottom
    }

    public SlideYMode SlideMode { get; set; }

    protected override DependencyProperty AnimationProperty => TranslateTransform.YProperty;
    protected override string AnimationPath =>
        string.Format(
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.Y)",
            TranslateIndex
        );

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement targetObject,
        DependencyProperty property,
        out object owner
    )
    {
        owner = ((TransformGroup)targetObject.RenderTransform).Children[TranslateIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        int dir = SlideMode == SlideYMode.Top ? -1 : 1;

        animation.From = (From!.HasValue ? From.Value : targetObject.ActualHeight) * dir;

        animation.To = (To!.HasValue ? To.Value : 0);

        return animation;
    }
}

public class ScaleXDeclareExtension : AnimationDeclareMarkupBase
{
    protected override DependencyProperty AnimationProperty => ScaleTransform.ScaleXProperty;
    protected override string AnimationPath =>
        string.Format(
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleX)",
            ScaleIndex
        );

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement targetObject,
        DependencyProperty property,
        out object owner
    )
    {
        owner = ((TransformGroup)targetObject.RenderTransform).Children[ScaleIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        if (this.From.HasValue)
        {
            animation.From = From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = To.Value;
        }

        return animation;
    }
}

public class ScaleYDeclareExtension : AnimationDeclareMarkupBase
{
    protected override DependencyProperty AnimationProperty => ScaleTransform.ScaleYProperty;
    protected override string AnimationPath =>
        string.Format(
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleY)",
            ScaleIndex
        );

    protected override AnimationTimeline CreateAnimation(
        FrameworkElement targetObject,
        DependencyProperty property,
        out object owner
    )
    {
        owner = ((TransformGroup)targetObject.RenderTransform).Children[ScaleIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        if (this.From.HasValue)
        {
            animation.From = From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = To.Value;
        }

        return animation;
    }
}
