//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Toolkits.Core;

///// <summary>
///// a <see  langword="interface"/> of <see cref="IDescriptor"/>
///// </summary>
//public interface IDescriptor
//{
//    /// <summary>
//    /// <see langword="field"/> infos
//    /// </summary>
//    IReadOnlyList<FieldInfo> FieldInfos { get; }

//    /// <summary>
//    /// <see langword="property"/> infos
//    /// </summary>
//    IReadOnlyList<PropertyInfo> PropertyInfos { get; }

//    /// <summary>
//    /// get <paramref name="fieldInfo"/>'s <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <param name="fieldInfo"></param>
//    /// <returns></returns>
//    TAttribute GetAttribute<TAttribute>(FieldInfo fieldInfo)
//        where TAttribute : Attribute;

//    /// <summary>
//    /// get <paramref name="propertyInfo"/>'s <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <param name="propertyInfo"></param>
//    /// <returns></returns>
//    TAttribute GetAttribute<TAttribute>(PropertyInfo propertyInfo)
//        where TAttribute : Attribute;

//    /// <summary>
//    /// get all <see langword="field"/> info's <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <returns></returns>
//    IEnumerable<(FieldInfo, TAttribute)> GetFieldAttributes<TAttribute>()
//        where TAttribute : Attribute;

//    /// <summary>
//    /// get all <see langword="property"/> info's <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <returns></returns>
//    IEnumerable<(PropertyInfo, TAttribute)> GetPropertyAttributes<TAttribute>()
//        where TAttribute : Attribute;
//}

///// <summary>
///// a <see  langword="interface"/> of <see cref="IDescriptor{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//public interface IDescriptor<T> : IDescriptor
//{
//    /// <summary>
//    ///  get target member <paramref name="expression"/>'s  <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <param name="expression"></param>
//    /// <returns></returns>
//    TAttribute GetAttribute<TAttribute>(Expression<Func<T, object>> expression)
//        where TAttribute : Attribute;
//}

///// <summary>
///// a class of <see cref="GeneralDescriptor"/>
///// </summary>
//[DebuggerDisplay("{typeof(T)}")]
//[EditorBrowsable(EditorBrowsableState.Never)]
//public class GeneralDescriptor : IDescriptor
//{
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    IReadOnlyList<FieldInfo> fieldInfos;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    IReadOnlyList<PropertyInfo> propertyInfos;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    FieldInfoStructurer[] fieldInfoStructurers;

//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    PropertyInfoStructurer[] propertyInfoStructurers;

//    record FieldInfoStructurer(FieldInfo FieldInfo, Attribute[] Attributes);

//    record PropertyInfoStructurer(PropertyInfo PropertyInfo, Attribute[] Attributes);

//    /// <summary>
//    /// create a new instance of <see cref="GeneralDescriptor"/>
//    /// </summary>
//    /// <param name="type"></param>
//    /// <exception cref="ArgumentNullException"></exception>
//    public GeneralDescriptor(Type type)
//    {
//        _ = type ?? throw new ArgumentNullException(nameof(type));

//        fieldInfos = new ReadOnlyCollection<FieldInfo>(type.GetFields(Instance | Public | NonPublic).ToArray());
//        propertyInfos = new ReadOnlyCollection<PropertyInfo>(type.GetProperties(Instance | Public | NonPublic).ToArray());

//        fieldInfoStructurers = fieldInfos.Select(i => new FieldInfoStructurer(i, i.GetCustomAttributes().ToArray())).ToArray();
//        propertyInfoStructurers = propertyInfos.Select(i => new PropertyInfoStructurer(i, i.GetCustomAttributes().ToArray())).ToArray();
//    }

//    /// <summary>
//    /// field infos
//    /// </summary>
//    public IReadOnlyList<FieldInfo> FieldInfos => fieldInfos;

//    /// <summary>
//    /// property infos
//    /// </summary>
//    public IReadOnlyList<PropertyInfo> PropertyInfos => propertyInfos;

//    /// <summary>
//    /// get field attributes.
//    /// </summary>
//    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
//    /// <returns></returns>
//    public IEnumerable<(FieldInfo, TAttribute)> GetFieldAttributes<TAttribute>()
//        where TAttribute : Attribute
//    {
//        for (int i = 0; i < fieldInfoStructurers.Length; i++)
//        {
//            TAttribute attr = default!;

