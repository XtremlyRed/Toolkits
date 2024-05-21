using System.Windows;
using System.Windows.Controls;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="GridExtensions"/>
/// </summary>
public static class GridExtensions
{
    private static GridLength StartLength = new GridLength(1, GridUnitType.Star);
    private static char[] splitChars = new char[] { ',', ';', ' ' };

    /// <summary>
    /// Gets the row definitions.
    /// </summary>
    /// <param name="grid">The object.</param>
    /// <returns></returns>
    public static string GetRowDefinitions(Grid grid)
    {
        return (string)grid.GetValue(RowDefinitionsProperty);
    }

    /// <summary>
    /// Sets the row definitions.
    /// </summary>
    /// <param name="grid">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetRowDefinitions(Grid grid, string value)
    {
        grid.SetValue(RowDefinitionsProperty, value);
    }

    /// <summary>
    /// The row definitions property
    /// </summary>
    public static readonly DependencyProperty RowDefinitionsProperty = DependencyProperty.RegisterAttached(
        "RowDefinitions",
        typeof(string),
        typeof(GridExtensions),
        new PropertyMetadata(
            "*",
            (s, e) =>
            {
                if (s is not Grid grid)
                {
                    return;
                }

                grid.RowDefinitions.Clear();

                if (e.NewValue is not string @string)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = StartLength });
                    return;
                }

                var strings = @string.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                if (strings is null || strings.Length == 0)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = StartLength });
                    return;
                }

                var lengths = ParseString(strings).ToArray();

                for (int i = 0; i < lengths.Length; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = lengths[i] });
                }
            }
        )
    );

    /// <summary>
    /// Gets the column definitions.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static string GetColumnDefinitions(Grid obj)
    {
        return (string)obj.GetValue(ColumnDefinitionsProperty);
    }

    /// <summary>
    /// Sets the column definitions.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetColumnDefinitions(Grid obj, string value)
    {
        obj.SetValue(ColumnDefinitionsProperty, value);
    }

    /// <summary>
    /// The column definitions property
    /// </summary>
    public static readonly DependencyProperty ColumnDefinitionsProperty = DependencyProperty.RegisterAttached(
        "ColumnDefinitions",
        typeof(string),
        typeof(GridExtensions),
        new PropertyMetadata(
            "*",
            (s, e) =>
            {
                if (s is not Grid grid)
                {
                    return;
                }

                grid.ColumnDefinitions.Clear();

                if (e.NewValue is not string @string)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = StartLength });
                    return;
                }

                var strings = @string.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                if (strings is null || strings.Length == 0)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = StartLength });
                    return;
                }

                var lengths = ParseString(strings).ToArray();

                for (int i = 0; i < lengths.Length; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = lengths[i] });
                }
            }
        )
    );

    private static IEnumerable<GridLength> ParseString(string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            var value = strings[i];
            if (string.Equals(value, "Auto", StringComparison.OrdinalIgnoreCase))
            {
                yield return GridLength.Auto;
            }

            if (int.TryParse(value, out var intValue))
            {
                yield return new GridLength(intValue, GridUnitType.Pixel);
            }

            if (value.Contains("*"))
            {
                var numString = value.Replace("*", string.Empty);
                if (int.TryParse(numString, out intValue) == false)
                {
                    yield return new GridLength(1, GridUnitType.Pixel);
                }

                yield return new GridLength(intValue, GridUnitType.Star);
            }
        }
    }
}
