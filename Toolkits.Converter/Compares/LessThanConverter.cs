namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="LessThanConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class LessThanConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="LessThanConverter"/>
    /// </summary>
    public LessThanConverter()
        : base(CompareMode.LessThan) { }
}
