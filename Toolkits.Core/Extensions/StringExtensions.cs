using System.ComponentModel;

namespace Toolkits.Core;

/// <summary>
/// string extensions
/// </summary>
/// 2023/12/19 15:22
public static class StringExtensions
{
    /// <summary>
    /// Check if the string is <see langword="null"/> or a whitespace character
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// If the current string <paramref name="value"/> is <see langword="null"/> or a blank character, use <paramref name="defaultValue"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string WhenIsNullOrWhiteSpaceUse(this string? value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value!;
    }

    /// <summary>
    /// If the current string <paramref name="value"/> is <see langword="null"/> or a blank character, use <paramref name="exception"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string WhenIsNullOrWhiteSpaceUse(this string? value, Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        if (string.IsNullOrWhiteSpace(value))
        {
            throw exception;
        }

        return value!;
    }

    /// <summary>
    /// Check if the string is not <see langword="null"/> or a whitespace character
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNotNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) == false;
    }

    /// <summary>
    /// If the current string <paramref name="value" />is <see langword="null"/> or empty, use <paramref name="defaultValue"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string WhenIsNullOrEmptyUse(this string? value, string defaultValue)
    {
        return string.IsNullOrEmpty(value) ? defaultValue : value!;
    }

    /// <summary>
    /// If the current string <paramref name="value"/> is <see langword="null"/> or empty, use <paramref name="exception"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static string WhenIsNullOrEmptyUse(this string? value, Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        if (string.IsNullOrEmpty(value))
        {
            throw exception;
        }

        return value!;
    }

    /// <summary>
    /// Check if the string is <see langword="null"/>or empty
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Check if the string is not <see langword="null"/> or empty
    /// </summary>
    /// <param name="value">The value.</param>
    public static bool IsNotNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value) == false;
    }

    /// <summary>
    /// Joins the specified interval symbol.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="intervalSymbol">The interval symbol.</param>
    /// <returns></returns>
    /// 2023/12/19 15:23
    public static string Join<T>(this IEnumerable<T> source, string intervalSymbol = ",")
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = intervalSymbol ?? throw new ArgumentNullException(nameof(intervalSymbol));

        return string.Join(intervalSymbol, source);
    }

    /// <summary>
    /// Joins the specified selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="selector">The selector.</param>
    /// <param name="intervalSymbol">The interval symbol.</param>
    /// <returns></returns>
    /// 2023/12/19 15:24
    /// <exception cref="System.ArgumentNullException">
    /// source
    /// or
    /// selector
    /// or
    /// intervalSymbol
    /// </exception>
    public static string Join<T>(
        this IEnumerable<T> source,
        Func<T, string> selector,
        string intervalSymbol = ","
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = selector ?? throw new ArgumentNullException(nameof(selector));
        _ = intervalSymbol ?? throw new ArgumentNullException(nameof(intervalSymbol));

        return string.Join(intervalSymbol, source.Select(selector));
    }
}
