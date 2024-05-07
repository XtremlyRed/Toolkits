using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Reflection.BindingFlags;

namespace Toolkits.Animation;

public static class Animation
{
    ///// <summary>
    /////   the declare.
    ///// </summary>
    ///// <param name="obj">The object.</param>
    ///// <returns></returns>
    //public static AnimationDeclareBase GetDeclare(FrameworkElement obj)
    //{
    //    return (AnimationDeclareBase)obj.GetValue(DeclareProperty);
    //}

    /// <summary>
    ///  the declare.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetDeclare(FrameworkElement obj, AnimationDeclareBase value)
    {
        obj.SetValue(DeclareProperty, value);
    }

    /// <summary>
    /// The declare property
    /// </summary>
    public static readonly DependencyProperty DeclareProperty = DependencyProperty.RegisterAttached(
        "Declare",
        typeof(AnimationDeclareBase),
        typeof(AnimationDeclare),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is not FrameworkElement frameworkElement || e.NewValue is not AnimationDeclareBase animationBase)
                {
                    return;
                }

                if (animationBase is AnimationDeclareGroup declareGroup)
                {
                    //var propertyInfo = declareGroup.GetType().GetProperty("ContextList", Instance | Public | NonPublic);

                    //var value = propertyInfo?.GetValue(declareGroup);

                    if (declareGroup.Children is not null && declareGroup.Children.Count > 0)
                    {
                        foreach (var item in declareGroup.Children)
                        {
                            // propertyInfo?.SetValue(item, value);

                            Register(frameworkElement, item);
                        }
                    }
                }
                else
                {
                    Register(frameworkElement, animationBase);
                }

                static void Register(FrameworkElement frameworkElement, AnimationDeclareBase animationBase)
                {
                    var animation = animationBase.CreateAnimation(frameworkElement, out var owner);

                    var animationType = animationBase.GetType();

                    var defaultValue = animationType.GetProperty("DefaultValue", Instance | Public | NonPublic)?.GetValue(animationBase);
                    var fromValue = animation.GetType().GetProperty("From", Instance | Public | NonPublic)?.GetValue(animation);

                    var info = new AnimationInfo()
                    {
                        EventMode = animationBase.EventMode,
                        Animation = animation,
                        ElementRef = new WeakReference(owner),
                        Property = animationType.GetProperty("Property", Instance | Public | NonPublic)?.GetValue(animationBase) as DependencyProperty
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
