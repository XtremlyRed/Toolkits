using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Toolkits.Core;

/// <summary>
/// reflection extensions
/// </summary>
/// 2024/1/26 13:24
public static class ReflectionExtensions
{
    #region get set path

    /// <summary>
    /// Get Member Value
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyOrFieldPath"></param>
    /// <returns></returns>
    public static object? GetValue(object? obj, string propertyOrFieldPath)
    {
        if (obj is null || string.IsNullOrWhiteSpace(propertyOrFieldPath))
        {
            return obj;
        }

        string[] parts = propertyOrFieldPath.Split('.');
        object? currentObj = obj;

        foreach (string part in parts)
        {
            if (currentObj == null)
            {
                return null;
            }

            Type currentType = currentObj.GetType();

            if (currentType.IsCollectionType())
            {
                int index = GetCollectionIndex(part);
                currentObj = GetCollectionItem(currentObj, index);
                continue;
            }

            currentObj = FindMemberValueIgnoreCase(currentObj, part);
        }

        return currentObj;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyOrFieldPath"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool SetValue(object? obj, string propertyOrFieldPath, object? value)
    {
        if (obj is null || string.IsNullOrWhiteSpace(propertyOrFieldPath))
        {
            return false;
        }

        string[] parts = propertyOrFieldPath.Split('.');
        object? currentObj = obj;

        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (currentObj == null)
            {
                return false;
            }

            Type currentType = currentObj.GetType();

            if (currentType.IsCollectionType())
            {
                int index = GetCollectionIndex(parts[i]);
                currentObj = GetCollectionItem(currentObj, index);
                continue;
            }

            currentObj = FindMemberValueIgnoreCase(currentObj, parts[i]);
        }

        if (currentObj != null)
        {
            string lastPropertyName = parts[parts.Length - 1];

            Type currentType = currentObj.GetType();

            if (currentType.IsCollectionType())
            {
                int index = GetCollectionIndex(lastPropertyName);
                SetCollectionItem(currentObj, index, value);

                return true;
            }

            FindMemberValueIgnoreCase(currentObj, lastPropertyName, true, value);

            return true;
        }

        return false;
    }

    private static int GetCollectionIndex(string part)
    {
        if (part.EndsWith("]"))
        {
            int startIndex = part.IndexOf("[");
            int endIndex = part.IndexOf("]");
            if (startIndex >= 0 && endIndex >= 0 && startIndex < endIndex)
            {
                string indexStr = part.Substring(startIndex + 1, endIndex - startIndex - 1);
                if (int.TryParse(indexStr, out int index))
                {
                    return index;
                }
            }
        }
        return -1;
    }

    private static object? GetCollectionItem(object? collection, int index)
    {
        if (index < 0)
        {
            throw new IndexOutOfRangeException("invalid index");
        }

        if (collection is IList list)
        {
            return list[index];
        }

        if (collection is IEnumerable enumerable)
        {
            return enumerable.Cast<object>().ElementAtOrDefault(index);
        }

        return null;
    }

    private static void SetCollectionItem(object? collection, int index, object? value)
    {
        if (collection is IList list)
        {
            if (index >= 0 && index < list.Count)
            {
                list[index] = value;
                return;
            }
            throw new IndexOutOfRangeException();
        }

        throw new InvalidOperationException($"The data type must be {typeof(IList)}");
    }

    /// <summary>
    /// Find Member value IgnoreCase
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="memberName"></param>
    /// <param name="isSetValue"></param>
    /// <param name="setValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static object? FindMemberValueIgnoreCase(object instance, string memberName, bool isSetValue = false, object? setValue = null)
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;

        Type type = instance.GetType();

        PropertyInfo? property = type.GetProperty(memberName, bindingFlags);
        if (property != null)
        {
            if (isSetValue)
            {
                property.SetValue(instance, setValue);
                return setValue;
            }

            return property.GetValue(instance);
        }

