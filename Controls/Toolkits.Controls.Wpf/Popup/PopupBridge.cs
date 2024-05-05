using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Toolkits.Animation;
using Toolkits.Core;

namespace Toolkits.Controls.Assist;

[EditorBrowsable(EditorBrowsableState.Never)]
internal class PopupBridge
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? messageSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? contentSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? messageCloseSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? contentCloseSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? visualSemaphoreSlim;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal AsyncLocker? visualAnimationSemaphoreSlim;
    public AdornerLayer? AdornerLayer { get; set; }

    public PopupAdorner? PopupAdornet { get; set; }

    public bool IsLoaded { get; internal set; }

    private bool IsPopup;

    private int popupIndex = 0;

    public async Task DisplayVisualAsync()
    {
        await visualSemaphoreSlim!.WaitAsync();

        Interlocked.Increment(ref popupIndex);

        if (IsPopup == false)
        {
            IsPopup = true;

            PopupAdornet!.Opacity = 0.001;
            AdornerLayer!.Add(PopupAdornet);

            var dur = TimeSpan.FromMilliseconds(150);

            Storyboard storyboard = new Storyboard();

            var da = PopupAdornet.BuildAnimation(i => i.Opacity, 1d, dur);

            storyboard.AppendAnimations(da);
            storyboard.RegisterCompleted(visualAnimationSemaphoreSlim!.Release);
            storyboard.Begin();

            await visualAnimationSemaphoreSlim!.WaitAsync();
        }

        visualSemaphoreSlim.Release();
    }

    public async Task CloseVisualAsync(Action? closeCallback = null)
    {
        await visualSemaphoreSlim!.WaitAsync();

        Interlocked.Decrement(ref popupIndex);

        if (popupIndex == 0 && IsPopup == true)
        {
            IsPopup = false;

            PopupAdornet!.Opacity = 1;

            var da = new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(150) };
            Storyboard.SetTarget(da, PopupAdornet);
            Storyboard.SetTargetProperty(da, new PropertyPath(nameof(PopupAdornet.Opacity)));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(da);
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();
            void Storyboard_Completed(object? sender, EventArgs e)
            {
                storyboard.Completed -= Storyboard_Completed;
                AdornerLayer!.Remove(PopupAdornet);
                visualAnimationSemaphoreSlim!.Release();
                closeCallback?.Invoke();
            }

            await visualAnimationSemaphoreSlim!.WaitAsync();
        }
        else
        {
            closeCallback?.Invoke();
        }

        visualSemaphoreSlim.Release();
    }

    internal void Release()
    {
        popupIndex = 0;
        IsPopup = false;
        AdornerLayer = null;
        PopupAdornet = null;

        messageSemaphoreSlim?.Dispose();
        messageSemaphoreSlim = null;

        contentSemaphoreSlim?.Dispose();
        contentSemaphoreSlim = null;

        messageCloseSemaphoreSlim?.Dispose();
        messageCloseSemaphoreSlim = null;

        contentCloseSemaphoreSlim?.Dispose();
        contentCloseSemaphoreSlim = null;

        visualSemaphoreSlim?.Dispose();
        visualSemaphoreSlim = null;

        visualAnimationSemaphoreSlim?.Dispose();
        visualAnimationSemaphoreSlim = null;
    }

    internal void Init()
    {
        visualAnimationSemaphoreSlim = new AsyncLocker(0, 1);
        visualSemaphoreSlim = new(1, 1);
        messageSemaphoreSlim = new(1, 1);
        contentSemaphoreSlim = new(1, 1);
        messageCloseSemaphoreSlim = new(0, 1);
        contentCloseSemaphoreSlim = new(0, 1);
    }
}
