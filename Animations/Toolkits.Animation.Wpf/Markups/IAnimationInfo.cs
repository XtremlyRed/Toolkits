using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toolkits.Wpf;

internal interface IAnimationInfo
{
    void Invoke();

    EventMode EventMode { get; }
}

/// <summary>
/// a <see langword="interface"/> of <see cref="IPropertyAnimation"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IPropertyAnimation
{
    /// <summary>
    /// Gets the property.
    /// </summary>
    /// <value>
    /// The property.
    /// </value>
    DependencyProperty Property { get; }
}
