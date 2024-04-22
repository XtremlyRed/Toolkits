using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public static class TypeStructurer<T>
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    static ConcurrentDictionary<Type, object> propertyMaps = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    static ConcurrentDictionary<Type, object> fieldMaps = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    static PropertyInfo[] emptyProperty = new PropertyInfo[0];

    [EditorBrowsable(EditorBrowsableState.Never)]
    static FieldInfo[] emptyField = new FieldInfo[0];

    /// <summary>
    /// Initializes the <see cref="EnumStructurer{T}"/> class.
    /// </summary>
    static TypeStructurer()
    {
        var properties = typeof(T).GetProperties() ?? emptyProperty;

        PropertyInfos = new ReadOnlyCollection<PropertyInfo>(properties);

        var fields = typeof(T).GetFields() ?? emptyField;

        FieldInfos = new ReadOnlyCollection<FieldInfo>(fields);
    }

    /// <summary>
    /// The fields
    /// </summary>
    public static readonly IReadOnlyList<FieldInfo> FieldInfos;

    /// <summary>
    /// The property infos
    /// </summary>
    public static readonly IReadOnlyList<PropertyInfo> PropertyInfos;

    /// <summary>
    /// Gets the property attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static IReadOnlyDictionary<PropertyInfo, TAttribute> GetPropertyAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        var type = typeof(TAttribute);

        if (propertyMaps.TryGetValue(type, out var maps) == false)
        {
            var attributeMap = new Dictionary<PropertyInfo, TAttribute>();

            foreach (var propertyInfo in PropertyInfos)
            {
                var attribute = propertyInfo.GetCustomAttribute<TAttribute>();

                attributeMap[propertyInfo] = attribute!;
            }

            propertyMaps[type] = maps = new ReadOnlyDictionary<PropertyInfo, TAttribute>(
                attributeMap
            );
        }

        return (IReadOnlyDictionary<PropertyInfo, TAttribute>)maps;
    }

    /// <summary>
    /// Gets the field attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static IReadOnlyDictionary<FieldInfo, TAttribute> GetFieldAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        var type = typeof(TAttribute);

        if (fieldMaps.TryGetValue(type, out var maps) == false)
        {
            var attributeMap = new Dictionary<FieldInfo, TAttribute>();

            foreach (var fieldInfo in FieldInfos)
            {
                var attribute = fieldInfo.GetCustomAttribute<TAttribute>();

                attributeMap[fieldInfo] = attribute!;
            }

            fieldMaps[type] = maps = new ReadOnlyDictionary<FieldInfo, TAttribute>(attributeMap);
        }

        return (IReadOnlyDictionary<FieldInfo, TAttribute>)maps;
    }
}
