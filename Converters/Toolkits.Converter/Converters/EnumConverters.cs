using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Converter;

/// <summary>
/// string converters
/// </summary>
public static class EnumConverters
{
    /// <summary>
    /// The description
    /// </summary>
    public static EnumDescriptionConverter Description = new();

    /// <summary>
    /// The display name
    /// </summary>
    public static EnumDisplayNameConverter DisplayName = new();
}
