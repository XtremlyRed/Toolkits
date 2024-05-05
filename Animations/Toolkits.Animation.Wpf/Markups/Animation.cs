using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Toolkits.Animation;

public static class Animation
{
    public static FadeDeclareExtension GetFade(DependencyObject obj)
    {
        return (FadeDeclareExtension)obj.GetValue(FadeProperty);
    }

    public static void SetFade(DependencyObject obj, FadeDeclareExtension value)
    {
        obj.SetValue(FadeProperty, value);
    }

    public static readonly DependencyProperty FadeProperty = DependencyProperty.RegisterAttached(
        "Fade",
        typeof(FadeDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static RotateDeclareExtension GetRotate(DependencyObject obj)
    {
        return (RotateDeclareExtension)obj.GetValue(RotateProperty);
    }

    public static void SetRotate(DependencyObject obj, RotateDeclareExtension value)
    {
        obj.SetValue(RotateProperty, value);
    }

    public static readonly DependencyProperty RotateProperty = DependencyProperty.RegisterAttached(
        "Rotate",
        typeof(RotateDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static SlideXDeclareExtension GetSlideX(DependencyObject obj)
    {
        return (SlideXDeclareExtension)obj.GetValue(SlideXProperty);
    }

    public static void SetSlideX(DependencyObject obj, SlideXDeclareExtension value)
    {
        obj.SetValue(SlideXProperty, value);
    }

    public static readonly DependencyProperty SlideXProperty = DependencyProperty.RegisterAttached(
        "SlideX",
        typeof(SlideXDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static SlideYDeclareExtension GetSlideY(DependencyObject obj)
    {
        return (SlideYDeclareExtension)obj.GetValue(SlideYProperty);
    }

    public static void SetSlideY(DependencyObject obj, SlideYDeclareExtension value)
    {
        obj.SetValue(SlideYProperty, value);
    }

    public static readonly DependencyProperty SlideYProperty = DependencyProperty.RegisterAttached(
        "SlideY",
        typeof(SlideYDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static ScaleXDeclareExtension GetScaleX(DependencyObject obj)
    {
        return (ScaleXDeclareExtension)obj.GetValue(ScaleXProperty);
    }

    public static void SetScaleX(DependencyObject obj, ScaleXDeclareExtension value)
    {
        obj.SetValue(ScaleXProperty, value);
    }

    public static readonly DependencyProperty ScaleXProperty = DependencyProperty.RegisterAttached(
        "ScaleX",
        typeof(ScaleXDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static ScaleYDeclareExtension GetScaleY(DependencyObject obj)
    {
        return (ScaleYDeclareExtension)obj.GetValue(ScaleYProperty);
    }

    public static void SetScaleY(DependencyObject obj, ScaleYDeclareExtension value)
    {
        obj.SetValue(ScaleYProperty, value);
    }

    public static readonly DependencyProperty ScaleYProperty = DependencyProperty.RegisterAttached(
        "ScaleY",
        typeof(ScaleYDeclareExtension),
        typeof(Animation),
        new PropertyMetadata(null)
    );

    public static bool? GetPlay(DependencyObject obj)
    {
        return (bool?)obj.GetValue(PlayProperty);
    }

    public static void SetPlay(DependencyObject obj, bool? value)
    {
        obj.SetValue(PlayProperty, value);
    }

    public static readonly DependencyProperty PlayProperty = DependencyProperty.RegisterAttached(
        "Play",
        typeof(bool?),
        typeof(Animation),
        new PropertyMetadata(
            null,
            (s, e) =>
            {
                if (s is FrameworkElement ffe)
                {
                    AnimationExtensions
                        .GetAnimations(ffe)
                        .Where(i => i.EventMode == EventMode.None)
                        .ForEach(i => i.Invoke());
                }
            }
        )
    );
}
