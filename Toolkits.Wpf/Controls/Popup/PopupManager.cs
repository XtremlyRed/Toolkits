﻿using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Toolkits.Controls;
using Toolkits.Controls.Assist;
using Toolkits.Controls.PopupView;
using Toolkits.Wpf;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="PopupManager"/>
/// </summary>
public class PopupManager : IPopupManager
{
    /// <summary>
    /// all elements that can be popup container
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static readonly List<WeakReference> adornerDecorators = new();

    /// <summary>
    /// show message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/>,
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    public async Task ShowAsync(string message, string title, params string[] buttonContents)
    {
        ButtonContentsCheck(buttonContents);
        PopupContainerCheck(true, out var adornerDecorator);
        await PopupManagerAssist.InnerMessagePopup(adornerDecorator, message, title, buttonContents);
    }

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/>
    /// when using, there must be a popup container with the <code><see cref="PopupManager.IsMainContainerProperty"/></code>  attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns>the content of a clicked button</returns>
    public async Task<bool> ComfirmAsync(string message, string title, params string[] buttonContents)
    {
        return await ComfirmAsync(message, title, buttonContents, 0);
    }

    /// <summary>
    ///
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <param name="expectedClickIndex">expected click index of the (see parameter: <paramref name="buttonContents"/>).</param>
    /// <returns></returns>
    public async Task<bool> ComfirmAsync(string message, string title, string[] buttonContents, int expectedClickIndex = 0)
    {
        ButtonContentsCheck(buttonContents, expectedClickIndex);

        PopupContainerCheck(true, out var adornerDecorator);

        var clickConent = await PopupManagerAssist.InnerMessagePopup(adornerDecorator, message, title, buttonContents);

        return buttonContents[expectedClickIndex] == clickConent;
    }

    /// <summary>
    /// show message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    public async Task ShowAsyncIn(string containerName, string message, string title, params string[] buttonContents)
    {
        ButtonContentsCheck(buttonContents);
        PopupContainerCheck(containerName, out var adornerDecorator);
        await PopupManagerAssist.InnerMessagePopup(adornerDecorator, message, title, buttonContents);
    }

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    public async Task<bool> ComfirmAsyncIn(string containerName, string message, string title, params string[] buttonContents)
    {
        return await ComfirmAsyncIn(containerName, message, title, buttonContents, 0);
    }

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <param name="expectedClickIndex">expected click index of the (see parameter: <paramref name="buttonContents"/>).</param>
    /// <returns></returns>
    public async Task<bool> ComfirmAsyncIn(string containerName, string message, string title, string[] buttonContents, int expectedClickIndex = 0)
    {
        ButtonContentsCheck(buttonContents, expectedClickIndex);
        PopupContainerCheck(containerName, out var adornerDecorator);

        var clickContent = await PopupManagerAssist.InnerMessagePopup(adornerDecorator, message, title, buttonContents);

        return buttonContents[expectedClickIndex] == clickContent;
    }

    /// <summary>
    /// <para> popup view with <paramref name="parameters"/> from  main container</para>
    /// <para> when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true</para>
    /// <para> The <paramref name="view"/> type or the view model type bound to it must inherit from <see cref="IPopupAware"/> to obtain support for closing popup</para>
    /// </summary>
    /// <param name="view">view</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    public async Task<object> PopupAsync(object view, PopupParameters? parameters = null)
    {
        return await PopupAsync(() => view, parameters);
    }

    /// <summary>
    /// <para> popup view with <paramref name="parameters"/> from  main container</para>
    /// <para> when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true</para>
    /// <para> The <see cref="object"/> type or the view model type bound to it must inherit from <see cref="IPopupAware"/> to obtain support for closing popup</para>
    /// </summary>
    /// <param name="viewCreator">view creator</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    public async Task<object> PopupAsync(Func<object> viewCreator, PopupParameters? parameters = null)
    {
        PopupContainerCheck(true, out var adornerDecorator);

        return viewCreator is null
            ? throw new ArgumentNullException(nameof(viewCreator))
            : (await adornerDecorator.Dispatcher.InvokeAsync(() => viewCreator())) is not object view
                ? throw new ArgumentException("invalid visual")
                : await PopupManagerAssist.InnerContentPopup(adornerDecorator, view, parameters);
    }

