using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using Toolkits.Core;
using Toolkits.Wpf;

namespace WPF_UI_TEST;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        test();
        base.OnStartup(e);
    }

    private void test()
    {
        var ss = Disposable.Use(true, i => Debug.WriteLine(i));

        using var _ = ss.Shadow();

        Disposable
            .Interval(TimeSpan.FromSeconds(1))
            .Counter(5)
            .Subscribe(
                (index) =>
                {
                    Debug.WriteLine(index);
                }
            );

        //BufferSegments<byte> bufferSegments = new BufferSegments<byte>(30, 10);

        //ThreadPool.QueueUserWorkItem(async o =>
        //{
        //    var index = 0;

        //    while (true)
        //    {
        //        index++;

        //        var buffer = BitConverter.GetBytes(index);

        //        bufferSegments.Write(buffer, 0, buffer.Length);

        //        await Task.Delay(5);
        //    }
        //});

        //ThreadPool.QueueUserWorkItem(o =>
        //{
        //    var buffer = new byte[4];

        //    while (true)
        //    {
        //        bufferSegments.Read(buffer, 0, buffer.Length);

        //        var result = BitConverter.ToInt32(buffer, 0);

        //        Console.WriteLine(result);
        //    }
        //});
    }
}
