namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NotEqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class NotEqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="NotEqualConverter"/>
    /// </summary>
    public NotEqualConverter()
        : base(CompareMode.NotEqual) { }
}
