using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Toolkits.Wpf;

/// <summary>
/// An extended combobox that is enumerating Enum values.
///  <para>Use the <see cref="DescriptionAttribute" /> to display entries.</para>
/// <para>Use the <see cref="BrowsableAttribute" /> to hide specific entries.</para>
/// </summary>
public class EnumSelector : ComboBox
{
    /// <summary>
    /// The enum infos
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<Type, EnumInfo[]> enumInfoCaches = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ObservableCollection<EnumInfo> enumInfos = new ObservableCollection<EnumInfo>();

    static EnumSelector()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumSelector), new FrameworkPropertyMetadata(typeof(ComboBox)));
    }

    [DebuggerNonUserCode]
    private class EnumInfo
    {
        public object? Value;
        public int HashCode;
        public string? DisplayName;

        public override string ToString() => DisplayName!;
    }

    /// <summary>
    /// enum type
    /// </summary>
    public static DependencyProperty EnumTypeProperty = DependencyProperty.Register(
        nameof(EnumType),
        typeof(Type),
        typeof(EnumSelector),
        new FrameworkPropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is not EnumSelector selector)
                {
                    return;
                }

                selector.enumInfos.Clear();

                if (e.NewValue?.GetType() is not Type enumType)
                {
                    return;
                }

                if (enumInfoCaches.TryGetValue(enumType, out var infos) == false)
                {
                    enumInfoCaches[enumType] = enumType
                        .GetFields()
                        .Where(static i => i.IsStatic)
                        .Where(static i => i.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
                        .Select(static i => new EnumInfo
                        {
                            Value = i.GetValue(null)!,
                            HashCode = i.GetValue(null)!.GetHashCode(),
                            DisplayName =
                                i.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                                ?? i.GetCustomAttribute<DescriptionAttribute>()?.Description
                                ?? i.Name
                        })
                        .ToArray();
                }

                for (int i = 0; i < (infos?.Length ?? 0); i++)
                {
                    selector.enumInfos.Add(infos![i]);
                }

                selector.TrySetEmptyValue(selector.EmptyValue);
                selector.TryRemoveIgnores(selector.IgnoreValues?.Cast<object>().ToArray() ?? new object[0]);
                selector.TrySetEnumValue(selector.EnumValue);
            }
        )
    );

    /// <summary>
    /// enum type must be <see cref="Enum"/> type
    /// </summary>
    public Type EnumType
    {
        get => (Type)GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }

    #region private method

    private void TrySetEmptyValue(string? emptyName)
    {
        if (enumInfos.Count > 0 && enumInfos[0].Value is null)
        {
            enumInfos.RemoveAt(0);
        }

        if (string.IsNullOrWhiteSpace(emptyName) == false)
        {
            enumInfos.Insert(
                0,
                new EnumInfo()
                {
                    Value = null,
                    HashCode = 0,
                    DisplayName = emptyName,
                }
            );
        }
    }

    private void TrySetEnumValue(object objValue)
    {
        if (objValue is null)
        {
            this.SelectedIndex = -1;
            return;
        }

        var hashCode = objValue.GetHashCode();

        this.SelectedIndex = IndexOf(this.enumInfos, item => item.HashCode == hashCode);
    }

    private void TryRemoveIgnores(object[] objects)
    {
        if (objects is null || objects.Length == 0 || EnumType is null || enumInfoCaches.TryGetValue(EnumType, out var allInfos) == false)
        {
            return;
        }

        var exist = allInfos.ToList();

        for (int i = 0, length = objects.Length; i < length; i++)
        {
            if (objects[i] is null)
            {
                continue;
            }

            var curHashCode = objects[i].GetHashCode();

            if (exist.FirstOrDefault(i => i.HashCode == curHashCode) is EnumInfo enumInfo)
            {
                exist.Remove(enumInfo);
            }
        }

        var hasEmptyValue = string.IsNullOrWhiteSpace(EmptyValue) == false;
        var offset = (hasEmptyValue ? 1 : 0);
        for (int i = 0, length = (Math.Min(exist.Count, this.enumInfos.Count - offset)); i < length; i++)
        {
            if (enumInfos[i + offset].HashCode != exist[i].HashCode)
            {
                enumInfos.Insert(i + offset, exist[i]);
            }
        }

        while (enumInfos.Count - exist.Count > offset)
        {
            enumInfos.RemoveAt(enumInfos.Count - 1);
        }
    }

    private static int IndexOf(IList<EnumInfo> enumInfos, Func<EnumInfo, bool> func)
    {
        for (int i = 0, length = (enumInfos?.Count ?? 0); i < length; i++)
        {
            if (func(enumInfos![i]) == true)
            {
                return i;
            }
        }

        return -1;
    }

    #endregion




    /// <summary>
    /// enum type must be <see cref="Enum"/> type
    /// </summary>
    public static DependencyProperty EnumValueProperty = DependencyProperty.Register(
        nameof(EnumValue),
        typeof(object),
        typeof(EnumSelector),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (s is EnumSelector @enum)
                {
                    @enum.TrySetEnumValue(e.NewValue);
                }
            }
        )
    );

    /// <summary>
    /// selected enum value ,can be null
    /// </summary>
    public object EnumValue
    {
        get => (object)GetValue(EnumValueProperty);
        set => SetValue(EnumValueProperty, value);
    }

    /// <summary>
    /// default empty value
    /// </summary>
    public static DependencyProperty EmptyValueProperty = DependencyProperty.Register(
        nameof(EmptyValue),
        typeof(string),
        typeof(EnumSelector),
        new FrameworkPropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is EnumSelector @enum)
                {
                    @enum.TrySetEmptyValue(e.NewValue as string);
                    @enum.TrySetEnumValue(@enum.EnumValue);
                }
            }
        )
    );

    /// <summary>
    /// default empty value
    /// </summary>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public string? EmptyValue
    {
        get => (string)GetValue(EmptyValueProperty);
        set => SetValue(EmptyValueProperty, value);
    }

    /// <summary>
    /// ignore values
    /// </summary>
    public static DependencyProperty IgnoreValuesProperty = DependencyProperty.Register(
        nameof(IgnoreValues),
        typeof(IEnumerable),
        typeof(EnumSelector),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (s is EnumSelector @enum)
                {
                    @enum.TryRemoveIgnores((e.NewValue as IEnumerable)?.Cast<object>().ToArray() ?? new object[0]);
                    @enum.TrySetEnumValue(@enum.EnumValue);
                }
            }
        )
    );

    /// <summary>
    /// ignore values
    /// </summary>
    public IEnumerable IgnoreValues
    {
        get => (IEnumerable)GetValue(IgnoreValuesProperty);
        set => SetValue(IgnoreValuesProperty, value);
    }

    /// <summary>
    /// Responds to a <see cref="EnumSelector"/> selection change by raising a <see cref="System.Windows.Controls.Primitives.Selector.SelectionChanged"/> event.
    /// </summary>
    /// <param name="e">Provides data for <see cref="SelectionChangedEventArgs"/>.</param>
    protected sealed override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);

        if (EnumValue is null && SelectedIndex == -1)
        {
            return;
        }

        IsEditable = false;

        object? updateValue = null!;

        if (SelectedIndex >= 0 && SelectedIndex < this.enumInfos.Count)
        {
            updateValue = this.enumInfos[SelectedIndex].Value;
        }

        if (updateValue?.GetHashCode() == EnumValue?.GetHashCode())
        {
            return;
        }

        SetCurrentValue(EnumValueProperty, updateValue);
    }
}
