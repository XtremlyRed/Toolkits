using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Toolkits.Controls;

/// <summary>
/// The base class for all message popup views.
/// </summary>
public abstract class PopupMessageViewBase : UserControl
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private SemaphoreSlim? messageSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string[]? buttonContents;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? currentClickResult;

    /// <summary>
    /// Set pop-up information, title, and button content
    /// </summary>
    /// <param name="message">Popup message</param>
    /// <param name="title">Popup Title</param>
    /// <param name="buttonContents">Popup Button Contents</param>
    protected abstract void SetPopupMessageInfo(
        string message,
        string title,
        string[] buttonContents
    );

    internal void SetMessageInfo(string message, string title, string[] buttonContents)
    {
        this.buttonContents = buttonContents;
        SetPopupMessageInfo(message, title, buttonContents);
    }

    internal async Task<string>? WaitMessageResultAsync()
    {
        messageSemaphoreSlim?.Dispose();
        messageSemaphoreSlim = null;

        messageSemaphoreSlim = new(0, 1);
        await messageSemaphoreSlim.WaitAsync();

        return currentClickResult!;
    }

    /// <summary>
    /// Set the content of the currently clicked button,
    /// which must be set through the parameter <c>buttonContents</c> of method <see cref="SetPopupMessageInfo"/>
    /// </summary>
    /// <param name="currentClickContent"></param>
    protected void SetCurrentClickContent(string? currentClickContent)
    {
        if (currentClickContent is null)
        {
            throw new ArgumentNullException(nameof(currentClickContent));
        }

        if (buttonContents != null)
        {
            if (buttonContents.Contains(currentClickContent) == false)
            {
                throw new InvalidOperationException($"invalid parameter :{currentClickContent}");
            }
        }

        currentClickResult = currentClickContent;

        if (messageSemaphoreSlim!.CurrentCount == 0)
        {
            messageSemaphoreSlim.Release(1);
        }
    }
}
