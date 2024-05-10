using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace Toolkits.Wpf;

internal interface IAnimationInfo
{
    void Invoke();

    EventMode EventMode { get; }
}

/// <summary>
/// a <see langword="interface" of <see cref="IPropertyAnimation"/>/>
/// </summary>
public interface IPropertyAnimation
{
    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <value>
    /// The property.
    /// </value>
    DependencyProperty Property { get; }
}

/// <summary>
///
/// </summary>
internal class AnimationInfo : IAnimationInfo
{
    public MethodInfo Method = default!;
    public PropertyInfo PropertyInfo = default!;
    public Animation Animation = default!;
    public DependencyProperty? Property = default!;
    public WeakReference UIElementRef = default!;
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

        var animation = Animation.CreateAnimation(element, out var propertyOwner);

        //if (Property is not null && ElementRef.Target is DependencyObject element)
        //{
        //    object currentValue = element.GetValue(Property);

        //    PropertyInfo? property = PropertyInfo ??= Animation.GetType().GetProperty("From")!;

        //    property?.SetValue(Animation, currentValue);
        //}

        Method ??= propertyOwner?.GetType().GetMethod(nameof(UIElement.BeginAnimation), paramterTypes)!;

        Method?.Invoke(propertyOwner, new object[] { Property!, animation });
    }
}
