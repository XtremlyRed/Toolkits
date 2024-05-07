using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Toolkits.Animation;

/// <summary>
///
/// </summary>
public static class AnimationExtensions
{
    /// <summary>
    /// Adds the animation.
    /// </summary>
    /// <param name="storyboard">The storyboard.</param>
    /// <param name="animations">The animations.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">storyboard</exception>
    public static Storyboard AppendAnimations(this Storyboard? storyboard, params AnimationTimeline[] animations)
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (animations is null || animations.Length == 0)
        {
            return storyboard;
        }
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i] is not null)
                storyboard.Children.Add(animations[i]);
        }
        return storyboard;
    }

    /// <summary>
    /// Adds the animation.
    /// </summary>
    /// <param name="storyboard">The storyboard.</param>
    /// <param name="animations">The animations.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">storyboard</exception>
    public static Storyboard AppendAnimations(this Storyboard? storyboard, IEnumerable<AnimationTimeline> animations)
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (animations is null)
        {
            return storyboard;
        }

        foreach (var item in animations)
        {
            if (item is not null)
            {
                storyboard.Children.Add(item);
            }
        }
        return storyboard;
    }

    /// <summary>
    /// Registers the completed.
    /// </summary>
    /// <param name="storyboard">The storyboard.</param>
    /// <param name="completeCallback">The complete callback.</param>
    /// <param name="autoRelease"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">storyboard</exception>
    public static Storyboard RegisterCompleted(this Storyboard? storyboard, Action? completeCallback, bool autoRelease = true)
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (completeCallback is null)
        {
            return storyboard;
        }

        if (GetCompleteCallback(storyboard) is not HashSet<CompleteInfo> actions)
        {
            SetCompleteCallback(storyboard, actions = new HashSet<CompleteInfo>());
        }

        actions.Add(new CompleteInfo(completeCallback, autoRelease));

        storyboard.Completed += Storyboard_Completed;

        return storyboard;

        static void Storyboard_Completed(object? sender, EventArgs e)
        {
            if (sender is not ClockGroup clockGroup || clockGroup.Timeline is not Storyboard ss)
            {
                return;
            }

            if (GetCompleteCallback(ss) is not HashSet<CompleteInfo> infos)
            {
                return;
            }

            foreach (var info in infos)
            {
                if (info is null || info.Callback is null)
                {
                    continue;
                }

                if (info.AutoRelease)
                {
                    ss.Completed -= Storyboard_Completed;
                }

                info.Callback();
            }
        }
    }

    private static HashSet<CompleteInfo> GetCompleteCallback(Storyboard obj)
    {
        return (HashSet<CompleteInfo>)obj.GetValue(CompleteCallbackProperty);
    }

    private static void SetCompleteCallback(Storyboard obj, HashSet<CompleteInfo> value)
    {
        obj.SetValue(CompleteCallbackProperty, value);
    }

    private static readonly DependencyProperty CompleteCallbackProperty = DependencyProperty.RegisterAttached(
        "CompleteCallback",
        typeof(HashSet<CompleteInfo>),
        typeof(AnimationExtensions),
        new PropertyMetadata(null)
    );

    private record CompleteInfo(Action Callback, bool AutoRelease);

    internal static IAnimationInfo GetAnimationInfo(AnimationDeclareBase obj)
    {
        return (IAnimationInfo)obj.GetValue(AnimationInfoProperty);
    }

    internal static void SetAnimationInfo(AnimationDeclareBase obj, IAnimationInfo value)
    {
        obj.SetValue(AnimationInfoProperty, value);
    }

    internal static readonly DependencyProperty AnimationInfoProperty = DependencyProperty.RegisterAttached(
        "AnimationInfo",
        typeof(IAnimationInfo),
        typeof(AnimationExtensions),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// Gets the animations.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    internal static Collection<IAnimationInfo> GetAnimations(DependencyObject obj)
    {
        if (obj.GetValue(AnimationsProperty) is not Collection<IAnimationInfo> collection)
        {
            obj.SetValue(AnimationsProperty, collection = new Collection<IAnimationInfo>());
        }

        return collection;
    }

    /// <summary>
    /// The animations property
    /// </summary>
    internal static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
        "Animations",
        typeof(Collection<IAnimationInfo>),
        typeof(IAnimationInfo),
        new PropertyMetadata(null)
    );

    #region Event
    /// <summary>
    /// Registers the event.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    /// <param name="eventMode">The event mode.</param>
    internal static void RegisterEvent(FrameworkElement frameworkElement, EventMode eventMode)
    {
        switch (eventMode)
        {
            case EventMode.None:
                break;
            case EventMode.Loaded:

                WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.Loaded),
                    FrameworkElement_Loaded
                );
                break;
            case EventMode.Unloaded:
                WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.Unloaded),
                    FrameworkElement_Unloaded
                );
                break;
            case EventMode.MouseEnter:
                WeakEventManager<FrameworkElement, MouseEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.MouseEnter),
                    FrameworkElement_MouseEnter
                );
                break;
            case EventMode.MouseLeave:
                WeakEventManager<FrameworkElement, MouseEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.MouseLeave),
                    FrameworkElement_MouseLeave
                );
                break;
            case EventMode.DataContextChanged:

                frameworkElement.DataContextChanged += FrameworkElement_DataContextChanged;
                break;
            case EventMode.GotFocus:
                WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.GotFocus),
                    FrameworkElement_GotFocus
                );
                break;
            case EventMode.LostFocus:
                WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(
                    frameworkElement,
                    nameof(FrameworkElement.LostFocus),
                    FrameworkElement_LostFocus
                );
                break;
        }
    }

    /// <summary>
    /// Frameworks the element lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_LostFocus(object? sender, RoutedEventArgs e)
    {
        BeginAnimation(sender, EventMode.LostFocus);
    }

    /// <summary>
    /// Frameworks the element got focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_GotFocus(object? sender, RoutedEventArgs e)
    {
        BeginAnimation(sender, EventMode.GotFocus);
    }

    /// <summary>
    /// Handles the DataContextChanged event of the FrameworkElement control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        BeginAnimation(sender, EventMode.DataContextChanged);
    }

    /// <summary>
    /// Frameworks the element mouse leave.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_MouseLeave(object? sender, System.Windows.Input.MouseEventArgs e)
    {
        BeginAnimation(sender, EventMode.MouseLeave);
    }

    /// <summary>
    /// Frameworks the element mouse enter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_MouseEnter(object? sender, MouseEventArgs e)
    {
        BeginAnimation(sender, EventMode.MouseEnter);
    }

    /// <summary>
    /// Frameworks the element unloaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_Unloaded(object? sender, RoutedEventArgs e)
    {
        BeginAnimation(sender, EventMode.Unloaded);
    }

    /// <summary>
    /// Frameworks the element loaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private static void FrameworkElement_Loaded(object? sender, RoutedEventArgs e)
    {
        BeginAnimation(sender, EventMode.Loaded);
    }

    /// <summary>
    /// Begins the animation.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventMode">The event mode.</param>
    private static void BeginAnimation(object? sender, EventMode eventMode)
    {
        if (sender is not FrameworkElement element)
        {
            return;
        }

        IAnimationInfo[] array = GetAnimations(element).Where(i => i.EventMode == eventMode).ToArray();

        for (int i = 0; i < array.Length; i++)
        {
            array[i].Invoke();
        }
    }

    #endregion
}
