using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="class"/> of <see cref="TypeConverterExtensions"/>
/// </summary>
public static class TypeConverterExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, TypeConverter>> typeConvertMaps = new();

    /// <summary>
    /// Tries the convert to.
    /// </summary>
    /// <typeparam name="TOut">The type of the out.</typeparam>
    /// <param name="from">From.</param>
    /// <param name="outValue">The out value.</param>
    /// <returns></returns>
    public static bool TryConvertTo<TOut>(object from, out TOut outValue)
    {
        try
        {
            outValue = from.ConvertTo<TOut>();

            return true;
        }
        catch
        {
            outValue = default!;
            return false;
        }
    }

    /// <summary>
    /// Converts to.
    /// </summary>
    /// <typeparam name="To">The type of the o.</typeparam>
    /// <param name="from">From.</param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException">
    /// null values cannot be converted
    /// or
    /// type conversion unsuccessful
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// type converter not registered
    /// or
    /// type converter from {fromType} to {toType} not registered
    /// </exception>
    public static To ConvertTo<To>(this object from)
    {
        Type fromType = from?.GetType() ?? throw new InvalidCastException("null values cannot be converted");

        if (typeConvertMaps.TryGetValue(fromType, out ConcurrentDictionary<Type, TypeConverter>? targetTypeConverterMaps) == false)
        {
            typeConvertMaps[fromType] = targetTypeConverterMaps = new ConcurrentDictionary<Type, TypeConverter>();
        }

        Type toType = typeof(To);

        if (targetTypeConverterMaps.TryGetValue(toType, out TypeConverter? typeConverter) == false)
        {
            typeConverter = TypeDescriptor.GetConverter(toType);

            if (typeConverter is null)
            {
                throw new InvalidOperationException("type converter not registered");
            }

            targetTypeConverterMaps[toType] = typeConverter;
        }

        if (typeConverter.CanConvertFrom(fromType) == false)
        {
            throw new InvalidOperationException($"type converter from {fromType} to {toType} not registered");
        }

        object? destination = typeConverter.ConvertFrom(from);

        if (destination is To toValue)
        {
            return toValue;
        }

        throw new InvalidCastException("type conversion unsuccessful");
    }

    /// <summary>
    /// Creates the converter.
    /// </summary>
    /// <typeparam name="From">The type of the rom.</typeparam>
    /// <typeparam name="To">The type of the o.</typeparam>
    /// <param name="typeConverter">The type converter.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">typeConverter</exception>
    public static ITypeConverterBuilder<To, From> CreateConverter<From, To>(Func<From, To> typeConverter)
    {
        _ = typeConverter ?? throw new ArgumentNullException(nameof(typeConverter));

        InnerCreateConverter(typeConverter);

        return new TypeConverterBuilder<To, From>();
    }

    private static void InnerCreateConverter<From, To>(Func<From, To> typeConverter)
    {
        Type fromType = typeof(From);

        if (typeConvertMaps.TryGetValue(fromType, out ConcurrentDictionary<Type, TypeConverter>? targetTypeConverterMaps) == false)
        {
            typeConvertMaps[fromType] = targetTypeConverterMaps = new ConcurrentDictionary<Type, TypeConverter>();
        }

        targetTypeConverterMaps[typeof(To)] = new InnerTypeConverter<From, To>(typeConverter);
    }

    private class TypeConverterBuilder<From, To> : ITypeConverterBuilder<From, To>
    {
        public void ReverseConverter(Func<From, To> typeConverter)
        {
            _ = typeConverter ?? throw new ArgumentNullException(nameof(typeConverter));

            InnerCreateConverter(typeConverter);
        }
    }

    private class InnerTypeConverter<From, To> : TypeConverter
    {
        private readonly Func<From, To> typeConverter;

        public InnerTypeConverter(Func<From, To> typeConverter)
        {
            this.typeConverter = typeConverter;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return true;
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is From from)
            {
                return typeConverter(from);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}

/// <summary>
/// a <see langword="interface"/> of <see cref="ITypeConverterBuilder{From, To}"/>
/// </summary>
/// <typeparam name="From">The type of the rom.</typeparam>
/// <typeparam name="To">The type of the o.</typeparam>
public interface ITypeConverterBuilder<From, To>
{
    /// <summary>
    /// Reverses the converter.
    /// </summary>
    /// <param name="typeConverter">The type converter.</param>
    void ReverseConverter(Func<From, To> typeConverter);
}
