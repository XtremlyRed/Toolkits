using System.ComponentModel;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="EnumDisplayNameConverter"/>
/// </summary>
public class EnumDisplayNameConverter : EnumConverter<DisplayNameAttribute>
{
    /// <summary>
    /// display
    /// </summary>
    protected override Func<DisplayNameAttribute?, string?> DisplaySelector => i => i?.DisplayName;
}
