using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Converter;

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
