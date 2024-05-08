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
/// object converters
/// </summary>
public static class ObjectConverters
{
    /// <summary>
    /// determine whether the object is null
    /// </summary>
    public static ObjectIsNullConverter IsNull = new ObjectIsNullConverter();

    /// <summary>
    /// determine whether the object is not null
    /// </summary>
    public static ObjectIsNotNullConverter IsNotNull = new ObjectIsNotNullConverter();
}
