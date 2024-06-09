namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="EqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class EqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="EqualConverter"/>
    /// </summary>
    public EqualConverter()
        : base(CompareMode.Equal) { }
}
