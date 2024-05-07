using System.Windows;

namespace Toolkits.Core;

/// <summary>
/// a class of <see cref="IPopupManager"/>
/// </summary>
public interface IPopupManager
{
    /// <summary>
    /// show message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/>,
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    Task ShowAsync(string message, string title, params string[] buttonContents);

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/>
    /// when using, there must be a popup container with the <code><see cref="PopupManager.IsMainContainerProperty"/></code>  attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns>the content of a clicked button</returns>
    Task<bool> ComfirmAsync(string message, string title, params string[] buttonContents);

    /// <summary>
    ///
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <param name="expectedClickIndex">expected click index of the (see parameter: <paramref name="buttonContents"/>).</param>
    /// <returns></returns>
    Task<bool> ComfirmAsync(string message, string title, string[] buttonContents, int expectedClickIndex = 0);

    /// <summary>
    /// show message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    Task ShowAsyncIn(string containerName, string message, string title, params string[] buttonContents);

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <returns></returns>
    Task<bool> ComfirmAsyncIn(string containerName, string message, string title, params string[] buttonContents);

    /// <summary>
    /// comfirm message with <paramref name="message"/>,<paramref name="title"/>,<paramref name="buttonContents"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="message">the message content of the pop-up box</param>
    /// <param name="title">the title of the pop-up box</param>
    /// <param name="buttonContents">the button contents of the pop-up box</param>
    /// <param name="expectedClickIndex">expected click index of the (see parameter: <paramref name="buttonContents"/>).</param>
    /// <returns></returns>
    Task<bool> ComfirmAsyncIn(string containerName, string message, string title, string[] buttonContents, int expectedClickIndex = 0);

    /// <summary>
    /// popup view with <paramref name="parameters"/> from  main container
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="view">view</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>d
    Task<object> PopupAsync(object view, Parameters? parameters = null);

    /// <summary>
    /// popup view with <paramref name="parameters"/> from  main container
    /// when using, there must be a popup container with the <see cref="PopupManager.IsMainContainerProperty"/> attribute set to true
    /// </summary>
    /// <param name="viewCreator">view creator</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    Task<object> PopupAsync(Func<object> viewCreator, Parameters? parameters = null);

    /// <summary>
    /// popup view with <paramref name="parameters"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="view">view</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    Task<object> PopupAsyncIn(string containerName, object view, Parameters? parameters = null);

    /// <summary>
    /// popup view with <paramref name="parameters"/> from <paramref name="containerName"/>
    /// </summary>
    /// <param name="containerName">popup <paramref name="containerName"/></param>
    /// <param name="viewCreator">view creator</param>
    /// <param name="parameters">parameters</param>
    /// <returns></returns>
    Task<object> PopupAsyncIn(string containerName, Func<object> viewCreator, Parameters? parameters = null);
}
