using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Toolkits.Wpf;

/// <summary>
///  a class of <see cref="PropertyAnimationGenerice{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PropertyAnimationGenerice<T> : AnimationGeneric<T>, IPropertyAnimation
{
    /// <summary>
    /// animation property.
    /// </summary>
    public virtual DependencyProperty Property
    {
        get { return (DependencyProperty)GetValue(PropertyProperty); }
        set { SetValue(PropertyProperty, value); }
    }

    /// <summary>
    /// animation property.
    /// </summary>
    public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
        "Property",
        typeof(DependencyProperty),
        typeof(PropertyAnimationGenerice<T>),
        new PropertyMetadata(null)
    );
}

/// <summary>
/// a class of <see cref="BrushPropertyAnimation"/>
/// </summary>
public class BrushPropertyAnimation : PropertyAnimationGenerice<Color>, IPropertyAnimation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrushPropertyAnimation"/> class.
    /// </summary>
    public BrushPropertyAnimation()
    {
        SetCurrentValue(PropertyProperty, SolidColorBrush.ColorProperty);
    }

    DependencyProperty IPropertyAnimation.Property => SolidColorBrush.ColorProperty;

    /// <summary>
    /// Animations the build.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        SolidColorBrush brush = new SolidColorBrush(From);

        var property = SolidColorBrush.ColorProperty;

        element.SetCurrentValue(base.Property, brush);

        ColorAnimation animation = element.BuildAnimation(property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);

        propertyOwner = brush;

        return animation;
    }
}

/// <summary>
///  a class of <see cref="ThicknessPropertyAnimation"/>
/// </summary>
public class ThicknessPropertyAnimation : PropertyAnimationGenerice<Thickness>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="Int32PropertyAnimation"/>
/// </summary>
public class Int32PropertyAnimation : PropertyAnimationGenerice<int>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="DoublePropertyAnimation"/>
/// </summary>
public class DoublePropertyAnimation : PropertyAnimationGenerice<double>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="ColorPropertyAnimation"/>
/// </summary>
public class ColorPropertyAnimation : PropertyAnimationGenerice<Color>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="Point3DPropertyAnimation"/>
/// </summary>
public class Point3DPropertyAnimation : PropertyAnimationGenerice<Point3D>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="QuaternionPropertyAnimation"/>
/// </summary>
public class QuaternionPropertyAnimation : PropertyAnimationGenerice<Quaternion>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="Rotation3DPropertyAnimation"/>
/// </summary>
public class Rotation3DPropertyAnimation : PropertyAnimationGenerice<Rotation3D>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To!, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="VectorPropertyAnimation"/>
/// </summary>
public class VectorPropertyAnimation : PropertyAnimationGenerice<Vector>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="Vector3DPropertyAnimation"/>
/// </summary>
public class Vector3DPropertyAnimation : PropertyAnimationGenerice<Vector3D>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}

/// <summary>
///  a class of <see cref="SizePropertyAnimation"/>
/// </summary>
public class SizePropertyAnimation : PropertyAnimationGenerice<Size>
{
    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        propertyOwner = element;

        return element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);
    }
}
