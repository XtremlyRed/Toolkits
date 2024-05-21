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
using static System.Reflection.BindingFlags;

namespace Toolkits.Wpf;

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
    /// <param name="removeCallbackWhenStoryboardCompleted"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">storyboard</exception>
    public static Storyboard RegisterCompleted(
        this Storyboard? storyboard,
        Action? completeCallback,
        bool removeCallbackWhenStoryboardCompleted = true
    )
    {
        _ = storyboard ?? throw new ArgumentNullException(nameof(storyboard));

        if (completeCallback is null)
        {
            return storyboard;
        }

        if (GetCompleteCallback(storyboard) is not HashSet<CompleteInfo> completeInfos)
        {
            SetCompleteCallback(storyboard, completeInfos = new HashSet<CompleteInfo>());
        }

        completeInfos.Add(new CompleteInfo(completeCallback, removeCallbackWhenStoryboardCompleted));

        storyboard.Completed += Storyboard_Completed;

        return storyboard;

        static async void Storyboard_Completed(object? sender, EventArgs e)
        {
            if (sender is not ClockGroup clockGroup || clockGroup.Timeline is not Storyboard storyboard)
            {
                return;
            }

            if (GetCompleteCallback(storyboard) is not HashSet<CompleteInfo> completeInfos)
            {
                return;
            }

            var removeCallback = false;

            foreach (var completeInfo in completeInfos)
            {
                if (completeInfo is null || completeInfo.Callback is null)
                {
                    continue;
                }

                completeInfo.Callback();

                removeCallback |= completeInfo.AutoRelease;
            }

            if (removeCallback)
            {
                await clockGroup.Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(300);
                    storyboard.Completed -= Storyboard_Completed;
                });
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
}
