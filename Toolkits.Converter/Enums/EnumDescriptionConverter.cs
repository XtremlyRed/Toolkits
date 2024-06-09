using System.ComponentModel;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="EnumDescriptionConverter"/>
/// </summary>
public class EnumDescriptionConverter : EnumConverter<DescriptionAttribute>
{
    /// <summary>
    /// display
    /// </summary>
    protected override Func<DescriptionAttribute?, string?> DisplaySelector => i => i?.Description;
}
