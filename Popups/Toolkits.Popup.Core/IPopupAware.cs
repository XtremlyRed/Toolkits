namespace Toolkits;

/// <summary>
/// request close popup event
/// </summary>
/// <param name="sender">sender</param>
/// <param name="args">args</param>
public delegate void RequestCloseEventHandler(object sender, object args);

/// <summary>
/// popup content aware to view close event
/// </summary>
public interface IPopupAware
{
    /// <summary>
    /// on popup closed
    /// </summary>
    void OnPopupClosed();

    /// <summary>
    /// on popup opened
    /// </summary>
    /// <param name="parameters"></param>
    void OnPopupOpened(Parameters? parameters);

    /// <summary>
    /// request close popup
    /// </summary>
    event RequestCloseEventHandler RequestCloseEvent;
}
