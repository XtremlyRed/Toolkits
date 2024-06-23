//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Runtime.Serialization.Formatters;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Reflection.BindingFlags;

//namespace Toolkits.Core;

///// <summary>
///// a class of <see cref="Descriptor"/>
///// </summary>
//public static class Descriptor
//{
//    static ConcurrentDictionary<Type, object> enumStorages = new();
//    static ConcurrentDictionary<Type, object> classStorages = new();

//    /// <summary>
//    /// <see langword="get"/> descriptor <see langword="when"/> <typeparamref name="T"/> is <see cref="Enum"/>
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <returns></returns>
//    public static IEnumDescriptor<T> GetEnumDescriptor<T>()
//        where T : struct, Enum
//    {
//        if (enumStorages.TryGetValue(typeof(T), out var value) == false || value is not IEnumDescriptor<T>)
//        {
//            enumStorages[typeof(T)] = value = new EnumDescriptor<T>();
//        }

//        return (IEnumDescriptor<T>)value;
//    }

//    /// <summary>
//    /// <see langword="get"/> descriptor <see langword="when"/> <typeparamref name="T"/> is not <see cref="Enum"/>
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <returns></returns>
//    /// <exception cref="NotSupportedException"></exception>
//    public static IDescriptor<T> GetDescriptor<T>()
//    {
//        var type = typeof(T);

//        if (type.IsEnum)
//        {
//            throw new NotSupportedException();
//        }

//        if (classStorages.TryGetValue(type, out var value) == false || value is not IDescriptor<T>)
//        {
//            classStorages[type] = value = new GeneralDescriptor<T>();
//        }

//        return (IDescriptor<T>)value;
//    }

//    /// <summary>
//    /// <see langword="get"/> descriptor <see langword="when"/> is not <see cref="Enum"/>
//    /// </summary>
//    /// <returns></returns>
//    /// <exception cref="NotSupportedException"></exception>
//    public static IDescriptor GetDescriptor(Type type)
//    {
//        _ = type ?? throw new ArgumentNullException(nameof(type));

//        _ = type.IsEnum ? throw new NotSupportedException() : 0;

//        if (classStorages.TryGetValue(type, out var value) == false || value is not IDescriptor)
//        {
//            classStorages[type] = value = new GeneralDescriptor(type);
//        }

//        return (IDescriptor)value;
//    }
//}
