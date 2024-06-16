namespace Toolkits.Core;

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
