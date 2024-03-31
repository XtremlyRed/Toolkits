using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Toolkits.Popup.Assist;

internal class PopupAdorner : Adorner
{
    internal readonly Grid popupContainer;
    internal readonly Grid messageContainer = new() { Visibility = Visibility.Collapsed };
    internal readonly Grid contentContainer = new() { Visibility = Visibility.Collapsed };
    internal readonly StackPanel toastContainer =
        new()
        {
            Visibility = Visibility.Collapsed,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };
    internal readonly PopupMessageViewBase? messageView;

    internal readonly bool[] shown = new[] { false, false, false };
    internal readonly AsyncLocker[] DisplaysemaphoreSlims = new[]
    {
        new AsyncLocker(10, 10),
        new AsyncLocker(1, 1),
        new AsyncLocker(1, 1)
    };
    internal readonly AsyncLocker[] InteropsemaphoreSlims = new[]
    {
        new AsyncLocker(0, 1),
        new AsyncLocker(0, 1),
        new AsyncLocker(0, 1)
    };
    internal readonly System.Reflection.ConstructorInfo? toastContainerTypeConstructor;

    internal object? contentResult;
    internal object? toastResult;

    internal int messageCounter;
    internal int contentCounter;
    internal int toastCounter;

    public PopupAdorner(
        Type messageContainerType,
        Type toastContainerType,
        UIElement adornedElement,
        Brush maskBrush
    )
        : base(adornedElement)
    {
        System.Reflection.ConstructorInfo[] cs = messageContainerType.GetConstructors(
            BF.Instance | BF.Public | BF.NonPublic
        );
        System.Reflection.ConstructorInfo[] cs1 = toastContainerType.GetConstructors(
            BF.Instance | BF.Public | BF.NonPublic
        );

        toastContainerTypeConstructor = cs1.OrderBy(i => i.GetParameters().Length).FirstOrDefault();

        popupContainer = new Grid() { ClipToBounds = true };

        popupContainer.Children.Add(toastContainer);
        popupContainer.Children.Add(contentContainer);
        popupContainer.Children.Add(messageContainer);

        Panel.SetZIndex(toastContainer, 1 << 1);
        Panel.SetZIndex(contentContainer, 1 << 2);
        Panel.SetZIndex(messageContainer, 1 << 3);

        contentContainer.Background = maskBrush;
        messageContainer.Background = maskBrush;

        messageView =
            cs.OrderBy(i => i.GetParameters().Length).FirstOrDefault()?.Invoke(null)
            as PopupMessageViewBase;
        messageContainer.Children.Add(messageView);

        AddVisualChild(popupContainer);
    }

    internal Task<string> WaitMessageResultAsync(int messageIndex)
    {
        return messageView!.WaitMessageResultAsync()!;
    }

    internal async Task<object> WaitContentReaultAsync(int contentIndex)
    {
        try
        {
            await InteropsemaphoreSlims[contentIndex].WaitAsync();
            return contentResult!;
        }
        finally
        {
            contentResult = null;
        }
    }

    internal async Task<object> WaitToastReaultAsync(int toastIndex)
    {
        try
        {
            await InteropsemaphoreSlims[toastIndex].WaitAsync();
            return toastResult!;
        }
        finally
        {
            toastResult = null;
        }
    }

    #region override

    protected override int VisualChildrenCount => popupContainer != null ? 1 : 0;

    protected override Size ArrangeOverride(Size finalSize)
    {
        popupContainer?.Arrange(new Rect(finalSize));
        return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
        return index == 0 && popupContainer != null ? popupContainer : base.GetVisualChild(index);
    }

    #endregion
}
