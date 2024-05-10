using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf;

/// <summary>
///
/// </summary>
public partial class Animation
{
    /// <summary>
    ///  the declare.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetDeclare(FrameworkElement obj, Animation value)
    {
        obj.SetValue(DeclareProperty, value);
    }

    /// <summary>
    /// The declare property
    /// </summary>
    public static readonly DependencyProperty DeclareProperty = DependencyProperty.RegisterAttached(
        "Declare",
        typeof(Animation),
        typeof(Animation),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is not FrameworkElement frameworkElement || e.NewValue is not AnimationGroup animationBase)
                {
                    return;
                }

                if (animationBase is AnimationGroup declareGroup)
                {
                    if (declareGroup is not null && declareGroup.Children.Count > 0)
                    {
                        foreach (var item in declareGroup.Children)
                        {
                            Register(frameworkElement, item);
                        }
                    }
                }
                else
                {
                    Register(frameworkElement, animationBase);
                }

                static void Register(FrameworkElement frameworkElement, Animation animationBase)
                {
                    //var animation = animationBase.CreateAnimation(frameworkElement, out var owner);

                    var animationType = animationBase.GetType();

                    var defaultValue = animationType.GetProperty("DefaultValue", Instance | Public | NonPublic)?.GetValue(animationBase);
                    // var fromValue = animation.GetType().GetProperty("From", Instance | Public | NonPublic)?.GetValue(animation);

                    DependencyProperty? property = animationBase is IPropertyAnimation propertyAnimation
                        ? propertyAnimation.Property
                        : animationType.GetProperty("Property", Instance | Public | NonPublic)?.GetValue(animationBase) as DependencyProperty;

                    var info = new AnimationInfo()
                    {
                        // EventMode = animationBase.EventMode,
                        Animation = animationBase,
                        //ElementRef = new WeakReference(owner),
                        UIElementRef = new WeakReference(frameworkElement),
                        Property = property
                    };

                    AnimationExtensions.SetAnimationInfo(animationBase, info);

                    if (info.Property is not null && defaultValue is not null)
                    {
                        frameworkElement.SetCurrentValue(info.Property, defaultValue);
                    }

                    AnimationExtensions.GetAnimations(frameworkElement).Add(info);

                    AnimationExtensions.RegisterEvent(frameworkElement, animationBase.EventMode);
                }
            }
        )
    );
}
