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
///  a class of <see cref="PropertyAnimationDeclareGenerice{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class PropertyAnimationDeclareGenerice<T> : AnimationDeclareGeneric<T>
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
        typeof(PropertyAnimationDeclareGenerice<T>),
        new PropertyMetadata(null)
    );
}

/// <summary>
/// a class of <see cref="BrushAnimationDeclare"/>
/// </summary>
public class BrushAnimationDeclare : PropertyAnimationDeclareGenerice<Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrushAnimationDeclare"/> class.
    /// </summary>
    public BrushAnimationDeclare()
    {
        SetCurrentValue(PropertyProperty, SolidColorBrush.ColorProperty);
    }

    /// <summary>
    /// the property.
    /// </summary>
    public override DependencyProperty Property
    {
        get => base.Property;
    }

    /// <summary>
    /// Animations the build.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        SolidColorBrush brush = new SolidColorBrush(From);

        ColorAnimation animation = element.BuildAnimation(Property!.Name, From, To, TimeSpan.Zero, Duration.TimeSpan);

        propertyOwner = brush;

        return animation;
    }
}

/// <summary>
///  a class of <see cref="ThicknessAnimationDeclare"/>
/// </summary>
public class ThicknessAnimationDeclare : PropertyAnimationDeclareGenerice<Thickness>
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
///  a class of <see cref="Int32AnimationDeclare"/>
/// </summary>
public class Int32AnimationDeclare : PropertyAnimationDeclareGenerice<int>
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
///  a class of <see cref="DoubleAnimationDeclare"/>
/// </summary>
public class DoubleAnimationDeclare : PropertyAnimationDeclareGenerice<double>
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
///  a class of <see cref="ColorAnimationDeclare"/>
/// </summary>
public class ColorAnimationDeclare : PropertyAnimationDeclareGenerice<Color>
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
///  a class of <see cref="Point3DAnimationDeclare"/>
/// </summary>
public class Point3DAnimationDeclare : PropertyAnimationDeclareGenerice<Point3D>
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
///  a class of <see cref="QuaternionAnimationDeclare"/>
/// </summary>
public class QuaternionAnimationDeclare : PropertyAnimationDeclareGenerice<Quaternion>
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
///  a class of <see cref="Rotation3DAnimationDeclare"/>
/// </summary>
public class Rotation3DAnimationDeclare : PropertyAnimationDeclareGenerice<Rotation3D>
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
///  a class of <see cref="VectorAnimationDeclare"/>
/// </summary>
public class VectorAnimationDeclare : PropertyAnimationDeclareGenerice<Vector>
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
///  a class of <see cref="Vector3DAnimationDeclare"/>
/// </summary>
public class Vector3DAnimationDeclare : PropertyAnimationDeclareGenerice<Vector3D>
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
///  a class of <see cref="SizeAnimationDeclare"/>
/// </summary>
public class SizeAnimationDeclare : PropertyAnimationDeclareGenerice<Size>
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
