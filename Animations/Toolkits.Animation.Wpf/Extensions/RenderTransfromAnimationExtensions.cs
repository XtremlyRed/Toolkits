using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Toolkits.Animation;

/// <summary>
///
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class RenderTransfromAnimationExtensions
{
    #region Transform

    /// <summary>
    /// The scale index
    /// </summary>
    public const int ScaleIndex = 0;

    /// <summary>
    /// The rotate index
    /// </summary>
    public const int RotateIndex = 1;

    /// <summary>
    /// The translate index
    /// </summary>
    public const int TranslateIndex = 2;

    /// <summary>
    /// Applies the transform group.
    /// </summary>
    /// <param name="frameworkElement">The framework element.</param>
    public static void ApplyTransformGroup(FrameworkElement frameworkElement)
    {
        frameworkElement.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

        Transform? exist = null;

        if (frameworkElement.RenderTransform is not TransformGroup transformGroup)
        {
            exist = frameworkElement.RenderTransform;
            frameworkElement.RenderTransform = transformGroup = new TransformGroup();
        }

        List<Transform> exists = new List<Transform>();

        if (transformGroup.Children.Count == 0 || transformGroup.Children[ScaleIndex] is not ScaleTransform)
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(ScaleIndex)!);

            Add(ScaleIndex, (exist as ScaleTransform) ?? exists.FirstOrDefault(i => i is not null and ScaleTransform) ?? new ScaleTransform(1, 1));
        }

        if (transformGroup.Children.Count < 2 || transformGroup.Children[RotateIndex] is not RotateTransform)
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(RotateIndex)!);
            Add(RotateIndex, (exist as RotateTransform) ?? exists.FirstOrDefault(i => i is not null and RotateTransform) ?? new RotateTransform(0));
        }

        if (transformGroup.Children.Count < 3 || transformGroup.Children[TranslateIndex] is not TranslateTransform)
        {
            exists.Add(transformGroup.Children.ElementAtOrDefault(TranslateIndex)!);
            Add(
                TranslateIndex,
                (exist as TranslateTransform) ?? exists.FirstOrDefault(i => i is not null and TranslateTransform) ?? new TranslateTransform(0, 0)
            );
        }

        exists.Add(exist!);

        exists
            .Where(i => i != null)
            .Where(i => i is not ScaleTransform)
            .Where(i => i is not RotateTransform)
            .Where(i => i is not TranslateTransform)
            .ForEach(transformGroup.Children.Add);

        void Add(int index, Transform transform)
        {
            if (transformGroup.Children.Count > index)
            {
                transformGroup.Children[index] = transform;
            }
            else
            {
                transformGroup.Children.Add(transform);
            }
        }
    }

    #endregion
}
