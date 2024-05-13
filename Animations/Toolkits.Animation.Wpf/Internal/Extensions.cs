using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf.Internal;

internal class Extensions
{
    internal static IAnimationInfo GetAnimationInfo(Animation obj)
    {
        return (IAnimationInfo)obj.GetValue(AnimationInfoProperty);
    }

    internal static void SetAnimationInfo(Animation obj, IAnimationInfo value)
    {
        obj.SetValue(AnimationInfoProperty, value);
    }

    internal static readonly DependencyProperty AnimationInfoProperty = DependencyProperty.RegisterAttached(
        "AnimationInfo",
        typeof(IAnimationInfo),
        typeof(Extensions),
        new PropertyMetadata(null)
    );

    #region register animation event


    internal static bool GetHasRegistered(Animation animation)
    {
        return (bool)animation.GetValue(HasRegisteredProperty);
    }

    internal static void SetHasRegistered(Animation animation, bool value)
    {
        animation.SetValue(HasRegisteredProperty, value);
    }

    internal static readonly DependencyProperty HasRegisteredProperty = DependencyProperty.RegisterAttached(
        "HasRegistered",
        typeof(bool),
        typeof(Extensions),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// Registers the specified framework element.
    /// </summary>
    /// <param name="element">The framework element.</param>
    /// <param name="animation">The animation base.</param>
    internal static void Register(FrameworkElement element, Animation animation)
    {
        if (element is null || GetHasRegistered(animation))
        {
            return;
        }

        var animationType = animation.GetType();

        DependencyProperty? property = animation is IPropertyAnimation propertyAnimation
            ? propertyAnimation.Property
            : animationType.GetProperty("Property", Instance | Public | NonPublic)?.GetValue(animation) as DependencyProperty;

        var info = new AnimationInfo(new WeakReference(element), property!, animation);

        SetAnimationInfo(animation, info);

        GetAnimations(element).Add(info);

        RegisterEvent(element, animation.EventMode);

        SetHasRegistered(animation, true);
    }

    record AnimationInfo(WeakReference UIElementRef, DependencyProperty Property, Animation Animation) : IAnimationInfo
    {
        private MethodInfo? Method;

        //private PropertyInfo? PropertyInfo;
        //private bool isPlayed;
        public EventMode EventMode => Animation?.EventMode ?? EventMode.Loaded;

        /// <summary>
        /// The paramter types
        /// </summary>
        private static readonly Type[] paramterTypes = new Type[] { typeof(DependencyProperty), typeof(AnimationTimeline) };

        public void Invoke()
        {
            if (UIElementRef.Target is not FrameworkElement element)
            {
                return;
            }

            AnimationTimeline animation = Animation.CreateAnimation(element, out object? propertyOwner);

            //if (isPlayed && Property is not null)
            //{
            //    object currentValue = element.GetValue(Property);

            //    PropertyInfo ??= animation.GetType().GetProperty("From")!;

            //    PropertyInfo?.SetValue(animation, currentValue);
            //}

            Method ??= propertyOwner?.GetType().GetMethod(nameof(UIElement.BeginAnimation), paramterTypes)!;

            if (Method is null)
            {
                return;
            }

            Method.Invoke(propertyOwner, new object[] { Property!, animation });

            //isPlayed = true;
        }
    }

    #endregion



    /// <summary>
    /// Gets the animations.
    /// </summary>
    /// <param name="element">The object.</param>
    /// <returns></returns>
    internal static Collection<IAnimationInfo> GetAnimations(FrameworkElement element)
    {
        if (element.GetValue(AnimationsProperty) is not Collection<IAnimationInfo> collection)
        {
            element.SetValue(AnimationsProperty, collection = new Collection<IAnimationInfo>());
        }

        return collection;
    }

    /// <summary>
    /// The animations property
    /// </summary>
    internal static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
        "Animations",
        typeof(Collection<IAnimationInfo>),
        typeof(Extensions),
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
