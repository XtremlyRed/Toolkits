using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="class"/> of <see cref="IEnumDescriptor{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEnumDescriptor<T>
    where T : struct, Enum
{
    /// <summary>
    /// all <see langword="field"/> infos
    /// </summary>
    IReadOnlyList<FieldInfo> FieldInfos { get; }

    /// <summary>
    /// all <typeparamref name="T"/> values
    /// </summary>
    IReadOnlyList<T> Values { get; }

    /// <summary>
    /// get target <typeparamref name="TAttribute"/>
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    IEnumerable<(T, TAttribute)> GetAttribute<TAttribute>()
        where TAttribute : Attribute;

    /// <summary>
    /// get target <paramref name="value"/>'s <typeparamref name="TAttribute"/>
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    TAttribute GetAttribute<TAttribute>(T value)
        where TAttribute : Attribute;
}

/// <summary>
/// a class of <see cref="EnumDescriptor{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay("{typeof(T)}")]
public class EnumDescriptor<T> : IEnumDescriptor<T>
    where T : struct, Enum
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static IReadOnlyList<T> values;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static IReadOnlyList<FieldInfo> fieldInfos;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static Structurer[] structurers;

    record Structurer(T Value, int HashCode, Attribute[] Attributes);

    static EnumDescriptor()
    {
        values = new ReadOnlyCollection<T>(Enum.GetValues(typeof(T)).OfType<T>().ToArray());

        fieldInfos = typeof(T).GetFields().Where(x => x.IsStatic).ToList();

        structurers = fieldInfos
            .Select(i => new Structurer(
                (T)i.GetValue(null)!,
                ((T)i.GetValue(null)!).GetHashCode(),
                i.GetCustomAttributes()?.ToArray() ?? new Attribute[0]
            ))
            .ToArray();
    }

    /// <summary>
    /// all enum values
    /// </summary>
    public IReadOnlyList<T> Values => values;

    /// <summary>
    /// all enum field infos
    /// </summary>
    public IReadOnlyList<FieldInfo> FieldInfos => fieldInfos;

    /// <summary>
    /// get attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public IEnumerable<(T, TAttribute)> GetAttribute<TAttribute>()
        where TAttribute : Attribute
    {
        for (int i = 0; i < structurers.Length; i++)
        {
            TAttribute attr = default!;

            for (int j = 0; j < structurers[i].Attributes.Length; j++)
            {
                if (structurers[i].Attributes[j] is TAttribute attribute)
                {
                    attr = attribute;
                    break;
                }
            }

            yield return (structurers[i].Value, attr);
        }
    }

    /// <summary>
    /// get attributes.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    public TAttribute GetAttribute<TAttribute>(T value)
        where TAttribute : Attribute
    {
        var hashCode = value.GetHashCode();
        for (int i = 0; i < structurers.Length; i++)
        {
            if (structurers[i].HashCode == hashCode)
            {
                for (int j = 0; j < structurers[i].Attributes.Length; j++)
                {
                    if (structurers[i].Attributes[j] is TAttribute attribute)
                    {
                        return attribute;
                    }
                }
            }
        }

        return default!;
    }
}
