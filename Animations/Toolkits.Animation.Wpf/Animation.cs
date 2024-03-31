using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Toolkits.Animation;

public enum EventMode
{
    None,
    Loaded,
    Unloaded,
    MouseEnter,
    MouseLeave,
    DataContextChanged,
    GotFocus,
    LostFocus,
}

public enum EasingType
{
    Back,
    Bounce,
    Circle,
    Cubic,
    Elastic,
    Exponential,
    Quadratic,
    Quartic,
    Quintic,
    Sine
}

public enum CallMode
{
    Multi,
    Once
}

public static class Animation
{
    #region private


    private static Storyboard GetStoryboard(FrameworkElement obj)
    {
        var sb = obj.GetValue(StoryboardProperty) as Storyboard;

        if (sb is null)
        {
            sb = new Storyboard();
            sb.Completed += (s, e) =>
            {
                if (
                    s is Storyboard storyboard
                    && storyboard.GetValue(ComplateCommandProperty) is ICommand command
                    && command.CanExecute(null)
                )
                {
                    command.Execute(null);
                }
            };

            obj.SetValue(StoryboardProperty, sb);
        }

        return sb;
    }

    private static readonly DependencyProperty StoryboardProperty =
        DependencyProperty.RegisterAttached(
            "Storyboard",
            typeof(Storyboard),
            typeof(Animation),
            new PropertyMetadata(null)
        );

    private static EventManager GetEventManager(FrameworkElement obj)
    {
        return (EventManager)obj.GetValue(EventManagerProperty);
    }

    private static void SetEventManager(FrameworkElement obj, EventManager value)
    {
        obj.SetValue(EventManagerProperty, value);
    }

    private static readonly DependencyProperty EventManagerProperty =
        DependencyProperty.RegisterAttached(
            "EventManager",
            typeof(EventManager),
            typeof(Animation),
            new PropertyMetadata(null)
        );

    #endregion

    #region Play

    public static bool GetPlay(DependencyObject obj)
    {
        return (bool)obj.GetValue(PlayProperty);
    }

    public static void SetPlay(DependencyObject obj, bool value)
    {
        obj.SetValue(PlayProperty, value);
    }

    public static readonly DependencyProperty PlayProperty = DependencyProperty.RegisterAttached(
        "Play",
        typeof(bool),
        typeof(Animation),
        new PropertyMetadata(
            false,
            (s, e) =>
            {
                if (s is FrameworkElement element && e.NewValue is bool boolValue && boolValue)
                {
                    GetStoryboard(element)?.Begin();
                }
            }
        )
    );

    #endregion

    #region CallMode

    public static CallMode GetCallMode(DependencyObject obj)
    {
        return (CallMode)obj.GetValue(CallModeProperty);
    }

    public static void SetCallMode(DependencyObject obj, CallMode value)
    {
        obj.SetValue(CallModeProperty, value);
    }

    public static readonly DependencyProperty CallModeProperty =
        DependencyProperty.RegisterAttached(
            "CallMode",
            typeof(CallMode),
            typeof(Animation),
            new FrameworkPropertyMetadata(
                CallMode.Multi,
                (s, e) =>
                {
                    if (s is not FrameworkElement element || e.NewValue is not CallMode callMode)
                    {
                        return;
                    }
                    Register(element);
                }
            )
        );

    #endregion

    #region EventMode

    public static EventMode GetEventMode(DependencyObject obj)
    {
        return (EventMode)obj.GetValue(EventModeProperty);
    }

    public static void SetEventMode(DependencyObject obj, EventMode value)
    {
        obj.SetValue(EventModeProperty, value);
    }

    public static readonly DependencyProperty EventModeProperty =
        DependencyProperty.RegisterAttached(
            "EventMode",
            typeof(EventMode),
            typeof(Animation),
            new FrameworkPropertyMetadata(
                EventMode.Loaded,
                (s, e) =>
                {
                    if (s is FrameworkElement element && e.NewValue is EventMode)
                    {
                        Register(element);
                    }
                }
            )
        );

    #endregion


    #region Easing

    public static EasingType GetEasingType(FrameworkElement obj)
    {
        return (EasingType)obj.GetValue(EasingTypeProperty);
    }

    public static void SetEasingType(FrameworkElement obj, EasingType value)
    {
        obj.SetValue(EasingTypeProperty, value);
    }

