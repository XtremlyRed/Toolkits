using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Toolkits.Wpf.Internal;
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="Interaction"/>
/// </summary>
public static class Interaction
{
    /// <summary>
    /// Gets the animations.
    /// </summary>
    /// <param name="element">The object.</param>
    /// <returns></returns>
    public static AnimationCollection GetAnimations(FrameworkElement element)
    {
        if (element.GetValue(AnimationsProperty) is not AnimationCollection animations)
        {
            animations = new AnimationCollection();

            animations.Attach(element!);

            element.SetValue(AnimationsProperty, animations);
        }

        return animations;
    }

    /// <summary>
    /// Sets the animations.
    /// </summary>
    /// <param name="element">The object.</param>
    /// <param name="value">The value.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetAnimations(FrameworkElement element, AnimationCollection animations)
    {
        if (animations is not null)
        {
            animations.Attach(element!);

            for (int i = 0, length = animations.Count; i < length; i++)
            {
                var animation = animations[i];

                if (animations[i] is null)
                {
                    continue;
                }

                Extensions.Register(element!, animations[i]);
            }
        }

        element.SetValue(AnimationsProperty, animations);
    }

    /// <summary>
    /// The animations property
    /// </summary>
    public static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
        "ShadowAnimations",
        typeof(AnimationCollection),
        typeof(Interaction),
        new FrameworkPropertyMetadata(null)
    );
}
