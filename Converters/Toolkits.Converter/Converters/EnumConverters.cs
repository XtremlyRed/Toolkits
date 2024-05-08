using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

#if ___WPF___
namespace Toolkits.Wpf;

#endif
#if ___AVALONIA___
namespace Toolkits.Avalonia;

#endif
#if ___MAUI___
namespace Toolkits.Maui;

#endif

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
