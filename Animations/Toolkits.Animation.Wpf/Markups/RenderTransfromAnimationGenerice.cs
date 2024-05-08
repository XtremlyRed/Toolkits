using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using static Toolkits.Wpf.RenderTransfromAnimationExtensions;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="RenderTransfromAnimationDeclare"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class RenderTransfromAnimationDeclare : AnimationDeclareGeneric<double?>
{
    /// <summary>
    ///  animation property.
    /// </summary>
    protected abstract DependencyProperty Property { get; }

    /// <summary>
    ///  animation path.
    /// </summary>
    protected abstract string AnimationPath { get; }

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(element)
        };

        if (this.From.HasValue)
        {
            animation.From = this.From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = this.To.Value;
        }

        propertyOwner = element;

        return animation;
    }
}

///// <summary>
/////  a class of <see cref="FadeAnimation"/>
///// </summary>
///// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
//public class FadeAnimation : RenderTransfromAnimationDeclare
//{
//    /// <summary>
//    ///  animation property.
//    /// </summary>
//    protected override DependencyProperty Property => FrameworkElement.OpacityProperty;

//    /// <summary>
//    ///  animation path.
//    /// </summary>
//    protected override string AnimationPath => "(FrameworkElement.Opacity)";
//}

/// <summary>
/// a class of <see cref="RotateAnimation"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
public class RotateAnimation : RenderTransfromAnimationDeclare
{
    /// <summary>
    ///  animation property.
    /// </summary>
    protected override DependencyProperty Property => RotateTransform.AngleProperty;

    /// <summary>
    ///  animation path.
    /// </summary>
    protected override string AnimationPath =>
        string.Format("(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(RotateTransform.Angle)", RotateIndex);

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="propertyOwner">The property owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement element, out object propertyOwner)
    {
        ApplyTransformGroup(element);

        propertyOwner = ((TransformGroup)element.RenderTransform).Children[RotateIndex];

        return base.AnimationBuild(element, out _);
    }
}

/// <summary>
/// a class of <see cref="SlideXAnimation"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
public class SlideXAnimation : RenderTransfromAnimationDeclare
{
    /// <summary>
    ///
    /// </summary>
    public enum SlideXMode
    {
        /// <summary>
        ///   left mode
        /// </summary>
        Left,

        /// <summary>
        ///   right mode
        /// </summary>
        Right
    }

    /// <summary>
    /// slide mode.
    /// </summary>
    public SlideXMode SlideMode { get; set; }

    /// <summary>
    /// animation property.
    /// </summary>
    protected override DependencyProperty Property => TranslateTransform.XProperty;

    /// <summary>
    /// animation path.
    /// </summary>
    protected override string AnimationPath =>
        string.Format("(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.X)", TranslateIndex);

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement targetObject, out object owner)
    {
        ApplyTransformGroup(targetObject);

        owner = ((TransformGroup)targetObject.RenderTransform).Children[TranslateIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        int dir = SlideMode == SlideXMode.Left ? -1 : 1;

        animation.From = Math.Abs((From!.HasValue ? From.Value : targetObject.ActualWidth)) * dir;

        animation.To = (To!.HasValue ? To.Value : 0);

        return animation;
    }
}

/// <summary>
///  a class of <see cref="SlideYAnimation"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
public class SlideYAnimation : RenderTransfromAnimationDeclare
{
    /// <summary>
    ///  an enum of <see cref="SlideYMode"/>
    /// </summary>
    public enum SlideYMode
    {
        /// <summary>
        ///   top mode
        /// </summary>
        Top,

        /// <summary>
        ///   bottom mode
        /// </summary>
        Bottom
    }

    /// <summary>
    ///  slide mode.
    /// </summary>
    public SlideYMode SlideMode { get; set; }

    /// <summary>
    /// animation property.
    /// </summary>
    protected override DependencyProperty Property => TranslateTransform.YProperty;

    /// <summary>
    /// animation path.
    /// </summary>
    protected override string AnimationPath =>
        string.Format("(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(TranslateTransform.Y)", TranslateIndex);

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override AnimationTimeline AnimationBuild(FrameworkElement targetObject, out object owner)
    {
        ApplyTransformGroup(targetObject);

        owner = ((TransformGroup)targetObject.RenderTransform).Children[TranslateIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        int dir = SlideMode == SlideYMode.Top ? -1 : 1;

        animation.From = Math.Abs(From!.HasValue ? From.Value : targetObject.ActualHeight) * dir;

        animation.To = (To!.HasValue ? To.Value : 0);

        return animation;
    }
}

/// <summary>
///  a class of <see cref="ScaleXAnimatio"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
public class ScaleXAnimatio : RenderTransfromAnimationDeclare
{
    /// <summary>
    /// animation property.
    /// </summary>
    protected override DependencyProperty Property => ScaleTransform.ScaleXProperty;

    /// <summary>
    /// animation path.
    /// </summary>
    protected override string AnimationPath =>
        string.Format("(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleX)", ScaleIndex);

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    protected override AnimationTimeline AnimationBuild(FrameworkElement targetObject, out object owner)
    {
        ApplyTransformGroup(targetObject);

        owner = ((TransformGroup)targetObject.RenderTransform).Children[ScaleIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        if (this.From.HasValue)
        {
            animation.From = From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = To.Value;
        }

        return animation;
    }
}

/// <summary>
///  a class of <see cref="ScaleYAnimatio"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.RenderTransfromAnimationDeclare" />
public class ScaleYAnimatio : RenderTransfromAnimationDeclare
{
    /// <summary>
    /// animation property.
    /// </summary>
    protected override DependencyProperty Property => ScaleTransform.ScaleYProperty;

    /// <summary>
    /// animation path.
    /// </summary>
    protected override string AnimationPath =>
        string.Format("(FrameworkElement.RenderTransform).(TransformGroup.Children)[{0}].(ScaleTransform.ScaleY)", ScaleIndex);

    /// <summary>
    /// Creates the animation.
    /// </summary>
    /// <param name="targetObject">The target object.</param>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    protected override AnimationTimeline AnimationBuild(FrameworkElement targetObject, out object owner)
    {
        ApplyTransformGroup(targetObject);

        owner = ((TransformGroup)targetObject.RenderTransform).Children[ScaleIndex];

        var animation = new DoubleAnimation()
        {
            Duration = this.Duration,
            BeginTime = this.BeginTime,
            EasingFunction = GetEasingFunction(targetObject)
        };

        if (this.From.HasValue)
        {
            animation.From = From.Value;
        }
        if (this.To.HasValue)
        {
            animation.To = To.Value;
        }

        return animation;
    }
}
