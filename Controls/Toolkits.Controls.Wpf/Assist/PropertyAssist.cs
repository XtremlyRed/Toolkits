using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Toolkits.Core;

namespace Toolkits.Wpf;

/// <summary>
/// dependency property assist
/// </summary>
public static partial class PropertyAssist
{
    #region  binding

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);
        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new PropertyMetadata(
                default(TPropertyType),
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    }
            )
        );
        return property;
    }

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        TPropertyType defaultValue,
        Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);
        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new PropertyMetadata(
                defaultValue,
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    }
            )
        );
        return property;
    }

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="updateSourceTrigger"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        TPropertyType defaultValue,
        UpdateSourceTrigger updateSourceTrigger,
        Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);
        FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault;
        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    },
                null,
                false,
                updateSourceTrigger
            )
        );
        return property;
    }

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="flags"></param>
    /// <param name="updateSourceTrigger"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        TPropertyType defaultValue,
        FrameworkPropertyMetadataOptions flags,
        UpdateSourceTrigger updateSourceTrigger,
        Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);
        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    },
                null,
                false,
                updateSourceTrigger
            )
        );
        return property;
    }

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="defaultValue"></param>
    /// <param name="flags"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        TPropertyType defaultValue,
        FrameworkPropertyMetadataOptions flags,
        Action<TDependencyObject, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);

        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    }
            )
        );
        return property;
    }

    /// <summary>
    /// dependency property register
    /// </summary>
    /// <typeparam name="TDependencyObject"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyNameSelector"></param>
    /// <param name="flags"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    public static DependencyProperty PropertyRegister<TDependencyObject, TPropertyType>(
        Expression<Func<TDependencyObject, TPropertyType>> propertyNameSelector,
        FrameworkPropertyMetadataOptions flags,
        Action<TDependencyObject?, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
        where TDependencyObject : DependencyObject
    {
        string propertyName = ReflectionExtensions.GetPropertyName(propertyNameSelector);
        TPropertyType? defaultValue = default;
        DependencyProperty property = DependencyProperty.Register(
            propertyName,
            typeof(TPropertyType),
            typeof(TDependencyObject),
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                propertyChangedCallback is null
                    ? null
                    : (s, e) =>
                    {
                        propertyChangedCallback.Invoke(
                            (s as TDependencyObject)!,
                            new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue)
                        );
                    }
            )
        );
        return property;
    }

    #endregion

    #region

    /// <summary>
    /// dependency property attached
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="ownerType"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DependencyProperty PropertyAttached<TPropertyType>(
        string propertyName,
        Type ownerType,
        Action<object, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (ownerType == null)
        {
            throw new ArgumentNullException(nameof(ownerType));
        }
        PropertyMetadata mata =
            new(
                default(TPropertyType),
                (s, e) =>
                {
                    propertyChangedCallback?.Invoke(s, new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue));
                }
            );
        DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, mata);
        return property;
    }

    /// <summary>
    ///  dependency property attached
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="ownerType"></param>
    /// <param name="defaultValue"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DependencyProperty PropertyAttached<TPropertyType>(
        string propertyName,
        Type ownerType,
        TPropertyType defaultValue,
        Action<object, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (ownerType == null)
        {
            throw new ArgumentNullException(nameof(ownerType));
        }

        PropertyMetadata mata =
            new(
                defaultValue,
                (s, e) =>
                {
                    propertyChangedCallback?.Invoke(s, new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue));
                }
            );

        DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(TPropertyType), ownerType, mata);
        return property;
    }

    /// <summary>
    ///  dependency property attached
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="ownerType"></param>
    /// <param name="defaultValue"></param>
    /// <param name="flags"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DependencyProperty PropertyAttached<TPropertyType>(
        string propertyName,
        Type ownerType,
        TPropertyType defaultValue,
        FrameworkPropertyMetadataOptions flags,
        Action<object, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (ownerType == null)
        {
            throw new ArgumentNullException(nameof(ownerType));
        }

        DependencyProperty property = DependencyProperty.RegisterAttached(
            propertyName,
            typeof(TPropertyType),
            ownerType,
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                (s, e) =>
                {
                    propertyChangedCallback?.Invoke(s, new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue));
                },
                null,
                false
            )
        );
        return property;
    }

    /// <summary>
    ///  dependency property attached
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="ownerType"></param>
    /// <param name="defaultValue"></param>
    /// <param name="flags"></param>
    /// <param name="updateSourceTrigger"></param>
    /// <param name="propertyChangedCallback"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static DependencyProperty PropertyAttached<TPropertyType>(
        string propertyName,
        Type ownerType,
        TPropertyType defaultValue,
        FrameworkPropertyMetadataOptions flags,
        UpdateSourceTrigger updateSourceTrigger,
        Action<object, PropertyChangedEventArgs<TPropertyType>>? propertyChangedCallback = null
    )
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (ownerType == null)
        {
            throw new ArgumentNullException(nameof(ownerType));
        }

        DependencyProperty property = DependencyProperty.RegisterAttached(
            propertyName,
            typeof(TPropertyType),
            ownerType,
            new FrameworkPropertyMetadata(
                defaultValue,
                flags,
                (s, e) =>
                {
                    propertyChangedCallback?.Invoke(s, new PropertyChangedEventArgs<TPropertyType>(e.Property, e.NewValue, e.OldValue));
                },
                null,
                false,
                updateSourceTrigger
            )
        );
        return property;
    }

    #endregion

    /// <summary>
    /// Gets the dependency property.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    public static DependencyProperty? GetDependencyProperty(Type type, string propertyName)
    {
        FieldInfo? fieldInfo = type.GetField(propertyName + "Property", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        return fieldInfo?.GetValue(null) as DependencyProperty;
    }

    /// <summary>
    /// PropertyAssist.DefaultStyle{<typeparamref name="thisType"/>}(DefaultStyleKeyProperty)
    /// </summary>
    /// <typeparam name="thisType">The type of his type.</typeparam>
    /// <param name="dp">The dp.</param>
    public static void DefaultStyle<thisType>(DependencyProperty dp)
    {
        dp.OverrideMetadata(typeof(thisType), new FrameworkPropertyMetadata(typeof(thisType)));
    }

    /// <summary>
    /// property changed event args
    /// </summary>
    /// <typeparam name="TargetType"></typeparam>
    [DebuggerDisplay("property:{Property.Name}  new value:{NewValue}  old value:{OldValue}")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PropertyChangedEventArgs<TargetType> : EventArgs
    {
        internal PropertyChangedEventArgs(DependencyProperty property, object newValue, object oldValue)
        {
            Property = property;
            if (oldValue is TargetType old)
            {
                OldValue = old;
            }
            if (newValue is TargetType @new)
            {
                NewValue = @new;
            }
        }

        /// <summary>
        /// property
        /// </summary>
        public readonly DependencyProperty? Property;

        /// <summary>
        /// old value
        /// </summary>
        public readonly TargetType? OldValue;

        /// <summary>
        /// new value
        /// </summary>
        public readonly TargetType? NewValue;
    }
}
