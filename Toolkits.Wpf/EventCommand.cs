//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Input;

//namespace Toolkits.Wpf;

///// <summary>
/////
///// </summary>
//public static class EventCommand
//{
//    /// <summary>
//    /// Sets the load.
//    /// </summary>
//    /// <param name="obj">The object.</param>
//    /// <param name="value">The value.</param>
//    public static void SetLoad(DependencyObject obj, object value)
//    {
//        obj.SetValue(LoadProperty, value);
//    }

//    /// <summary>
//    /// The load property
//    /// </summary>
//    public static readonly DependencyProperty LoadProperty = DependencyProperty.RegisterAttached(
//        "Load",
//        typeof(EventBinding),
//        typeof(EventCommand),
//        new PropertyMetadata(
//            null,
//            (s, e) =>
//            {
//                if (s is not FrameworkElement element)
//                {
//                    return;
//                }
//                if (e.OldValue is EventBinding oldBinding)
//                {
//                    element.Loaded -= Element_Loaded;
//                }
//                if (e.NewValue is EventBinding newbinding)
//                {
//                    element.Loaded += Element_Loaded;
//                }

//                static void Element_Loaded(object sender, RoutedEventArgs e)
//                {
//                    if (sender is not FrameworkElement @object)
//                    {
//                        return;
//                    }

//                    if (@object.GetValue(LoadProperty) is not EventBinding eventBinding)
//                    {
//                        return;
//                    }

//                    if (eventBinding.Multiple == false)
//                    {
//                        @object.Loaded -= Element_Loaded;
//                    }

//                    if (eventBinding.Command is null)
//                    {
//                        return;
//                    }

//                    if (eventBinding.Command.CanExecute(eventBinding.CommandParameter))
//                    {
//                        eventBinding.Command.Execute(eventBinding.CommandParameter);
//                    }
//                }
//            }
//        )
//    );
//}

///// <summary>
///// a class of <see cref="EventBinding"/>
///// </summary>
///// <seealso cref="System.Windows.DependencyObject" />
//public class EventBinding : DependencyObject
//{
//    /// <summary>
//    /// Gets or sets a value indicating whether this <see cref="EventBinding"/> is multiple.
//    /// </summary>
//    /// <value>
//    ///   <c>true</c> if multiple; otherwise, <c>false</c>.
//    /// </value>
//    public bool Multiple
//    {
//        get { return (bool)GetValue(MultipleProperty); }
//        set { SetValue(MultipleProperty, value); }
//    }

//    /// <summary>
//    /// The multiple property
//    /// </summary>
//    public static readonly DependencyProperty MultipleProperty = DependencyProperty.Register(
//        "Multiple",
//        typeof(bool),
//        typeof(EventBinding),
//        new PropertyMetadata(true)
//    );

//    /// <summary>
//    /// Gets or sets the command.
//    /// </summary>
//    /// <value>
//    /// The command.
//    /// </value>
//    public ICommand Command
//    {
//        get { return (ICommand)GetValue(CommandProperty); }
//        set { SetValue(CommandProperty, value); }
//    }

//    /// <summary>
//    /// The command property
//    /// </summary>
//    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
//        "Command",
//        typeof(ICommand),
//        typeof(EventBinding),
//        new PropertyMetadata(null)
//    );

//    /// <summary>
//    /// Gets or sets the command parameter.
//    /// </summary>
//    /// <value>
//    /// The command parameter.
//    /// </value>
//    public object CommandParameter
//    {
//        get { return (object)GetValue(CommandParameterProperty); }
//        set { SetValue(CommandParameterProperty, value); }
//    }

//    /// <summary>
//    /// The command parameter property
//    /// </summary>
//    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
//        "CommandParameter",
//        typeof(object),
//        typeof(EventBinding),
//        new PropertyMetadata(null)
//    );
//}
