using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    public static Storyboard AddAnimation(
        this Storyboard? storyboard,
        params AnimationTimeline[] animations
    )
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (animations is null || animations.Length == 0)
        {
            return storyboard;
        }
        for (int i = 0; i < animations.Length; i++)
        {
            storyboard.Children.Add(animations[i]);
        }
        return storyboard;
    }

    /// <summary>
    /// Registers the completed.
    /// </summary>
    /// <param name="storyboard">The storyboard.</param>
    /// <param name="completeCallback">The complete callback.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">storyboard</exception>
    public static Storyboard RegisterCompleted(
        this Storyboard? storyboard,
        Action? completeCallback
    )
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (completeCallback is null)
        {
            return storyboard;
        }

        if (GetCompleteCallback(storyboard) is not HashSet<Action> actions)
        {
            SetCompleteCallback(storyboard, actions = new HashSet<Action>());
        }

        actions.Add(completeCallback);

        storyboard.Completed += Storyboard_Completed;

        return storyboard;

        static void Storyboard_Completed(object? sender, EventArgs e)
        {
            if (sender is not ClockGroup clockGroup || clockGroup.Timeline is not Storyboard ss)
            {
                return;
            }

            if (GetCompleteCallback(ss) is not HashSet<Action> actions)
            {
                return;
            }

            foreach (var action in actions)
            {
                if (action is null)
                {
                    continue;
                }
                action();
            }
        }
    }

    private static HashSet<Action> GetCompleteCallback(Storyboard obj)
    {
        return (HashSet<Action>)obj.GetValue(CompleteCallbackProperty);
    }

    private static void SetCompleteCallback(Storyboard obj, HashSet<Action> value)
    {
        obj.SetValue(CompleteCallbackProperty, value);
    }

    private static readonly DependencyProperty CompleteCallbackProperty =
        DependencyProperty.RegisterAttached(
            "CompleteCallback",
            typeof(HashSet<Action>),
            typeof(AnimationExtensions),
            new PropertyMetadata(null)
        );
}
