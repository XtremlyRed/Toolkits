using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Converter;

/// <summary>
/// enumerable converters
/// </summary>
public static class EnumerableConverters
{
    /// <summary>
    /// determine whether the object is not null or empty
    /// </summary>
    public static EnumerableIsNotNullOrEmptyConverter IsNotNullOrEmpty = new EnumerableIsNotNullOrEmptyConverter();

    /// <summary>
    /// determine whether the object is null or empty
    /// </summary>
    public static EnumerableIsNullOrEmptyConverter IsNullOrEmpty = new EnumerableIsNullOrEmptyConverter();

    /// <summary>
    /// get enumerable count
    /// </summary>
    public static EnumerableCountConverter Count = new EnumerableCountConverter();
}
