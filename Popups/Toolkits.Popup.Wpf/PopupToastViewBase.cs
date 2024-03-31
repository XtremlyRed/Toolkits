global using UserControl = System.Windows.Controls.UserControl;
using System.Windows;
using System.Windows.Media.Animation;
using Toolkits.Animation;

namespace Toolkits.Popup;

/// <summary>
/// The base class for all toast popup views.
/// </summary>
public abstract class PopupToastViewBase : UserControl
{
    internal Action? CloseCallback { get; set; }

    /// <summary>
    /// Sets the toast information.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="objects">The objects.</param>
    /// <returns></returns>
    protected abstract void SetToastInfo(string title, string message, params object[] @objects);

    /// <summary>
    /// Sets the content of the toast.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="displayTimeSpan_Ms">The display time span ms.</param>
    /// <param name="objects">The objects.</param>
    /// <returns></returns>
    internal void SetToastContent(
        string title,
        string message,
        int displayTimeSpan_Ms,
        params object[] @objects
    )
    {
        SetToastInfo(title, message, @objects);
        SizeChanged += PopupToastViewBase_SizeChanged;

        Ealy(this, displayTimeSpan_Ms);

        static void Ealy(PopupToastViewBase dependencyObject, int displayTimeSpan_Ms)
        {
            if (displayTimeSpan_Ms < 0)
            {
                return;
            }

            System.Timers.Timer timer = new() { Interval = displayTimeSpan_Ms };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs? e)
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
                timer.Dispose();
                timer = null!;
                dependencyObject.Dispatcher.InvokeAsync(dependencyObject.CloseToast);
            }
        }
    }

    private void PopupToastViewBase_SizeChanged(
        object? sender,
        System.Windows.SizeChangedEventArgs e
    )
    {
        ResetSize();
    }

    internal void ResetSize()
    {
        Width = 320;
        if (ActualHeight > 1)
        {
            Height = ActualHeight;
        }

        MaxHeight = 100;
    }

    /// <summary>
    /// request close this toast
    /// </summary>
    protected void CloseToast()
    {
        SizeChanged -= PopupToastViewBase_SizeChanged;

        Thickness t = new(Width + Margin.Left + Margin.Right + 10, 5, 5, 0);

        TimeSpan ts = TimeSpan.FromMilliseconds(300);
        TimeSpan ts2 = TimeSpan.FromMilliseconds(150);

        ThicknessAnimation animation1 = this.BuildAnimation(i => i.Margin, t, ts, TimeSpan.Zero);
        DoubleAnimation animation2 = this.BuildAnimation(i => i.Height, 0, ts2, ts);

        Storyboard story = new Storyboard();

        story.AddAnimation(animation1);
        story.AddAnimation(animation2);
        story.RegisterCompleted(() =>
        {
            CloseCallback?.Invoke();
            CloseCallback = null;
        });

        story.Begin();
    }
}