        FieldInfo? field = type.GetField(memberName, bindingFlags);
        if (field != null)
        {
            if (isSetValue)
            {
                field.SetValue(instance, setValue);
                return setValue;
            }

            return field.GetValue(instance);
        }

        throw new ArgumentException($"Property or Field '{memberName}' does not exist in object type '{type}'.");
    }

    private static bool IsCollectionType(this Type type, bool containsStringType = false)
    {
        if (containsStringType && type == typeof(string))
        {
            return false;
        }

        return typeof(IEnumerable).IsAssignableFrom(type);
    }

    #endregion



    #region get/set member value

    /// <summary>
    /// Gets the member value.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="memberPath">The member path.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">source</exception>
    /// <exception cref="ArgumentException">Member path cannot be null or empty. - memberPath</exception>
    public static object GetMemberValue(object source, string memberPath, bool ignoreCase = false)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (string.IsNullOrWhiteSpace(memberPath))
        {
            throw new ArgumentException("Member path cannot be null or empty.", nameof(memberPath));
        }

        return InnerGetValue(source, memberPath.ToCharArray(), 0, ignoreCase);

        static object InnerGetValue(object source, char[] chars, int index = 0, bool ignoreCase = false)
        {
            int last = chars.Length - 1;

            for (int i = index, length = chars.Length; i < length; i++)
            {
                // member
                if (chars[i] is '.' or '[')
                {
                    string memberName = new string(chars, index, i - index);

                    object currentValue = GetValue(source, memberName, ignoreCase);

                    return InnerGetValue(currentValue, chars, i + 1, ignoreCase);
                }

                //collection
                if (chars[i] == ']')
                {
                    string memberName = new string(chars, index, i - index);
                    int offset = int.Parse(memberName);

                    object currentValue = GetCollectionValue(source as IEnumerable, offset);

                    offset = i + 1;

                    if (offset < length && (chars[offset] is '[' or '.'))
                    {
                        offset += 1;
                    }

                    return InnerGetValue(currentValue, chars, offset, ignoreCase);
                }

                // member
                if (i == last)
                {
                    string memberName = new string(chars, index, i - index + 1);

                    object currentValue = GetValue(source, memberName, ignoreCase);

                    return currentValue;
                }
            }

            return source;
        }
    }

    /// <summary>
    /// Sets the member value.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="memberPath">The member path.</param>
    /// <param name="value">The value.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <exception cref="ArgumentNullException">source</exception>
    /// <exception cref="ArgumentException">Member path cannot be null or empty. - memberPath</exception>
    public static void SetMemberValue(object source, string memberPath, object value, bool ignoreCase = false)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (string.IsNullOrWhiteSpace(memberPath))
        {
            throw new ArgumentException("Member path cannot be null or empty.", nameof(memberPath));
        }

        InnerSetValue(source, memberPath.ToCharArray(), 0, value, ignoreCase);

        static void InnerSetValue(object source, char[] chars, int index, object value, bool ignoreCase)
        {
            int last = chars.Length - 1;

            for (int i = index, length = chars.Length; i < length; i++)
            {
                // member
                if (chars[i] is '.' or '[')
                {
                    string memberName = new string(chars, index, i - index);

                    object currentValue = GetValue(source, memberName, ignoreCase);

                    InnerSetValue(currentValue, chars, i + 1, value, ignoreCase);

                    return;
                }

                // collection
                if (chars[i] == ']')
                {
                    string memberName = new string(chars, index, i - index);
                    int offset = int.Parse(memberName);

                    if (i == last)
                    {
                        if (source is IList list)
                        {
                            list[offset] = value;
                            return;
                        }

                        throw new InvalidOperationException($"object type must be {typeof(IList).FullName}");
                    }

                    object currentValue = GetCollectionValue(source as IEnumerable, offset);

                    offset = i + 1;

                    if (offset < length && (chars[offset] is '[' or '.'))
                    {
                        offset += 1;
                    }

                    InnerSetValue(currentValue, chars, offset, value, ignoreCase);

                    return;
                }

                // member
                if (i == last)
                {
                    string memberName = new string(chars, index, i - index + 1);

                    SetValue(source, memberName, value, ignoreCase);

                    return;
                }
            }
        }
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="value">The value.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <exception cref="InvalidOperationException">Member variable '{memberName}' not found.</exception>
    private static void SetValue(object obj, string memberName, object value, bool ignoreCase = false)
    {
        BindingFlags bindingFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        if (ignoreCase)
        {
            bindingFlag |= BindingFlags.IgnoreCase;
        }

        Type dataType = obj.GetType();

        PropertyInfo? property = dataType.GetProperty(memberName, bindingFlag);

        if (property is not null)
        {
            property.SetValue(obj, value);
            return;
        }

        FieldInfo? field = dataType.GetField(memberName, bindingFlag);

        if (field is not null)
        {
            field.SetValue(obj, value);
            return;
        }

        throw new InvalidOperationException($"Member variable '{memberName}' not found.");
    }

    /// <summary>
    /// Gets the collection value.
    /// </summary>
    /// <param name="objects">The objects.</param>
    /// <param name="offset">The offset.</param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private static object GetCollectionValue(IEnumerable? objects, int offset)
    {
        if (objects is null)
        {
            return null!;
        }

        int index = 0;
        foreach (object? item in objects)
        {
            if (index == offset)
            {
                return item!;
            }
            index++;
        }

        throw new IndexOutOfRangeException($"{offset}: out of range");
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="memberName">Name of the member.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">member variable:{memberName} not found</exception>
    private static object GetValue(object obj, string memberName, bool ignoreCase = false)
    {
        BindingFlags bindingFlag = Instance | Public | NonPublic;

        if (ignoreCase)
        {
            bindingFlag |= IgnoreCase;
        }

        Type dataType = obj.GetType();

        PropertyInfo? property = dataType.GetProperty(memberName, bindingFlag);

        if (property is not null)
        {
            return property.GetValue(obj)!;
        }

        FieldInfo? field = dataType.GetField(memberName, bindingFlag);

        if (field is not null)
        {
            return field.GetValue(obj)!;
        }

        throw new InvalidOperationException($"member variable:{memberName} not found");
    }

    #endregion


    /// <summary>
    /// get type string name
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetExplicitName(Type type)
    {
        string typeName = type.Name;

        Type[] typeArguments = type.GenericTypeArguments;

        if (typeArguments != null && typeArguments.Length > 0)
        {
            typeName = type.Name.Replace($"`{typeArguments.Length}", "");

            string typeArgumentString = string.Join(",", typeArguments.Select(genericType => GetExplicitName(genericType)));

            return $"{typeName}<{typeArgumentString}>";
        }

        return typeName;
    }

    /// <summary>
    /// Gets the assembly resource.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="resourceName">Name of the resource.</param>
    /// <param name="stringComparison">The string comparison.</param>
    /// <returns></returns>
    /// 2023/11/25 13:52
    public static Stream? GetAssemblyResource(
        Assembly assembly,
        string resourceName,
        StringComparison stringComparison = StringComparison.OrdinalIgnoreCase
    )
    {
        string? resourceFullName = assembly.GetManifestResourceNames().Where(i => i.IndexOf(resourceName, stringComparison) > 0).SingleOrDefault();

        if (string.IsNullOrWhiteSpace(resourceFullName))
        {
            return null;
        }

        Stream? stream = assembly.GetManifestResourceStream(resourceFullName);

        if (stream?.CanSeek ?? false)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        return stream;
    }

    /// <summary>
    /// execute method through reflection
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="target"></param>
    /// <param name="methodName"></param>
    /// <param name="params"></param>

    public static object? InvokeMethod<Target>(Target target, string methodName, params object[] @params)
        where Target : class
    {
        if (target is null)
        {
            return default;
        }

        Type[]? types = @params?.Select(i => i.GetType()).ToArray();

        BindingFlags binf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        Type type = target.GetType()!;

        Type baseType = typeof(object);

        do
        {
            if (type!.GetMethod(methodName, binf, null, types!, null) is not MethodInfo methodInfo)
            {
                type = type.BaseType!;

                if (type == baseType)
                {
                    break;
                }

                continue;
            }

            object? value = methodInfo.Invoke(target, @params);

            return value;
        } while (true);

        return default;
    }

    /// <summary>
    /// execute method through reflection
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="target"></param>
    /// <param name="methodName"></param>
    /// <param name="params"></param>
    public static async Task InvokeMethodAsync<Target>(Target target, string methodName, params object[] @params)
        where Target : class
    {
        if (target is null)
        {
            return;
        }

        Type[]? types = @params?.Select(i => i.GetType()).ToArray();

        BindingFlags binf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        System.Reflection.MethodInfo? methodInfo = target.GetType().GetMethod(methodName, binf, null, types!, null);
        if (methodInfo is null)
        {
            return;
        }

        if (typeof(Task).IsAssignableFrom(methodInfo.ReturnType))
        {
            await (methodInfo.Invoke(target, @params) as Task)!;
            return;
        }

#if NET451
        await Task.FromResult(true);
#else
        await Task.CompletedTask;
#endif

        _ = methodInfo.Invoke(target, @params);
    }

    /// <summary>
    /// execute method through reflection
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="target"></param>
    /// <param name="propertyName"></param>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Target? GetPropertyValue<Target>(object target, string propertyName)
    {
        if (target is null)
        {
            return default;
        }

        BindingFlags binf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        PropertyInfo? propertyInfo = target.GetType().GetProperty(propertyName, binf);

        if (propertyInfo is null)
        {
            return default;
        }

        object? propertyValue = propertyInfo.GetValue(target);

        return propertyValue is Target @switch ? @switch : default;
    }

    /// <summary>
    /// Sets the property value.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="target">The target.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    /// 2023/11/25 13:53
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetPropertyValue<Target>(object target, string propertyName, Target value)
    {
        if (target is null)
        {
            return;
        }

        BindingFlags binf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        PropertyInfo? propertyInfo = target.GetType().GetProperty(propertyName, binf);

        propertyInfo?.SetValue(target, value);
    }

    /// <summary>
    /// Sets the property value.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="target">The target.</param>
    /// <param name="filter">The filter.</param>
    /// <param name="value">The value.</param>
    /// 2024/2/19 14:33
    public static void SetPropertyValue<TObject, TProperty>(TObject target, Expression<Func<TObject, TProperty>> filter, TProperty value)
    {
        if (target is null || filter is null)
        {
            return;
        }

        string propertyName = ReflectionExtensions.GetPropertyName(filter);

        BindingFlags binf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        PropertyInfo? propertyInfo = target.GetType().GetProperty(propertyName, binf);

        propertyInfo?.SetValue(target, value);
    }

    /// <summary>
    ///  get proprety name from expression
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertySelector">property Selector</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public static string GetPropertyName<TSource, TPropertyType>(Expression<Func<TSource, TPropertyType>> propertySelector)
    {
        if (propertySelector is null)
        {
            throw new ArgumentNullException(nameof(propertySelector));
        }

        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        UnaryExpression? unaryExpression = propertySelector.Body as UnaryExpression;

        return unaryExpression?.Operand is MemberExpression memberExpression2 ? memberExpression2.Member.Name : string.Empty;
    }

    /// <summary>
    /// all exist attribute map
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Dictionary<string, Attribute[]>> attributeMapper = new();

    /// <summary>
    /// get all attributes from <see cref="Enum"/>
    /// </summary>
    /// <param name="enumValue"><see cref="Enum"/></param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public static TAttribute GetEnumAttribute<TAttribute>(Enum enumValue)
        where TAttribute : Attribute
    {
        _ = enumValue ?? throw new ArgumentNullException(nameof(enumValue));

        Type enumType = enumValue.GetType();

        if (enumType.IsEnum == false)
        {
            throw new InvalidOperationException("invalid data type");
        }

        if (attributeMapper.TryGetValue(enumType, out Dictionary<string, Attribute[]>? mapper) == false)
        {
            Type type = enumValue.GetType();
            string[] allNames = Enum.GetNames(type);

            Dictionary<string, Attribute[]> valueAttributes = new();

            foreach (string item in allNames)
            {
                FieldInfo field = type.GetField(item)!;
                string currentLongValue = field.GetValue(null)!.ToString()!;

                valueAttributes[currentLongValue] = field.GetCustomAttributes().OfType<Attribute>().ToArray();
            }

            attributeMapper[enumType] = mapper = valueAttributes;
        }
        string longValue = enumValue.ToString()!;
        return mapper[longValue].OfType<TAttribute>().FirstOrDefault()!;
    }

    /// <summary>
    /// Gets the property attributes.
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    /// 2024/1/26 11:59
    public static IDictionary<PropertyInfo, TAttribute> GetPropertyAttributes<TObjectType, TAttribute>(bool removeNull = false)
        where TAttribute : Attribute
        where TObjectType : class
    {
        PropertyInfo[] fields = typeof(TObjectType).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();

        Dictionary<PropertyInfo, TAttribute> dict = fields
            .Select(i => new { Mode = i, Attribute = i.GetCustomAttribute<TAttribute>() })
            .ToDictionary(i => i.Mode, i => i.Attribute!);

        if (removeNull == false)
        {
            return dict;
        }

        return dict.Where(i => i.Value != null).ToDictionary(i => i.Key, i => i.Value);
    }

    /// <summary>
    /// Gets the field attributes.
    /// </summary>
    /// <typeparam name="TObjectType">The type of the object type.</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
    /// <returns></returns>
    /// 2024/1/26 11:59
    public static IDictionary<FieldInfo, TAttribute> GetFieldAttributes<TObjectType, TAttribute>()
        where TAttribute : Attribute
        where TObjectType : class
    {
        FieldInfo[] fields = typeof(TObjectType).GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();

        return fields.Select(i => new { Mode = i, Attribute = i.GetCustomAttribute<TAttribute>() }).ToDictionary(i => i.Mode, i => i.Attribute!);
    }

    /// <summary>
    /// Get the<see cref="Attribute"/>of the specified type for the enum type
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDictionary<TEnum, TAttribute> GetEnumAttributes<TEnum, TAttribute>()
        where TAttribute : Attribute
        where TEnum : struct, Enum
    {
        FieldInfo[] fields = typeof(TEnum).GetFields().Where(i => i.IsStatic).ToArray();

        return fields
            .Select(i => new { Mode = (TEnum)i.GetValue(null)!, Attribute = i.GetCustomAttribute<TAttribute>() })
            .ToDictionary(i => i.Mode, i => i.Attribute!);
    }

    /// <summary>
    /// Get the<see cref="Attribute"/>of the specified type for the enum type
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TAttribute"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDictionary<TEnum, TResult> GetEnumAttributes<TEnum, TAttribute, TResult>(Func<TAttribute, TResult> selector)
        where TAttribute : Attribute
        where TEnum : struct, Enum
    {
        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        FieldInfo[] fields = typeof(TEnum).GetFields().Where(i => i.IsStatic).ToArray();

        return fields
            .Select(i => new { Mode = (TEnum)i.GetValue(null)!, Attribute = i.GetCustomAttribute<TAttribute>() })
            .ToDictionary(i => i.Mode, i => selector(i.Attribute!));
    }
}
