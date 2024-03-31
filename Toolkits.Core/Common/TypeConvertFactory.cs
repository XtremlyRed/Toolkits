using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Toolkits;

/// <summary>
/// class of <see cref="TypeConvertExtensions"/>
/// </summary>
public static class TypeConvertExtensions
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<string, TypeConverter> typeConverters = new();

    /// <summary>
    /// Tries to.
    /// </summary>
    /// <typeparam name="To">The type of the o.</typeparam>
    /// <param name="object">The object.</param>
    /// <param name="toValue">ConvertTo value.</param>
    /// <returns></returns>
    public static bool TryConvertTo<To>(object? @object, out To toValue)
    {
        if (@object is null)
        {
            toValue = default!;
            return false;
        }

        try
        {
            toValue = ConvertTo<To>(@object);
            return true;
        }
        catch
        {
            toValue = default!;
            return false;
        }
    }

    /// <summary>
    /// cast object value to target Type
    /// <para>The Type conversion process will use method <see cref="TypeConverter.ConvertTo(object?, Type)"/></para>
    /// </summary>
    /// <typeparam fieldName="TTo"></typeparam>
    /// <param fieldName="value">object value</param>
    /// <returns>cast success</returns>
    public static To ConvertTo<To>(
#if NETCOREAPP3_1_OR_GREATER ||NET5_0_OR_GREATER||NETSTANDARD2_1_OR_GREATER
        [NotNull]
#endif

        this object? value, TypeConverter? converter = null)
    {
        if (value is null)
        {
            throw new InvalidCastException("value is null");
        }

        if (value is To target)
        {
            return target;
        }

        if (TypeConvertExtensions.ConvertTo(value, typeof(To), converter) is To toValue)
        {
            return toValue;
        }

        var msg = $"can not convert {value} to {ReflectionExtensions.GetExplicitName(typeof(To))}";

        throw new System.InvalidCastException(msg);
    }

    /// <summary>
    /// ConvertTo the specified to type.
    /// </summary>
    /// <param name="object">The object.</param>
    /// <param name="toType">ConvertTo type.</param>
    /// <param name="converter">The converter.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidCastException">
    /// value is null
    /// or
    /// can not convert {@object ?? "null"} to {ReflectionExtensions.GetTypeName(toType)}
    /// </exception>
    /// <exception cref="System.ArgumentNullException">toType</exception>
    public static object? ConvertTo(
#if NETCOREAPP3_1_OR_GREATER ||NET5_0_OR_GREATER||NETSTANDARD2_1_OR_GREATER
        [NotNull]
#endif
        this object? @object,
        Type toType,
        TypeConverter? converter = null
    )
    {
        if (@object is null)
        {
            throw new InvalidCastException("value is null");
        }

        if (toType is null)
        {
            throw new ArgumentNullException(nameof(toType));
        }

        Type currentType = @object.GetType();

        if (toType.IsAssignableFrom(currentType))
        {
            return @object;
        }

        const string format = "{0}=>{1}";

        string key = string.Format(format, currentType.FullName, toType.FullName);

        if (typeConverters.TryGetValue(key, out TypeConverter? typeConverter) == false)
        {
            _ = converter ??= TypeDescriptor.GetConverter(currentType);
            typeConverters[key] = typeConverter = converter;
        }

        if (typeConverter.CanConvertTo(currentType))
        {
            object? convertValue = typeConverter.ConvertTo(@object, toType);

            return convertValue;
        }

        throw new System.InvalidCastException(
            $"can not convert {@object ?? "null"} to {ReflectionExtensions.GetExplicitName(toType)}"
        );
    }

    /// <summary>
    /// cast object value to target Type
    /// </summary>
    /// <typeparam fieldName="TTo"></typeparam>
    /// <param fieldName="value">object value</param>
    /// <returns>cast success</returns>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TTo ConvertTo<From, TTo>(
#if NETCOREAPP3_1_OR_GREATER ||NET5_0_OR_GREATER||NETSTANDARD2_1_OR_GREATER
        [NotNull]
#endif
        this From? value,
        Func<From, TTo> lambdaConverter
    )
    {
        return value is null
            ? throw new InvalidCastException("value is null")
            : value is TTo target
                ? target
                : TypeConvertExtensions.ConvertTo<TTo>(
                    value,
                    lambdaConverter is null
                        ? null
                        : new TypeConvertGeneric<From, TTo>(lambdaConverter, null)
                );
    }

    /// <summary>
    /// Appends the converter.
    /// </summary>
    /// <typeparam name="TFrom">The type of from.</typeparam>
    /// <typeparam name="TTo">The type of to.</typeparam>
    /// <param name="lambdaToConverter">The lambda converter.</param>
    /// <param name="labmbdaFromConverter"></param>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void AppendConverter<TFrom, TTo>(
        Func<TFrom, TTo> lambdaToConverter,
        Func<TTo, TFrom>? labmbdaFromConverter = null
    )
    {
        Type targetType = typeof(TTo);
        Type currentType = typeof(TFrom);

        string key = $"{currentType.FullName}=>{targetType.FullName}";

        typeConverters[key] = new TypeConvertGeneric<TFrom, TTo>(
            lambdaToConverter,
            labmbdaFromConverter
        );
    }

    /// <summary>
    /// target type converter
    /// </summary>
    /// <typeparam name="From"></typeparam>
    /// <typeparam name="To"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TypeConvertGeneric<From, To> : TypeConverter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type fromType = typeof(From);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Type toType = typeof(To);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<From, To>? labmbdaToConverter;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<To, From>? labmbdaFromConverter;

        /// <summary>
        /// create a new <see cref="TypeConvertGeneric{From, To}"/>
        /// </summary>
        /// <param name="labmbdaFromConverter"></param>
        /// <param name="labmbdaToConverter"></param>
        public TypeConvertGeneric(
            Func<From, To>? labmbdaToConverter,
            Func<To, From>? labmbdaFromConverter
        )
        {
            this.labmbdaFromConverter = labmbdaFromConverter;
            this.labmbdaToConverter = labmbdaToConverter;
        }

        /// <summary>
        /// can convert from
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type? sourceType)
        {
            return sourceType == toType ? true : base.CanConvertFrom(context, sourceType!);
        }

        /// <summary>
        /// convert from
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value
        )
        {
            if (value is From from)
            {
                return from;
            }

            if (value is To toValue && labmbdaFromConverter != null)
            {
                From? fromValue = labmbdaFromConverter!(toValue);
                return fromValue;
            }

            return base.ConvertFrom(context, culture, value!);
        }

        /// <summary>
        /// can convert to
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == fromType ? true : base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// convert to
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type? destinationType
        )
        {
            if (value is To to)
            {
                return to;
            }

            if (destinationType == toType && value is From fromValue)
            {
                To? toValue = labmbdaToConverter!(fromValue);

                return toValue;
            }

            return base.ConvertTo(context, culture, value, destinationType!);
        }
    }
}
