using System;
using System.Collections.Generic;
using System.Linq;
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
/// media converter
/// </summary>
public static class MediaConverters
{
    /// <summary>
    /// brush convert from string
    /// </summary>
    public static BrushStringConverter BrushFromString = new BrushStringConverter();

    /// <summary>
    /// color convert from string
    /// </summary>
    public static ColorStringConverter ColorFromString = new ColorStringConverter();
}
