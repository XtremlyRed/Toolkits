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
public static class StringConverters
{
    /// <summary>
    /// determine whether the string is null or empty
    /// </summary>
    public static StringIsNullOrEmptyConverter IsNullOrEmpty = new StringIsNullOrEmptyConverter();

    /// <summary>
    /// determine whether the string is null or white space
    /// </summary>
    public static StringIsNullOrWhiteSpaceConverter IsNullOrWhiteSpace = new StringIsNullOrWhiteSpaceConverter();

    /// <summary>
    ///determine whether the string is not null or empty
    /// </summary>
    public static StringIsNotNullOrEmptyConverter IsNotNullOrEmpty = new StringIsNotNullOrEmptyConverter();

    /// <summary>
    ///  determine whether the string is not  null or white space
    /// </summary>
    public static StringIsNotNullOrWhiteSpaceConverter IsNotNullOrWhiteSpace = new StringIsNotNullOrWhiteSpaceConverter();

    /// <summary>
    /// get string length
    /// </summary>
    public static StringLengthConverter Length = new StringLengthConverter();
}
