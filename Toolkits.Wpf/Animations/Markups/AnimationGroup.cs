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

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="AnimationGroup"/>
/// </summary>
/// <seealso cref="Toolkits.Wpf.AnimationBase" />
[DefaultProperty(nameof(Children))]
[ContentProperty(nameof(Children))]
[DefaultMember(nameof(Children))]
public class AnimationGroup : AnimationBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationGroup"/> class.
    /// </summary>
    public AnimationGroup()
    {
        SetCurrentValue(AnimationDeclaresProperty, new FreezableCollection<AnimationBase>());
    }

    /// <summary>
    ///  animation declares.
    /// </summary>
    public FreezableCollection<AnimationBase> Children
    {
        get { return (FreezableCollection<AnimationBase>)GetValue(AnimationDeclaresProperty); }
        set { SetValue(AnimationDeclaresProperty, value); }
    }

    /// <summary>
    ///  animation declares.
    /// </summary>
    public static readonly DependencyProperty AnimationDeclaresProperty = DependencyProperty.Register(
        nameof(Children),
        typeof(FreezableCollection<AnimationBase>),
        typeof(AnimationGroup),
        new PropertyMetadata(null)
    );

    //protected override void AnimationPlay()
    //{
    //    this.SetCurrentValue(PlayProperty, false);

    //    var exp = BindingOperations.GetBindingExpression(this, PlayProperty);
    //    exp?.UpdateSource();

    //    if (this.Children is null || this.Children.Count <= 0)
    //    {
    //        return;
    //    }

    //    foreach (var item in this.Children)
    //    {
    //        if (item is not null)
    //        {
    //            item.Play = true;
    //        }
    //    }
    //}
}