    /// <summary>
    /// popup view with <paramref name="parameters"/> from <paramref name="containerName"/>
    /// <para> when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true</para>
    /// <para> The <paramref name="view"/> type or the view model type bound to it must inherit from <see cref="IPopupAware"/> to obtain support for closing popup</para>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="view">view</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    public async Task<object> PopupAsyncIn(string containerName, object view, PopupParameters? parameters = null)
    {
        return await PopupAsyncIn(containerName, () => view, parameters);
    }

    /// <summary>
    /// popup view with <paramref name="parameters"/> from <paramref name="containerName"/>
    /// <para> when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true</para>
    /// <para> The <see cref="object"/>  type or the view model type bound to it must inherit from <see cref="IPopupAware"/> to obtain support for closing popup</para>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="viewCreator">view creator</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    public async Task<object> PopupAsyncIn(string containerName, Func<object> viewCreator, PopupParameters? parameters = null)
    {
        PopupContainerCheck(containerName, out var adornerDecorator);

        return viewCreator is null
            ? throw new ArgumentNullException(nameof(viewCreator))
            : (await adornerDecorator.Dispatcher.InvokeAsync(() => viewCreator())) is not object view
                ? throw new ArgumentException("invalid visual")
                : await PopupManagerAssist.InnerContentPopup(adornerDecorator, view, parameters);
    }

