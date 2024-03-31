using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Toolkits.Animation.Wpf.Extensions;

namespace Toolkits.Animation;

public static class ObjectAnimationUsingKeyFramesExtensions
{
    public static ObjectAnimationUsingKeyFrames BuildAnimation<TObject, TPropety>(
        this TObject @object,
        Expression<Func<TObject, TPropety>> propertyExpression,
        TPropety keyValue,
        KeyTime keyTime
    )
        where TObject : UIElement
    {
        var property = ReflectionExtensions.GetPropertyName(propertyExpression);

        return BuildAnimation(@object, property, keyValue, keyTime);
    }

    public static ObjectAnimationUsingKeyFrames BuildAnimation<TObject, TPropety>(
        this TObject @object,
        string animationProperty,
        TPropety keyValue,
        KeyTime keyTime
    )
        where TObject : UIElement
    {
        _ = @object ?? throw new ArgumentNullException(nameof(@object));
        _ = string.IsNullOrWhiteSpace(animationProperty)
            ? throw new ArgumentNullException(nameof(animationProperty))
            : 0;

        var objectAnimation = new ObjectAnimationUsingKeyFrames();

        Storyboard.SetTarget(objectAnimation, @object);
        Storyboard.SetTargetProperty(objectAnimation, new PropertyPath(animationProperty));

        var keyFrame = new DiscreteObjectKeyFrame(keyValue, keyTime);
        objectAnimation.KeyFrames.Add(keyFrame);

        return objectAnimation;
    }

    public static ObjectAnimationUsingKeyFrames AddKeyFrame<TProperty>(
        this ObjectAnimationUsingKeyFrames objectAnimation,
        TProperty keyValue,
        KeyTime keyTime
    )
    {
        _ = objectAnimation ?? throw new ArgumentNullException(nameof(objectAnimation));
        _ = keyValue ?? throw new ArgumentNullException(nameof(keyValue));

        var keyFrame = new DiscreteObjectKeyFrame(keyValue, keyTime);

        objectAnimation.KeyFrames.Add(keyFrame);

        return objectAnimation;
    }
}