    public static readonly DependencyProperty EasingTypeProperty =
        DependencyProperty.RegisterAttached(
            "EasingType",
            typeof(EasingType),
            typeof(Animation),
            new PropertyMetadata(
                EasingType.Quadratic,
                (s, e) =>
                {
                    if (s is not FrameworkElement element || e.NewValue is not EasingType)
                    {
                        return;
                    }

                    IEasingFunction eas = AnimationExtension.GetEasingFunction(element);

                    GetStoryboard(element)
                        ?.Children.ForEach(i =>
                        {
                            if (i is DoubleAnimation da)
                            {
                                da.EasingFunction = eas;
                            }
                        });
                }
            )
        );

    public static EasingMode GetEasingMode(DependencyObject obj)
    {
        return (EasingMode)obj.GetValue(EasingModeProperty);
    }

    public static void SetEasingMode(DependencyObject obj, EasingMode value)
    {
        obj.SetValue(EasingModeProperty, value);
    }

    public static readonly DependencyProperty EasingModeProperty =
        DependencyProperty.RegisterAttached(
            "EasingMode",
            typeof(EasingMode),
            typeof(Animation),
            new PropertyMetadata(
                EasingMode.EaseOut,
                (s, e) =>
                {
                    if (s is not FrameworkElement element || e.NewValue is not EasingMode)
                    {
                        return;
                    }

                    IEasingFunction eas = AnimationExtension.GetEasingFunction(element);

                    GetStoryboard(element)
                        ?.Children.ForEach(i =>
                        {
                            if (i is DoubleAnimation da)
                            {
                                da.EasingFunction = eas;
                            }
                        });
                }
            )
        );
    #endregion

    #region Register

    private static void RegisterStoryboard(
        object? elementObject,
        object? newObject,
        object? oldObject
    )
    {
        if (elementObject is not FrameworkElement element)
        {
            return;
        }

        Storyboard storyboard = GetStoryboard(element) ?? new Storyboard();

        if (oldObject is AnimationExtension oldExtension)
        {
            oldExtension.Unregister(storyboard, element);
        }

        if (newObject is not AnimationExtension newExtension)
        {
            return;
        }

        newExtension.Register(storyboard, element);

        Register(element, true);
    }

    private static void Register(FrameworkElement element, bool isExistReturn = false)
    {
        EventManager eventManager = GetEventManager(element);

        if (eventManager != null && isExistReturn)
        {
            return;
        }

        if (eventManager is null)
        {
            eventManager = new EventManager(element, static ele => GetStoryboard(ele)?.Begin());

            SetEventManager(element, eventManager);
        }

        EventMode eventMode = GetEventMode(element);

        eventManager.Unregister();

        if (eventMode == EventMode.None)
        {
            return;
        }

        eventManager.Register(eventMode, GetCallMode(element));
    }

    #endregion



