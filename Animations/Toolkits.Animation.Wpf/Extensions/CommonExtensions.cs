using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Animation;

internal static class CommonExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> loop)
    {
        if (source is null || loop is null)
        {
            return;
        }

        foreach (var item in source)
        {
            loop(item);
        }
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static double FromRange(
        this double value,
        double minValue = double.MinValue,
        double maxValue = double.MaxValue
    )
    {
        return value < minValue
            ? minValue
            : value > maxValue
                ? maxValue
                : value;
    }
}
