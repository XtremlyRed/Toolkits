using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
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

#if ___WPF___
namespace Toolkits.Wpf;

#endif
#if ___AVALONIA___
namespace Toolkits.Avalonia;

#endif
#if ___MAUI___
namespace Toolkits.Maui;

#endif

/// <summary>
///
/// </summary>
public class EnumDisplayNameConverter : IValueConverter
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    static ConcurrentDictionary<Type, Dictionary<int, string>> enumValueMaps = new();

    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueType = value?.GetType();

        if (value is null || valueType is null || valueType.IsEnum == false)
        {
#if ___WPF___
            return Binding.DoNothing;
#else
            return default;
#endif
        }

        var valueHashCode = value.GetHashCode();

        if (enumValueMaps.TryGetValue(valueType, out var enumMaps) == false)
        {
            enumValueMaps[valueType] = enumMaps = new Dictionary<int, string>();

            var fields = valueType.GetFields();

            for (int i = 0, length = fields.Length; i < length; i++)
            {
                if (fields[i].IsStatic)
                {
                    var enumValueHashCode = fields[i].GetValue(null)!.GetHashCode();

                    enumMaps[enumValueHashCode] = fields[i].GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? fields[i].Name;
                }
            }
        }

        if (enumMaps.TryGetValue(valueHashCode, out var displayName))
        {
            return displayName;
        }

#if ___WPF___
        return Binding.DoNothing;
#else
        return default;
#endif
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