    public static ICommand GetComplateCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(ComplateCommandProperty);
    }

    public static void SetComplateCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(ComplateCommandProperty, value);
    }

    public static readonly DependencyProperty ComplateCommandProperty =
        DependencyProperty.RegisterAttached(
            "ComplateCommand",
            typeof(ICommand),
            typeof(Animation),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    if (s is FrameworkElement element && e.NewValue is ICommand command)
                    {
                        GetStoryboard(element).SetValue(ComplateCommandProperty, command);
                    }
                }
            )
        );

    public static FadeFromExtension GetFadeFrom(FrameworkElement obj)
    {
        return (FadeFromExtension)obj.GetValue(FadeFromProperty);
    }

    public static void SetFadeFrom(FrameworkElement obj, FadeFromExtension value)
    {
        obj.SetValue(FadeFromProperty, value);
    }

    public static readonly DependencyProperty FadeFromProperty =
        DependencyProperty.RegisterAttached(
            "FadeFrom",
            typeof(FadeFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static FadeToExtension GetFadeTo(DependencyObject obj)
    {
        return (FadeToExtension)obj.GetValue(FadeToProperty);
    }

    public static void SetFadeTo(DependencyObject obj, FadeToExtension value)
    {
        obj.SetValue(FadeToProperty, value);
    }

    public static readonly DependencyProperty FadeToProperty = DependencyProperty.RegisterAttached(
        "FadeTo",
        typeof(FadeToExtension),
        typeof(Animation),
        new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
    );

    public static RotateFromExtension GetRotateFrom(FrameworkElement obj)
    {
        return (RotateFromExtension)obj.GetValue(RotateFromProperty);
    }

    public static void SetRotateFrom(FrameworkElement obj, RotateFromExtension value)
    {
        obj.SetValue(RotateFromProperty, value);
    }

    public static readonly DependencyProperty RotateFromProperty =
        DependencyProperty.RegisterAttached(
            "RotateFrom",
            typeof(RotateFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static RotateToExtension GetRotateTo(FrameworkElement obj)
    {
        return (RotateToExtension)obj.GetValue(RotateToProperty);
    }

    public static void SetRotateTo(FrameworkElement obj, RotateToExtension value)
    {
        obj.SetValue(RotateToProperty, value);
    }

    public static readonly DependencyProperty RotateToProperty =
        DependencyProperty.RegisterAttached(
            "RotateTo",
            typeof(RotateToExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static SlideXToExtension GetSlideXTo(FrameworkElement obj)
    {
        return (SlideXToExtension)obj.GetValue(SlideXToProperty);
    }

    public static void SetSlideXTo(FrameworkElement obj, SlideXToExtension value)
    {
        obj.SetValue(SlideXToProperty, value);
    }

    public static readonly DependencyProperty SlideXToProperty =
        DependencyProperty.RegisterAttached(
            "SlideXTo",
            typeof(SlideXToExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static SlideYToExtension GetSlideYTo(FrameworkElement obj)
    {
        return (SlideYToExtension)obj.GetValue(SlideYToProperty);
    }

    public static void SetSlideYTo(FrameworkElement obj, SlideYToExtension value)
    {
        obj.SetValue(SlideYToProperty, value);
    }

    public static readonly DependencyProperty SlideYToProperty =
        DependencyProperty.RegisterAttached(
            "SlideYTo",
            typeof(SlideYToExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static SlideYFromExtension GetSlideYFrom(FrameworkElement obj)
    {
        return (SlideYFromExtension)obj.GetValue(SlideYFromProperty);
    }

    public static void SetSlideYFrom(FrameworkElement obj, SlideYFromExtension value)
    {
        obj.SetValue(SlideYFromProperty, value);
    }

    public static readonly DependencyProperty SlideYFromProperty =
        DependencyProperty.RegisterAttached(
            "SlideYFrom",
            typeof(SlideYFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static SlideXFromExtension GetSlideXFrom(FrameworkElement obj)
    {
        return (SlideXFromExtension)obj.GetValue(SlideXFromProperty);
    }

    public static void SetSlideXFrom(FrameworkElement obj, SlideXFromExtension value)
    {
        obj.SetValue(SlideXFromProperty, value);
    }

    public static readonly DependencyProperty SlideXFromProperty =
        DependencyProperty.RegisterAttached(
            "SlideXFrom",
            typeof(SlideXFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static ScaleXFromExtension GetScaleXFrom(FrameworkElement obj)
    {
        return (ScaleXFromExtension)obj.GetValue(ScaleXFromProperty);
    }

    public static void SetScaleXFrom(FrameworkElement obj, ScaleXFromExtension value)
    {
        obj.SetValue(ScaleXFromProperty, value);
    }

    public static readonly DependencyProperty ScaleXFromProperty =
        DependencyProperty.RegisterAttached(
            "ScaleXFrom",
            typeof(ScaleXFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static ScaleYFromExtension GetScaleYFrom(FrameworkElement obj)
    {
        return (ScaleYFromExtension)obj.GetValue(ScaleYFromProperty);
    }

    public static void SetScaleYFrom(FrameworkElement obj, ScaleYFromExtension value)
    {
        obj.SetValue(ScaleYFromProperty, value);
    }

    public static readonly DependencyProperty ScaleYFromProperty =
        DependencyProperty.RegisterAttached(
            "ScaleYFrom",
            typeof(ScaleYFromExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static ScaleXToExtension GetScaleXTo(FrameworkElement obj)
    {
        return (ScaleXToExtension)obj.GetValue(ScaleXToProperty);
    }

    public static void SetScaleXTo(FrameworkElement obj, ScaleXToExtension value)
    {
        obj.SetValue(ScaleXToProperty, value);
    }

    public static readonly DependencyProperty ScaleXToProperty =
        DependencyProperty.RegisterAttached(
            "ScaleXTo",
            typeof(ScaleXToExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );

    public static ScaleYToExtension GetScaleYTo(FrameworkElement obj)
    {
        return (ScaleYToExtension)obj.GetValue(ScaleYToProperty);
    }

    public static void SetScaleYTo(FrameworkElement obj, ScaleYToExtension value)
    {
        obj.SetValue(ScaleYToProperty, value);
    }

    public static readonly DependencyProperty ScaleYToProperty =
        DependencyProperty.RegisterAttached(
            "ScaleYTo",
            typeof(ScaleYToExtension),
            typeof(Animation),
            new PropertyMetadata(null, (s, e) => RegisterStoryboard(s, e.NewValue, e.OldValue))
        );
}

public abstract class AnimationExtension : MarkupExtension
{
    private AnimationTimeline? timeline;

    [DefaultValue(300)]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(300);

    [DefaultValue(0)]
    public TimeSpan Delay { get; set; } = TimeSpan.Zero;

    public sealed override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (
            serviceProvider.GetService(typeof(IProvideValueTarget))
            is not IProvideValueTarget targetProvider
        )
        {
            throw new InvalidOperationException();
        }

        if (targetProvider.TargetObject is not FrameworkElement targetObject)
        {
            string msg =
                $"The bound element must be derived from the {typeof(FrameworkElement).FullName}.";
            throw new ArgumentException(msg);
        }

        ApplyTransformGroup(targetObject);

        timeline = CreateAnimationTimeline(targetObject);

        if (timeline is DoubleAnimation da)
        {
            da.EasingFunction = GetEasingFunction(targetObject);
        }

        return this;
    }

    private void TargetObject_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        OnRenderSizeChanged(e.NewSize);
    }

    internal void Register(Storyboard storyboard, FrameworkElement targetObject)
    {
        targetObject.SizeChanged += TargetObject_SizeChanged;

        storyboard.Children.Add(timeline);
    }

    internal void Unregister(Storyboard storyboard, FrameworkElement targetObject)
    {
        targetObject.SizeChanged -= TargetObject_SizeChanged;

        if (timeline != null && storyboard.Children.Contains(timeline))
        {
            storyboard.Children.Remove(timeline);
        }
    }

    protected virtual void OnRenderSizeChanged(Size newSize) { }

    protected abstract AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject);

    #region Index

    protected const int ScaleIndex = 0;
    protected const int RotateIndex = 1;
    protected const int TranslateIndex = 2;
    #endregion

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

    internal static IEasingFunction GetEasingFunction(FrameworkElement frameworkElement)
    {
        EasingFunctionBase easingFunctionBase = Animation.GetEasingType(frameworkElement) switch
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

        easingFunctionBase.EasingMode = Animation.GetEasingMode(frameworkElement);

        return easingFunctionBase;
    }
}

internal class EventManager
{
    private Delegate? @delegate;
    private EventInfo? eventInfo;
    private readonly Action<FrameworkElement>? callback;
    private EventMode eventMode;
    private CallMode callMode;
    private readonly FrameworkElement? element;

    public EventManager(FrameworkElement element, Action<FrameworkElement> callback)
    {
        this.element = element;
        this.callback = callback;
    }

    internal void Register(EventMode eventMode, CallMode callMode)
    {
        this.eventMode = eventMode;
        this.callMode = callMode;

        if (eventMode == EventMode.None)
        {
            return;
        }

        eventInfo = element!.GetType().GetEvent(eventMode.ToString());

        Type[] eventArguTypes = eventInfo!
            .EventHandlerType!.GetMethod("Invoke")!
            .GetParameters()
            .Select(i => i.ParameterType)
            .ToArray();

        MethodInfo methodInfo = GetType()
            .GetMethod(nameof(EventCallback), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(eventArguTypes);

        @delegate = Delegate.CreateDelegate(eventInfo!.EventHandlerType!, this, methodInfo!);
        eventInfo.AddEventHandler(element, @delegate);
    }

    internal void Unregister()
    {
        eventInfo?.RemoveEventHandler(element, @delegate);
        eventInfo = null;
        @delegate = null;
    }

    private void EventCallback<TObject, TEventArgs>(TObject sender, TEventArgs eventArgs)
        where TEventArgs : EventArgs
    {
        if (callMode == CallMode.Once)
        {
            if (eventInfo != null && @delegate != null && callMode == CallMode.Once)
            {
                eventInfo.RemoveEventHandler(element, @delegate);
                @delegate = null;
                eventInfo = null;
            }
        }

        callback?.Invoke(element!);
    }
}

public class FadeFromExtension : AnimationExtension
{
    public double From { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            From = From.FromRange(0, 1),
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath("(FrameworkElement.Opacity)")
        );

        return doubleAnimation;
    }
}

public class FadeToExtension : AnimationExtension
{
    public double To { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To.FromRange(0, 1),
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath("(FrameworkElement.Opacity)")
        );

        return doubleAnimation;
    }
}

public class RotateToExtension : AnimationExtension
{
    public double To { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(RotateTransform.Angle)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, RotateIndex))
        );

        return doubleAnimation;
    }
}

public class RotateFromExtension : AnimationExtension
{
    public double From { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(RotateTransform.Angle)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            From = From,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, RotateIndex))
        );

        return doubleAnimation;
    }
}

public enum SlideXMode
{
    Left,
    Right
}

public class SlideXFromExtension : AnimationExtension
{
    [DefaultValue(300)]
    public double? From { get; set; }

    public SlideXMode SlideXMode { get; set; }

    DoubleAnimation? doubleAnimation;

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.X)";

        int dir = SlideXMode == SlideXMode.Left ? -1 : 1;

        doubleAnimation = new DoubleAnimation
        {
            From = (From!.HasValue ? From.Value : targetObject.ActualWidth) * dir,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, TranslateIndex))
        );

        return doubleAnimation;
    }

    protected override void OnRenderSizeChanged(Size newSize)
    {
        base.OnRenderSizeChanged(newSize);

        if (doubleAnimation != null)
        {
            doubleAnimation.From =
                (From!.HasValue ? From.Value : newSize.Width)
                * (SlideXMode == SlideXMode.Left ? -1 : 1);
        }
    }
}

public class SlideXToExtension : AnimationExtension
{
    public double? To { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.X)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To!.HasValue ? To.Value : 0,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, TranslateIndex))
        );

        return doubleAnimation;
    }
}

