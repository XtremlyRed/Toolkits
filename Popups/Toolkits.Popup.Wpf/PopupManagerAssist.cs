using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Toolkits.Popup.Assist;

[EditorBrowsable(EditorBrowsableState.Never)]
internal static class PopupManagerAssist
{
    private static readonly TimeSpan waitTimeSpan = TimeSpan.FromMilliseconds(350);
    internal const int MessageIndex = 2;
    internal const int ContentIndex = 1;
    internal const int ToastIndex = 0;

    internal static async Task<string> InnerMessagePopup(
        UIElement uIElement,
        string message,
        string title,
        string[] buttonContents
    )
    {
        return await await uIElement.Dispatcher.InvokeAsync(async () =>
        {
            PopupAdorner popupAdorner = PopupManager.GetPopupAdornerContainer(uIElement);

            popupAdorner.messageCounter++;

            await popupAdorner.DisplaysemaphoreSlims[MessageIndex].WaitAsync();

            popupAdorner.ShowContainer(popupAdorner.messageContainer, MessageIndex, waitTimeSpan);

            popupAdorner.messageView!.SetMessageInfo(message, title, buttonContents);

            string popupResult = await popupAdorner.WaitMessageResultAsync(MessageIndex);

            popupAdorner.HideContainer(
                popupAdorner.messageContainer,
                MessageIndex,
                waitTimeSpan,
                () =>
                {
                    popupAdorner.DisplaysemaphoreSlims[MessageIndex].Release();
                }
            );

            return popupResult;
        });
    }

    internal static async Task<object> InnerContentPopup(
        UIElement uIElement,
        object popupContent,
        Parameters? parameters = null
    )
    {
        return await await uIElement.Dispatcher.InvokeAsync(async () =>
        {
            PopupAdorner popupAdorner = PopupManager.GetPopupAdornerContainer(uIElement);

            popupAdorner.contentCounter++;

            await popupAdorner.DisplaysemaphoreSlims[ContentIndex].WaitAsync();

            popupAdorner.ShowContainer(popupAdorner.contentContainer, ContentIndex, waitTimeSpan);

            Action action = popupAdorner.SetContent(ContentIndex, popupContent, parameters);

            object popupResult = await popupAdorner.WaitContentReaultAsync(ContentIndex);

            popupAdorner.HideContainer(
                popupAdorner.contentContainer,
                ContentIndex,
                waitTimeSpan,
                () =>
                {
                    action?.Invoke();
                    popupAdorner.DisplaysemaphoreSlims[ContentIndex].Release();
                }
            );

            return popupResult;
        });
    }

    //internal static async Task InnerTipPopup(
    //    UIElement uieleMent,
    //    string title,
    //    string message,
    //    int displayTimeSpan_Ms,
    //    object[] parameters
    //)
    //{
    //    await await uieleMent.Dispatcher.InvokeAsync(async () =>
    //    {
    //        PopupAdorner popupAdorner = PopupManager.GetPopupAdornerContainer(uieleMent);
    //        popupAdorner.toastCounter++;
    //        await popupAdorner.DisplaysemaphoreSlims[ToastIndex].WaitAsync();
    //        popupAdorner.ShowContainer(popupAdorner.toastContainer, ToastIndex, waitTimeSpan);
    //        PopupToastViewBase? toastView =
    //            popupAdorner.toastContainerTypeConstructor?.Invoke(null) as PopupToastViewBase;
    //        if (toastView != null)
    //        {
    //            toastView.ResetSize();
    //            toastView.HorizontalAlignment = HorizontalAlignment.Left;
    //            toastView.SetToastContent(title, message, displayTimeSpan_Ms, parameters);
    //            if (
    //                popupAdorner.toastContainer.Width is double.NaN
    //                || popupAdorner.toastContainer.Width < 1
    //            )
    //            {
    //                double totalWidth =
    //                    toastView.Margin.Left + toastView.Margin.Right + toastView.Width;
    //                popupAdorner.toastContainer.Width = totalWidth * 2 + 50;
    //                popupAdorner.toastContainer.Margin = new Thickness(0, 0, -(totalWidth + 50), 0);
    //            }
    //            popupAdorner.toastContainer.Children.Add(toastView);
    //            toastView.CloseCallback = () =>
    //            {
    //                popupAdorner.toastContainer.Children.Remove(toastView);
    //                popupAdorner.DisplaysemaphoreSlims[ToastIndex].Release();
    //                popupAdorner.HideContainer(
    //                    popupAdorner.toastContainer,
    //                    ToastIndex,
    //                    waitTimeSpan,
    //                    null!
    //                );
    //            };
    //            return;
    //        }
    //        popupAdorner.DisplaysemaphoreSlims[ToastIndex].Release();
    //    });
    //}
}
