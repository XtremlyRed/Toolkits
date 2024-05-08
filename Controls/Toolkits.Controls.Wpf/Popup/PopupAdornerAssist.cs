using System;
using System.Windows;
using System.Windows.Media.Animation;
using Toolkits.Core;
using Toolkits.Wpf;

namespace Toolkits.Controls.Assist;

internal static class PopupAdornerAssist
{
    internal static Action SetContent(this PopupAdorner popupAdorner, int contentIndex, object popupView, Parameters? parameters = null)
    {
        var popupContent = popupView as FrameworkElement;

        if (popupContent is null)
        {
            throw new ArgumentException("popupView is not FrameworkElement");
        }

        Action? popupContentCloseCallback = () =>
        {
            popupContent.DataContextChanged -= PopupContent_DataContextChanged;
            if (popupContent is IPopupAware popupAware)
            {
                popupAware.RequestCloseEvent -= PopupAware_RequestCloseEvent;
            }
            if (popupContent.DataContext is IPopupAware popupAware1)
            {
                popupAware1.RequestCloseEvent -= PopupAware_RequestCloseEvent;
            }
            popupContentCloseCallback = null;
        };

        popupAdorner.contentContainer.Children.Clear();
        popupAdorner.contentContainer.Children.Add(popupContent);

        AwareCallback(popupContent);
        AwareCallback(popupContent.DataContext);

        popupContent.DataContextChanged += PopupContent_DataContextChanged;

        return popupContentCloseCallback;

        void PopupAware_RequestCloseEvent(object sender, object args)
        {
            popupAdorner.contentResult = args;
            popupAdorner.InteropsemaphoreSlims[contentIndex].Release();
        }
        void PopupContent_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IPopupAware popupAware)
            {
                popupAware.RequestCloseEvent -= PopupAware_RequestCloseEvent;
            }
            AwareCallback(e.NewValue);
        }

        void AwareCallback(object @object)
        {
            if (@object is not IPopupAware popupAware)
            {
                return;
            }

            popupAware.OnPopupOpened(parameters);
            popupAware.RequestCloseEvent += PopupAware_RequestCloseEvent;
        }
    }

    public static void ShowContainer(this PopupAdorner popupAdorner, FrameworkElement visual, int index, TimeSpan durationAnimation)
    {
        if (popupAdorner.shown[index])
        {
            return;
        }
        popupAdorner.shown[index] = true;

        DisplayVisual(visual, durationAnimation);
    }

    public static void HideContainer(
        this PopupAdorner popupAdorner,
        FrameworkElement visual,
        int index,
        TimeSpan durationAnimation,
        Action? action = null
    )
    {
        if (index == PopupManagerAssist.MessageIndex)
        {
            popupAdorner.messageCounter--;

            if (popupAdorner.shown[index] == false)
            {
                return;
            }

            if (popupAdorner.messageCounter != 0)
            {
                action?.Invoke();
                return;
            }

            popupAdorner.shown[index] = false;
        }
        else if (index == PopupManagerAssist.ContentIndex)
        {
            popupAdorner.contentCounter--;

            if (popupAdorner.shown[index] == false)
            {
                return;
            }

            if (popupAdorner.contentCounter != 0)
            {
                action?.Invoke();
                return;
            }
            popupAdorner.shown[index] = false;
        }
        else if (index == PopupManagerAssist.ToastIndex)
        {
            popupAdorner.toastCounter--;

            if (popupAdorner.shown[index] == false || popupAdorner.toastCounter != 0)
            {
                return;
            }

            popupAdorner.shown[index] = false;
            action?.Invoke();
        }

        RemoveVisual(visual, durationAnimation, action);
    }

    private static void DisplayVisual(FrameworkElement @object, TimeSpan duration)
    {
        KeyTime keyTime = TimeSpan.FromMilliseconds(01);

        var animation2 = @object.BuildAnimation(i => i.Visibility, Visibility.Visible, keyTime);

        DoubleAnimation animation1 = @object.BuildAnimation(i => i.Opacity, 0.001d, 1d, duration);

        Storyboard storyboard = new Storyboard();

        storyboard.AppendAnimations(animation1);
        storyboard.AppendAnimations(animation2);

        storyboard.Begin();
    }

    private static void RemoveVisual(FrameworkElement @object, TimeSpan durationAnimation, Action? popupCloseCallback = null)
    {
        KeyTime keyTime = TimeSpan.FromMilliseconds(010);

        var animation1 = @object.BuildAnimation(i => i.Opacity, 1, 0, durationAnimation);
        var animation2 = @object.BuildAnimation(i => i.Visibility, Visibility.Collapsed, keyTime);

        Storyboard storyboard = new Storyboard();

        storyboard.AppendAnimations(animation1, animation2);

        storyboard.RegisterCompleted(popupCloseCallback);

        storyboard.Begin();
    }
}