public enum SlideYMode
{
    Top,
    Bottom
}

public class SlideYFromExtension : AnimationExtension
{
    public double? From { get; set; }

    public SlideYMode SlideYMode { get; set; }
    DoubleAnimation? doubleAnimation;

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.Y)";

        int dir = SlideYMode == SlideYMode.Top ? -1 : 1;

        doubleAnimation = new DoubleAnimation
        {
            From = (From.HasValue ? From.Value : targetObject.ActualHeight) * dir,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, TranslateIndex))
        );

        return doubleAnimation;
    }

    protected override void OnRenderSizeChanged(Size newSize)
    {
        base.OnRenderSizeChanged(newSize);

        if (doubleAnimation != null)
        {
            doubleAnimation.From =
                (From!.HasValue ? From.Value : newSize.Height)
                * (SlideYMode == SlideYMode.Top ? -1 : 1);
        }
    }
}

public class SlideYToExtension : AnimationExtension
{
    public double? To { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.Y)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To!.HasValue ? To.Value : 0,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, TranslateIndex))
        );

        return doubleAnimation;
    }
}

public class ScaleXFromExtension : AnimationExtension
{
    public double? From { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleX)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            From = From!.HasValue ? From.Value : 1.5d,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, ScaleIndex))
        );

        return doubleAnimation;
    }
}

public class ScaleXToExtension : AnimationExtension
{
    public double To { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleX)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, ScaleIndex))
        );

        return doubleAnimation;
    }
}

public class ScaleYFromExtension : AnimationExtension
{
    public double? From { get; set; }

    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleY)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            From = From!.HasValue ? From.Value : 1.5d,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, ScaleIndex))
        );

        return doubleAnimation;
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Tiny.Toolkits.AnimationExtension" />
public class ScaleYToExtension : AnimationExtension
{
    /// <summary>
    /// Gets or sets to.
    /// </summary>
    /// <value>
    /// To.
    /// </value>
    public double To { get; set; }

    /// <summary>
    /// Creates the animation timeline.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    /// <returns></returns>
    protected override AnimationTimeline CreateAnimationTimeline(FrameworkElement targetObject)
    {
        const string path =
            "(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleY)";

        DoubleAnimation doubleAnimation = new DoubleAnimation
        {
            To = To,
            BeginTime = Delay,
            Duration = Duration
        };

        Storyboard.SetTarget(doubleAnimation, targetObject);
        Storyboard.SetTargetProperty(
            doubleAnimation,
            new PropertyPath(string.Format(path, ScaleIndex))
        );

        return doubleAnimation;
    }
}