    /// <summary>
    /// Buttons the contents check.
    /// </summary>
    /// <param name="buttonContents">The button contents.</param>
    /// <param name="expectedClickIndex">Expected index of the click.</param>
    /// <exception cref="System.ArgumentNullException">buttonContents - the display content of the buttons has not been configured</exception>
    private void ButtonContentsCheck(string[] buttonContents, int expectedClickIndex = 0)
    {
        if (buttonContents is null || buttonContents.Length == 0)
        {
            throw new ArgumentNullException(nameof(buttonContents), "the display content of the buttons has not been configured");
        }

        if (expectedClickIndex < 0 || expectedClickIndex >= buttonContents.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedClickIndex), "the expected click index of the button to be clicked is out of range");
        }
    }

    /// <summary>
    /// Popups the container check.
    /// </summary>
    /// <param name="searchMain">if set to <c>true</c> [search main].</param>
    /// <param name="adornerDecorator">The adorner decorator.</param>
    /// <exception cref="System.Exception">
    /// PopupManager: No popup container found.
    /// or
    /// PopupManager: the main container was not found or the main container name is empty
    /// </exception>
    private void PopupContainerCheck(bool searchMain, out AdornerDecorator adornerDecorator)
    {
        if (adornerDecorators.Count == 0)
        {
            throw new Exception("PopupManager: No popup container found.");
        }

        if (searchMain == false)
        {
            adornerDecorator = null!;
            return;
        }

        adornerDecorator = (
            adornerDecorators
                .FirstOrDefault(i => i.Target is AdornerDecorator adorner && adorner.Dispatcher.Invoke(() => GetIsMainContainer(adorner)) == true)
                ?.Target as AdornerDecorator
            ?? throw new Exception("PopupManager: the main container was not found or the main container name is empty")
        )!;
    }

    /// <summary>
    /// Popups the container check.
    /// </summary>
    /// <param name="containerName">Name of the container.</param>
    /// <param name="adornerDecorator">The adorner decorator.</param>
    private void PopupContainerCheck(string containerName, out AdornerDecorator adornerDecorator)
    {
        PopupContainerCheck(false, out _);

        adornerDecorator = (
            adornerDecorators
                .FirstOrDefault(i =>
                    i.Target is AdornerDecorator adorner && adorner.Dispatcher.Invoke(() => GetContainerName(adorner)) == containerName
                )
                ?.Target as AdornerDecorator
        )!;
    }

    #region  popup container name

    /// <summary>
    /// get popup container name
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static string? GetContainerName(AdornerDecorator adornerDecorator)
    {
        return adornerDecorator.GetValue(ContainerNameProperty)! as string;
    }

    /// <summary>
    /// set popup container name
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="popupContainerName"></param>
    public static void SetContainerName(AdornerDecorator adornerDecorator, string popupContainerName)
    {
        adornerDecorator.SetValue(ContainerNameProperty, popupContainerName);
    }

    /// <summary>
    /// popup container name
    /// </summary>
    public static readonly DependencyProperty ContainerNameProperty = DependencyProperty.RegisterAttached(
        "ContainerName",
        typeof(string),
        typeof(PopupManager),
        new FrameworkPropertyMetadata(
            Guid.NewGuid().ToString(),
            (s, e) =>
            {
                if (s is AdornerDecorator adornerDecorator)
                {
                    if (adornerDecorator.IsLoaded)
                    {
                        CreateAdorner(adornerDecorator, e.NewValue as string);
                    }
                    adornerDecorator.Loaded += Element_Loaded;
                    adornerDecorator.Unloaded += Element_Unloaded;
                }

                static void Element_Unloaded(object sender, RoutedEventArgs e)
                {
                    if (sender is AdornerDecorator adornerDecorator)
                    {
                        if (GetPopupAdornerContainer(adornerDecorator) is PopupAdorner popupBridge)
                        {
                            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(adornerDecorator);
                            adornerLayer?.Remove(popupBridge);
                        }

                        var item = adornerDecorators.FirstOrDefault(i => i.Target is AdornerDecorator adorner && adorner == adornerDecorator);
                        if (item != null)
                            adornerDecorators.Remove(item);
                    }
                }

                static void Element_Loaded(object sender, RoutedEventArgs e)
                {
                    if (sender is AdornerDecorator current)
                    {
                        if (adornerDecorators.Any(i => i.Target is AdornerDecorator adorner && adorner == current))
                        {
                            return;
                        }
                        CreateAdorner(current, GetContainerName(current));
                    }
                }

                static void CreateAdorner(AdornerDecorator adornerDecorator, string? name)
                {
                    adornerDecorators.Add(new WeakReference(adornerDecorator));

                    AdornerLayer adornerLayer = adornerDecorator.AdornerLayer;
                    if (adornerLayer == null)
                    {
                        return;
                    }

                    Type toastContainerType = GetToastContainerType(adornerDecorator);
                    Type messageContainerType = GetMessageContainerType(adornerDecorator);
                    Brush maskBrush = GetMaskBrush(adornerDecorator);

                    PopupAdorner popupAdorner = new(messageContainerType, toastContainerType, adornerDecorator, maskBrush);

                    adornerLayer.Add(popupAdorner);

                    SetPopupAdornerContainer(adornerDecorator, popupAdorner);
                }
            }
        )
    );

    #endregion


    #region IsMainContainer

    /// <summary>
    /// get popup container is main container
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool GetIsMainContainer(AdornerDecorator element)
    {
        return (bool)element.GetValue(IsMainContainerProperty);
    }

    /// <summary>
    /// set popup container is main container
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isMainContainer"></param>
    public static void SetIsMainContainer(AdornerDecorator element, bool isMainContainer)
    {
        element.SetValue(IsMainContainerProperty, isMainContainer);
    }

    /// <summary>
    /// is main container
    /// </summary>
    public static readonly DependencyProperty IsMainContainerProperty = DependencyProperty.RegisterAttached(
        "IsMainContainer",
        typeof(bool),
        typeof(PopupManager),
        new PropertyMetadata(false)
    );

    #endregion


    #region MessageContainerType

    /// <summary>
    /// get message container type
    /// This type must inherit <see cref=" PopupMessageViewBase"/>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Type GetMessageContainerType(AdornerDecorator element)
    {
        return (Type)element.GetValue(MessageContainerTypeProperty);
    }

    /// <summary>
    /// set message container type
    /// This type must inherit <see cref="PopupMessageViewBase"/>
    /// and must contain an parameterless constructor
    /// </summary>
    /// <param name="element"></param>
    /// <param name="messageContainerType"></param>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetMessageContainerType(AdornerDecorator element, Type messageContainerType)
    {
        if (messageContainerType is null)
        {
            throw new Exception("invalid message Container Type");
        }

        Type baseType = typeof(PopupMessageViewBase);
        if (baseType.IsAssignableFrom(messageContainerType) == false)
        {
            throw new Exception($"PopupManager: {messageContainerType.FullName} must inherit {baseType.FullName}");
        }

        element.SetValue(MessageContainerTypeProperty, messageContainerType);
    }

    /// <summary>
    /// message container type
    /// </summary>
    public static readonly DependencyProperty MessageContainerTypeProperty = DependencyProperty.RegisterAttached(
        "MessageContainerType",
        typeof(Type),
        typeof(PopupManager),
        new PropertyMetadata(typeof(MessageView))
    );

    #endregion


    #region ToastContainerType

    /// <summary>
    /// get toast container type
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>

    [EditorBrowsable(EditorBrowsableState.Never)]
    private static Type GetToastContainerType(AdornerDecorator adornerDecorator)
    {
        return (Type)adornerDecorator.GetValue(ToastContainerTypeProperty);
    }

    /// <summary>
    /// set toast container type
    /// and must contain an parameterless constructor
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="toastContainerType"></param>

    [EditorBrowsable(EditorBrowsableState.Never)]
    private static void SetToastContainerType(AdornerDecorator adornerDecorator, Type toastContainerType)
    {
        if (toastContainerType is null)
        {
            throw new Exception("invalid message Container Type");
        }

        Type baseType = typeof(PopupToastViewBase);
        if (baseType.IsAssignableFrom(toastContainerType) == false)
        {
            throw new Exception($"PopupManager: {toastContainerType.FullName} must inherit {baseType.FullName}");
        }

        adornerDecorator.SetValue(ToastContainerTypeProperty, toastContainerType);
    }

    /// <summary>
    /// toast container type
    /// This type must inherit <see cref=" PopupToastViewBase"/>
    /// </summary>
    public static readonly DependencyProperty ToastContainerTypeProperty = DependencyProperty.RegisterAttached(
        "ToastContainerType",
        typeof(Type),
        typeof(PopupManager),
        new PropertyMetadata(typeof(ToastView))
    );

    #endregion


    #region hide base function

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj"> The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    ///  Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString()
    {
        return base.ToString();
    }

    #endregion


    #region container

    /// <summary>
    /// get mask maskBrush of popup container
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    internal static PopupAdorner GetPopupAdornerContainer(DependencyObject adornerDecorator)
    {
        return (PopupAdorner)adornerDecorator.GetValue(PopupAdornerContainerProperty);
    }

    /// <summary>
    /// set mask maskBrush of popup container
    /// </summary>
    /// <param name="element"></param>
    /// <param name="grid"></param>
    internal static void SetPopupAdornerContainer(DependencyObject element, PopupAdorner grid)
    {
        element.SetValue(PopupAdornerContainerProperty, grid);
    }

    /// <summary>
    /// mask maskBrush of popup container
    /// </summary>
    internal static readonly DependencyProperty PopupAdornerContainerProperty = DependencyProperty.RegisterAttached(
        "PopupAdornerContainer",
        typeof(PopupAdorner),
        typeof(PopupManager),
        new PropertyMetadata(null)
    );

    #endregion


    #region   theme



    /// <summary>
    /// get mask maskBrush of popup container
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static Brush GetMaskBrush(AdornerDecorator adornerDecorator)
    {
        return (Brush)adornerDecorator.GetValue(MaskBrushProperty);
    }

    /// <summary>
    /// set mask maskBrush of popup container
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="maskBrush"></param>
    public static void SetMaskBrush(AdornerDecorator adornerDecorator, Brush maskBrush)
    {
        adornerDecorator.SetValue(MaskBrushProperty, maskBrush);
    }

    /// <summary>
    /// mask maskBrush of popup container
    /// </summary>
    public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.RegisterAttached(
        "MaskBrush",
        typeof(Brush),
        typeof(PopupManager),
        new PropertyMetadata(
            new BrushConverter().ConvertFromString("#40000000"),
            (s, e) =>
            {
                ThemeDataContext.themeDataContext.MaskBrush = e.NewValue as Brush;
            }
        )
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static Brush GetForeground(AdornerDecorator adornerDecorator)
    {
        return (Brush)adornerDecorator.GetValue(ForegroundProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetForeground(AdornerDecorator adornerDecorator, Brush value)
    {
        adornerDecorator.SetValue(ForegroundProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
        "Foreground",
        typeof(Brush),
        typeof(PopupManager),
        new PropertyMetadata(
            new BrushConverter().ConvertFromString("#000000"),
            (s, e) =>
            {
                ThemeDataContext.themeDataContext.Foreground = e.NewValue as Brush;
            }
        )
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static Brush GetBackground(AdornerDecorator adornerDecorator)
    {
        return (Brush)adornerDecorator.GetValue(BackgroundProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetBackground(AdornerDecorator adornerDecorator, Brush value)
    {
        adornerDecorator.SetValue(BackgroundProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
        "Background",
        typeof(Brush),
        typeof(PopupManager),
        new PropertyMetadata(
            new BrushConverter().ConvertFromString("#fafafa"),
            (s, e) =>
            {
                ThemeDataContext.themeDataContext.Background = e.NewValue as Brush;
            }
        )
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static Brush GetBorderBrush(AdornerDecorator adornerDecorator)
    {
        return (Brush)adornerDecorator.GetValue(BorderBrushProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetBorderBrush(AdornerDecorator adornerDecorator, Brush value)
    {
        adornerDecorator.SetValue(BorderBrushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached(
        "BorderBrush",
        typeof(Brush),
        typeof(PopupManager),
        new PropertyMetadata(
            new BrushConverter().ConvertFromString("#8f8f8f"),
            (s, e) =>
            {
                ThemeDataContext.themeDataContext.BorderBrush = e.NewValue as Brush;
            }
        )
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <returns></returns>
    public static Brush GetOperationAreaBrush(AdornerDecorator adornerDecorator)
    {
        return (Brush)adornerDecorator.GetValue(OperationAreaBrushProperty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="adornerDecorator"></param>
    /// <param name="value"></param>
    public static void SetOperationAreaBrush(AdornerDecorator adornerDecorator, Brush value)
    {
        adornerDecorator.SetValue(OperationAreaBrushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly DependencyProperty OperationAreaBrushProperty = DependencyProperty.RegisterAttached(
        "OperationAreaBrush",
        typeof(Brush),
        typeof(PopupManager),
        new PropertyMetadata(
            new BrushConverter().ConvertFromString("#eaeaea"),
            (s, e) =>
            {
                ThemeDataContext.themeDataContext.OperationAreaBrush = e.NewValue as Brush;
            }
        )
    );

    //internal static readonly SolidColorBrush DefaultForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
    //internal static readonly SolidColorBrush DefaultBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fafafa"));
    //internal static readonly SolidColorBrush DefaultBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8f8f8f"));
    //internal static readonly SolidColorBrush DefaultOperationAreaBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eaeaea"));


    /// <summary>
    /// Sets the theme brush.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetPopupTheme(DependencyObject obj, PopupTheme value)
    {
        obj.SetValue(PopupThemeProperty, value);
    }

    /// <summary>
    /// The theme brush property
    /// </summary>
    public static readonly DependencyProperty PopupThemeProperty = DependencyProperty.RegisterAttached(
        "PopupTheme",
        typeof(PopupTheme),
        typeof(PopupManager),
        new PropertyMetadata(
            null,
            (s, e) => {
                //   ThemeDataContext.themeDataContext.ChangedTheme(e.NewValue as PopupTheme);
            }
        )
    );

    #endregion
}
