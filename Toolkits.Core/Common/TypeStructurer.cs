using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    private record PropertyStructurer(PropertyInfo Property, Attribute[] Attributes);

    private record FieldStructurer(FieldInfo Field, Attribute[] Attributes);

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static PropertyStructurer[] propertyStructurers;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static FieldStructurer[] fieldStructurers;

    /// <summary>
    /// Initializes the <see cref="EnumStructurer{T}"/> class.
    /// </summary>
    static TypeStructurer()
    {
        var properties = typeof(T).GetProperties() ?? new PropertyInfo[0];

        PropertyInfos = new ReadOnlyCollection<PropertyInfo>(properties);

        var fields = typeof(T).GetFields() ?? new FieldInfo[0];

        FieldInfos = new ReadOnlyCollection<FieldInfo>(fields);

        propertyStructurers = properties.Select(i => new PropertyStructurer(i, i.GetCustomAttributes()?.ToArray() ?? new Attribute[0])).ToArray();
        fieldStructurers = fields.Select(i => new FieldStructurer(i, i.GetCustomAttributes()?.ToArray() ?? new Attribute[0])).ToArray();
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
        var dict = new Dictionary<PropertyInfo, TAttribute>();

        for (int i = 0; i < propertyStructurers.Length; i++)
        {
            var attributes = propertyStructurers[i].Attributes;

            dict[propertyStructurers[i].Property] = default!;

            for (int j = 0; j < attributes.Length; j++)
            {
                if (attributes[i] is TAttribute attribute)
                {
                    dict[propertyStructurers[i].Property] = attribute;
                    break;
                }
            }
        }

        return dict;
    }

    /// <summary>
    /// Gets the field attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static IReadOnlyDictionary<FieldInfo, TAttribute> GetFieldAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        var dict = new Dictionary<FieldInfo, TAttribute>();

        for (int i = 0; i < fieldStructurers.Length; i++)
        {
            var attributes = fieldStructurers[i].Attributes;

            dict[fieldStructurers[i].Field] = default!;

            for (int j = 0; j < attributes.Length; j++)
            {
                if (attributes[i] is TAttribute attribute)
                {
                    dict[fieldStructurers[i].Field] = attribute;
                    break;
                }
            }
        }

        return dict;
    }
}
