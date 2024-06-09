namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="LessThanOrEqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class LessThanOrEqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="LessThanOrEqualConverter"/>
    /// </summary>
    public LessThanOrEqualConverter()
        : base(CompareMode.LessThanOrEqual) { }
}
