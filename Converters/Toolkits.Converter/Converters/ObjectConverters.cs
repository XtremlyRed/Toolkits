using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Converter;

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
