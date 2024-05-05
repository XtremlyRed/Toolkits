using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace Toolkits.Animation;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
[DefaultProperty(nameof(Animations))]
[ContentProperty(nameof(Animations))]
public abstract class AnimationDeclareGroupsBaseMarkupExtension<T> : MarkupExtension
{
    public virtual T DefaultValue { get; set; } = default!;

    public virtual Collection<AnimationDeclareBaseMarkupExtension<T>> Animations { get; } =
        new Collection<AnimationDeclareBaseMarkupExtension<T>>();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget? service =
            serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

        if (service?.TargetProperty is not DependencyProperty targetProperty)
        {
            return DependencyProperty.UnsetValue;
        }

        if (service?.TargetObject is not FrameworkElement frameworkElement)
        {
            return DependencyProperty.UnsetValue;
        }

        foreach (AnimationDeclareBaseMarkupExtension<T> item in Animations)
        {
            item.ProvideValue(serviceProvider);
        }

        return DefaultValue!;
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Double&gt;" />
public class DoubleAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<double> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Thickness&gt;" />
public class ThicknessAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Thickness> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Size&gt;" />
public class SizeAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Size> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Int32&gt;" />
public class Int32AnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<int> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Media.Color&gt;" />
public class ColorAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Color> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Media.Media3D.Point3D&gt;" />
public class Point3DAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Point3D> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Point&gt;" />
public class PointAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Point> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Media.Media3D.Quaternion&gt;" />
public class QuaternionAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Quaternion> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Media.Media3D.Rotation3D&gt;" />
public class Rotation3DAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Rotation3D> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Media.Media3D.Vector3D&gt;" />
public class Vector3DAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Vector3D> { }

/// <summary>
///
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationGroups&lt;System.Windows.Vector&gt;" />
public class VectorAnimationDeclareGroupsExtension
    : AnimationDeclareGroupsBaseMarkupExtension<Vector> { }
