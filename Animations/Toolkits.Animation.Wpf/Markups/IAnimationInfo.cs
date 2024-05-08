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
///
/// </summary>
internal class AnimationInfo : IAnimationInfo
{
    public MethodInfo Method = default!;
    public PropertyInfo PropertyInfo = default!;
    public AnimationTimeline Animation = default!;
    public DependencyProperty? Property = default!;
    public WeakReference ElementRef = default!;
    public EventMode EventMode { get; set; }

    /// <summary>
    /// The paramter types
    /// </summary>
    private static readonly Type[] paramterTypes = new Type[] { typeof(DependencyProperty), typeof(AnimationTimeline) };

    public void Invoke()
    {
        if (ElementRef.Target is null)
        {
            return;
        }

        //if (Property is not null && element.IsLoaded)
        //{
        //    object currentValue = element.GetValue(Property);

        //    PropertyInfo? property = PropertyInfo ??= Animation.GetType().GetProperty("From")!;

        //    property?.SetValue(Animation, currentValue);
        //}

        Method ??= ElementRef.Target?.GetType().GetMethod(nameof(UIElement.BeginAnimation), paramterTypes)!;

        Method?.Invoke(ElementRef.Target, new object[] { Property!, Animation });
    }
}
