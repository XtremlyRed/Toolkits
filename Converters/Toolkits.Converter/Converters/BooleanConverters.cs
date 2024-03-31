using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits;

public static class BooleanConverters
{
    /// <summary>
    /// The boolean reverse
    /// </summary>
    public static BooleanReverseConverter Reverse = new BooleanReverseConverter();

#if ___WPF___

    /// <summary>
    /// The boolean reverse
    /// </summary>
    public static BooleanToVisibilityReverseConverter ToVisibilityReverse =
        new BooleanToVisibilityReverseConverter();

    /// <summary>
    /// The boolean reverse
    /// </summary>
    public static BooleanToVisibilityConverter ToVisibility = new BooleanToVisibilityConverter();

#endif
}
