using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="EnumConverter{TAttribute}"/>
/// </summary>
/// <typeparam name="TAttribute"></typeparam>
public abstract class EnumConverter<TAttribute> : ValueConverterBase<object>
    where TAttribute : Attribute
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static ConcurrentDictionary<Type, Dictionary<int, string>> enumValueMaps = new();

    /// <summary>
    /// display
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected abstract Func<TAttribute?, string?> DisplaySelector { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected override object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueType = value?.GetType();

        if (value is null || valueType is null || valueType.IsEnum == false)
        {
            throw new InvalidOperationException($"invalid data type, must be : {typeof(Enum)}");
        }

        var valueHashCode = value.GetHashCode();

        if (enumValueMaps.TryGetValue(valueType, out var enumMaps) == false)
        {
            enumValueMaps[valueType] = enumMaps = new Dictionary<int, string>();

            var fields = valueType.GetFields();

            for (int i = 0, length = fields.Length; i < length; i++)
            {
                if (fields[i].IsStatic == false)
                {
                    continue;
                }

                var enumValueHashCode = fields[i].GetValue(null)!.GetHashCode();

                TAttribute? attribute = fields[i].GetCustomAttribute<TAttribute>();

                enumMaps[enumValueHashCode] = (DisplaySelector?.Invoke(attribute)) ?? fields[i].Name;
            }
        }

        if (enumMaps.TryGetValue(valueHashCode, out var display))
        {
            return display;
        }

#if ___WPF___
        return Binding.DoNothing;
#else
        return null!;
#endif
    }

    /// <summary>
    /// binding input convert
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected override object InputConvert(object? value)
    {
        return value!;
    }
}