//            for (int j = 0; j < fieldInfoStructurers[i].Attributes.Length; j++)
//            {
//                if (fieldInfoStructurers[i].Attributes[j] is TAttribute attribute)
//                {
//                    attr = attribute;
//                    break;
//                }
//            }

//            yield return (fieldInfoStructurers[i].FieldInfo, attr);
//        }
//    }

//    /// <summary>
//    /// get property attributes.
//    /// </summary>
//    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
//    /// <returns></returns>
//    public IEnumerable<(PropertyInfo, TAttribute)> GetPropertyAttributes<TAttribute>()
//        where TAttribute : Attribute
//    {
//        for (int i = 0; i < propertyInfoStructurers.Length; i++)
//        {
//            TAttribute attr = default!;

//            for (int j = 0; j < propertyInfoStructurers[i].Attributes.Length; j++)
//            {
//                if (propertyInfoStructurers[i].Attributes[j] is TAttribute attribute)
//                {
//                    attr = attribute;
//                    break;
//                }
//            }

//            yield return (propertyInfoStructurers[i].PropertyInfo, attr);
//        }
//    }

//    /// <summary>
//    /// get field attribute.
//    /// </summary>
//    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
//    /// <returns></returns>
//    public TAttribute GetAttribute<TAttribute>(FieldInfo fieldInfo)
//        where TAttribute : Attribute
//    {
//        _ = fieldInfo ?? throw new ArgumentNullException(nameof(fieldInfo));

//        for (int i = 0; i < fieldInfoStructurers.Length; i++)
//        {
//            if (fieldInfoStructurers[i].FieldInfo.Name == fieldInfo.Name)
//            {
//                for (int j = 0; j < fieldInfoStructurers[i].Attributes.Length; j++)
//                {
//                    if (fieldInfoStructurers[i].Attributes[j] is TAttribute attribute)
//                    {
//                        return attribute;
//                    }
//                }
//            }
//        }

//        return default!;
//    }

//    /// <summary>
//    /// get property attribute.
//    /// </summary>
//    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
//    /// <returns></returns>
//    public TAttribute GetAttribute<TAttribute>(PropertyInfo propertyInfo)
//        where TAttribute : Attribute
//    {
//        _ = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));

//        for (int i = 0; i < propertyInfoStructurers.Length; i++)
//        {
//            if (propertyInfoStructurers[i].PropertyInfo.Name == propertyInfo.Name)
//            {
//                for (int j = 0; j < propertyInfoStructurers[i].Attributes.Length; j++)
//                {
//                    if (propertyInfoStructurers[i].Attributes[j] is TAttribute attribute)
//                    {
//                        return attribute;
//                    }
//                }
//            }
//        }

//        return default!;
//    }
//}

///// <summary>
///// a class of <see cref="GeneralDescriptor{T}"/>
///// </summary>
///// <typeparam name="T"></typeparam>
//[DebuggerDisplay("{typeof(T)}")]
//[EditorBrowsable(EditorBrowsableState.Never)]
//public class GeneralDescriptor<T> : GeneralDescriptor
//{
//    /// <summary>
//    /// create a new instance of <see cref="GeneralDescriptor{T}"/>
//    /// </summary>
//    public GeneralDescriptor()
//        : base(typeof(T)) { }

//    /// <summary>
//    /// get target member <paramref name="expression"/>'s  <typeparamref name="TAttribute"/>
//    /// </summary>
//    /// <typeparam name="TAttribute"></typeparam>
//    /// <param name="expression"></param>
//    /// <returns></returns>
//    /// <exception cref="ArgumentNullException"></exception>
//    /// <exception cref="InvalidOperationException"></exception>
//    public TAttribute GetAttribute<TAttribute>(Expression<Func<T, object>> expression)
//        where TAttribute : Attribute
//    {
//        _ = expression ?? throw new ArgumentNullException(nameof(expression));

//        var memberName = ReflectionExtensions.GetPropertyName(expression);

//        if (PropertyInfos.FirstOrDefault(i => i.Name == memberName) is PropertyInfo propertyInfo)
//        {
//            return GetAttribute<TAttribute>(propertyInfo);
//        }

//        if (FieldInfos.FirstOrDefault(i => i.Name == memberName) is FieldInfo fieldInfo)
//        {
//            return GetAttribute<TAttribute>(fieldInfo);
//        }

//        throw new InvalidOperationException($"invalid member name : {memberName}");
//    }
//}
