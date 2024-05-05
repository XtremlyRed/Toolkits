using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public static class EnumStructurer<T>
    where T : struct, Enum
{
    private record Structurer(T Value, Attribute[] Attributes);

    /// <summary>
    /// The object maps
    /// </summary>

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static Structurer[] structurers;

    /// <summary>
    /// Initializes the <see cref="EnumStructurer{T}"/> class.
    /// </summary>
    static EnumStructurer()
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

        Values = new ReadOnlyCollection<T>(values);

        var fieldInfos = typeof(T).GetFields().Where(x => x.IsStatic).ToList();

        FieldInfos = new ReadOnlyCollection<FieldInfo>(fieldInfos);

        structurers = fieldInfos.Select(i => new Structurer((T)i.GetValue(null)!, i.GetCustomAttributes()?.ToArray() ?? new Attribute[0])).ToArray();
    }

    /// <summary>
    ///   all values
    /// </summary>
    public static readonly IReadOnlyList<T> Values;

    /// <summary>
    /// The fields
    /// </summary>
    public static readonly IReadOnlyList<FieldInfo> FieldInfos;

    /// <summary>
    /// Gets the attribute map.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public static IDictionary<T, TAttribute> GetAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        var dict = new Dictionary<T, TAttribute>();

        for (int i = 0; i < structurers.Length; i++)
        {
            dict[structurers[i].Value] = default!;

            var attributes = structurers[i].Attributes;
            for (int j = 0; j < attributes.Length; j++)
            {
                if (attributes[j] is TAttribute attribute)
                {
                    dict[structurers[i].Value] = attribute;
                    break;
                }
            }
        }

        return dict;
    }
}
