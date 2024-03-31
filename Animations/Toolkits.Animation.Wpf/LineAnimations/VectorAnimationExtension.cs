using System.Linq.Expressions;
using System.Windows;
using System.Windows.Media.Animation;
using Toolkits.Animation.Wpf.Extensions;

namespace Toolkits.Animation;

public static class VectorAnimationExtension
{
    public static VectorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Vector>> propertyExpression,
        Vector toValue,
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

    public static VectorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Vector>> propertyExpression,
        Vector fromValue,
        Vector toValue,
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

    public static VectorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Vector>> propertyExpression,
        Vector toValue,
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

    public static VectorAnimation BuildAnimation<TObject>(
        this TObject @object,
        Expression<Func<TObject, Vector>> propertyExpression,
        Vector? fromValue,
        Vector toValue,
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

    public static VectorAnimation BuildAnimation(
        this UIElement @object,
        string animationProperty,
        Vector? fromValue,
        Vector toValue,
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

        var animation = new VectorAnimation();

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
