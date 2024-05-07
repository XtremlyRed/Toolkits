using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static System.Reflection.BindingFlags;

namespace Toolkits.Animation;

/// <summary>
/// a class of <see cref="AnimationDeclareGroup"/>
/// </summary>
/// <seealso cref="Toolkits.Animation.AnimationDeclareBase" />
[DefaultProperty(nameof(Children))]
[ContentProperty(nameof(Children))]
[DefaultMember(nameof(Children))]
public class AnimationDeclareGroup : AnimationDeclareBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationDeclareGroup"/> class.
    /// </summary>
    public AnimationDeclareGroup()
    {
        SetCurrentValue(AnimationDeclaresProperty, new FreezableCollection<AnimationDeclare>());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal override AnimationTimeline CreateAnimation(FrameworkElement element, out object propertyOwner)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  animation declares.
    /// </summary>
    public FreezableCollection<AnimationDeclare> Children
    {
        get { return (FreezableCollection<AnimationDeclare>)GetValue(AnimationDeclaresProperty); }
        set { SetValue(AnimationDeclaresProperty, value); }
    }

    /// <summary>
    ///  animation declares.
    /// </summary>
    public static readonly DependencyProperty AnimationDeclaresProperty = DependencyProperty.Register(
        nameof(Children),
        typeof(FreezableCollection<AnimationDeclare>),
        typeof(AnimationDeclareGroup),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// animation play.
    /// </summary>
    public bool? Play
    {
        get { return (bool?)GetValue(PlayProperty); }
        set { SetValue(PlayProperty, value); }
    }

    /// <summary>
    /// animation play.
    /// </summary>
    public static readonly DependencyProperty PlayProperty = DependencyProperty.Register(
        "Play",
        typeof(bool?),
        typeof(AnimationDeclareGroup),
        new FrameworkPropertyMetadata(
            null,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (s, e) =>
            {
                if (e.NewValue is not true || s is not AnimationDeclareGroup declareGroup)
                {
                    return;
                }

                declareGroup.SetCurrentValue(PlayProperty, false);

                var exp = BindingOperations.GetBindingExpression(declareGroup, PlayProperty);
                exp?.UpdateSource();

                if (declareGroup.Children is null || declareGroup.Children.Count <= 0)
                {
                    return;
                }

                foreach (var item in declareGroup.Children)
                {
                    if (item is not null)
                    {
                        item.Play = true;
                    }
                }
            },
            null,
            true,
            System.Windows.Data.UpdateSourceTrigger.Explicit
        )
    );
}
