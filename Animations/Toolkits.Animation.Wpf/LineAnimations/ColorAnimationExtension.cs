using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media.Animation;
using Toolkits.Animation.Wpf.Extensions;

namespace Toolkits.Animation;

public static class ColorAnimationExtension
{
    public static ColorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Color>> propertyExpression,
        Color toValue,
        TimeSpan duration,
        Action? completeCallback = null
    )
        where TObject : UIElement
    {
        var property = ReflectionExtensions.GetPropertyName(propertyExpression);

        return BuildAnimation(
            @object,
            property,
            null,
            toValue,
            null,
            duration,
            null,
            completeCallback
        );
    }

    public static ColorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Color>> propertyExpression,
        Color fromValue,
        Color toValue,
        TimeSpan duration,
        Action? completeCallback = null
    )
        where TObject : UIElement
    {
        var property = ReflectionExtensions.GetPropertyName(propertyExpression);

        return BuildAnimation(
            @object,
            property,
            fromValue,
            toValue,
            null,
            duration,
            null,
            completeCallback
        );
    }

    public static ColorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Color>> propertyExpression,
        Color toValue,
        TimeSpan beginTime,
        TimeSpan duration,
        Action? completeCallback = null
    )
        where TObject : UIElement
    {
        var property = ReflectionExtensions.GetPropertyName(propertyExpression);

        return BuildAnimation(
            @object,
            property,
            null,
            toValue,
            beginTime,
            duration,
            null,
            completeCallback
        );
    }

    public static ColorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Color>> propertyExpression,
        Color? fromValue,
        Color toValue,
        TimeSpan? beginTime,
        TimeSpan duration,
        Action? completeCallback = null
    )
        where TObject : UIElement
    {
        var property = ReflectionExtensions.GetPropertyName(propertyExpression);

        return BuildAnimation(
            @object,
            property,
            fromValue,
            toValue,
            beginTime,
            duration,
            null,
            completeCallback
        );
    }

    public static ColorAnimation BuildAnimation(
        this UIElement @object,
        string animationProperty,
        Color? fromValue,
        Color toValue,
        TimeSpan? beginTime,
        TimeSpan duration,
        IEasingFunction? easingFunction = null,
        Action? completeCallback = null
    )
    {
        _ = @object ?? throw new ArgumentNullException(nameof(@object));
        _ = string.IsNullOrWhiteSpace(animationProperty)
            ? throw new ArgumentNullException(nameof(animationProperty))
            : 0;

        var animation = new ColorAnimation();

        if (fromValue.HasValue)
        {
            animation.From = fromValue.Value;
        }
        if (beginTime.HasValue)
        {
            animation.BeginTime = beginTime.Value;
        }
        if (easingFunction is not null)
        {
            animation.EasingFunction = easingFunction;
        }

        animation.Duration = duration;
        animation.To = toValue;

        if (completeCallback is not null)
        {
            animation.Completed += Animation_Completed;

            void Animation_Completed(object sender, EventArgs e)
            {
                animation.Completed -= Animation_Completed;
                completeCallback();
            }
        }

        Storyboard.SetTarget(animation, @object);
        Storyboard.SetTargetProperty(animation, new PropertyPath(animationProperty));

        return animation;
    }
}
