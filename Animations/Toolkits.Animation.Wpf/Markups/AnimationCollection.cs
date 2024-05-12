using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Toolkits.Wpf.Internal;
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="AnimationCollection"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.Animation" />
public class AnimationCollection : FreezableCollection<Animation>, IList, ICollection<Animation>
{
    /// <summary>
    /// The UI source
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    WeakReference uiSourceRefrence = default!;

    /// <summary>
    /// Adds the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    int IList.Add(object? value)
    {
        if (value is not Animation animation)
        {
            return -1;
        }

        ((ICollection<Animation>)this).Add(animation);
        return 1;
    }

    /// <summary>
    /// Adds the specified animation.
    /// </summary>
    /// <param name="animation">The animation.</param>
    void ICollection<Animation>.Add(Animation animation)
    {
        if (animation is not null && uiSourceRefrence.Target is FrameworkElement element)
        {
            Extensions.Register(element!, animation);

            base.Add(animation);
        }
    }

    /// <summary>
    /// the play.
    /// </summary>
    public bool? Play
    {
        get { return (bool?)GetValue(PlayProperty); }
        set { SetValue(PlayProperty, value); }
    }

    /// <summary>
    ///   play property
    /// </summary>
    public static readonly DependencyProperty PlayProperty = DependencyProperty.Register(
        "Play",
        typeof(bool?),
        typeof(AnimationCollection),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (e.NewValue is not true || s is null)
                {
                    return;
                }

                if (s is Animation animation)
                {
                    animation.AnimationInvoke();
                }
                else if (s is AnimationCollection animations)
                {
                    animations.AnimationInvoke();
                }
            },
            null,
            true,
            UpdateSourceTrigger.PropertyChanged
        )
    );

    /// <summary>
    /// Animations the invoke.
    /// </summary>
    internal void AnimationInvoke()
    {
        this.SetCurrentValue(PlayProperty, false);

        var exp = BindingOperations.GetBindingExpression(this, PlayProperty);
        exp?.UpdateSource();

        foreach (var item in this)
        {
            if (item is not null)
            {
                item.Play = true;
            }
        }
    }

    /// <summary>
    /// Attaches the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    internal void Attach(FrameworkElement element)
    {
        uiSourceRefrence = new WeakReference(element);
    }
}
