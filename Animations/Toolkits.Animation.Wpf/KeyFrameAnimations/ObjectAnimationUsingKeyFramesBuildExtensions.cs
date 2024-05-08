using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace Toolkits.Wpf;

/// <summary>
///
/// </summary>
public static class ObjectAnimationUsingKeyFramesBuildExtensions
{
    /// <summary>
    /// Builds the animation.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TPropety">The type of the propety.</typeparam>
    /// <param name="object">The object.</param>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="keyValue">The key value.</param>
    /// <param name="keyTime">The key time.</param>
    /// <returns></returns>
    public static ObjectAnimationUsingKeyFrames BuildAnimation<TObject, TPropety>(
        this TObject @object,
        Expression<Func<TObject, TPropety>> propertyExpression,
        TPropety keyValue,
        KeyTime keyTime
    )
        where TObject : UIElement
    {
        var property = propertyExpression.GetPropertyName();

        return BuildAnimation(@object, property, keyValue, keyTime);
    }

    /// <summary>
    /// Builds the animation.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TPropety">The type of the propety.</typeparam>
    /// <param name="object">The object.</param>
    /// <param name="animationProperty">The animation property.</param>
    /// <param name="keyValue">The key value.</param>
    /// <param name="keyTime">The key time.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// object
    /// or
    /// animationProperty
    /// </exception>
    public static ObjectAnimationUsingKeyFrames BuildAnimation<TObject, TPropety>(
        this TObject @object,
        string animationProperty,
        TPropety keyValue,
        KeyTime keyTime
    )
        where TObject : UIElement
    {
        _ = @object ?? throw new ArgumentNullException(nameof(@object));
        _ = string.IsNullOrWhiteSpace(animationProperty) ? throw new ArgumentNullException(nameof(animationProperty)) : 0;

        var objectAnimation = new ObjectAnimationUsingKeyFrames();

        Storyboard.SetTarget(objectAnimation, @object);
        Storyboard.SetTargetProperty(objectAnimation, new PropertyPath(animationProperty));

        var keyFrame = new DiscreteObjectKeyFrame(keyValue, keyTime);
        objectAnimation.KeyFrames.Add(keyFrame);

        return objectAnimation;
    }

    /// <summary>
    /// Adds the key frame.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="objectAnimation">The object animation.</param>
    /// <param name="keyValue">The key value.</param>
    /// <param name="keyTime">The key time.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// objectAnimation
    /// or
    /// keyValue
    /// </exception>
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
