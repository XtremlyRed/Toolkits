using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// The object maps
    /// </summary>
    static ConcurrentDictionary<Type, object> objMaps = new();

    /// <summary>
    /// Initializes the <see cref="EnumStructurer{T}"/> class.
    /// </summary>
    static EnumStructurer()
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

        Values = new ReadOnlyCollection<T>(values);

        var fieldInfos = typeof(T).GetFields().Where(x => x.IsStatic).ToList();

        FieldInfos = new ReadOnlyCollection<FieldInfo>(fieldInfos);
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
    public static IReadOnlyDictionary<T, TAttribute> GetAttributes<TAttribute>()
        where TAttribute : Attribute
    {
        var type = typeof(TAttribute);

        if (objMaps.TryGetValue(type, out var maps) == false)
        {
            var attributeMap = new Dictionary<T, TAttribute>();

            foreach (var fieldInfo in FieldInfos)
            {
                var value = (T)fieldInfo.GetValue(null)!;

                var attribute = fieldInfo.GetCustomAttribute<TAttribute>();

                attributeMap[value] = attribute!;
            }

            objMaps[type] = maps = new ReadOnlyDictionary<T, TAttribute>(attributeMap);
        }

        return (IReadOnlyDictionary<T, TAttribute>)maps;
    }
}
