using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="interface"/> of <see cref="ISubscriber"/>
/// </summary>
public interface ISubscriber
{
    /// <summary>
    /// subscribe message callback
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    IUnsubscriber Subscribe<TEvent>(Action<TEvent> subscribe);

    /// <summary>
    /// subscribe message callback by channel name
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channel"></param>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    IUnsubscriber Subscribe<TEvent>(string channel, Action<TEvent> subscribe);
}

/// <summary>
/// a <see langword="interface"/> of <see cref="IUnsubscriber"/>
/// </summary>
public interface IUnsubscriber : IDisposable
{
    /// <summary>
    /// cancel current subscription
    /// </summary>
    void Unsubscribe();
}

/// <summary>
/// a class of <see cref="EventSubscriber"/>
/// </summary>
internal class EventSubscriber : ISubscriber
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Storager storager;

    /// <summary>
    /// create a new instance of <see cref="EventSubscriber"/>
    /// </summary>
    /// <param name="storager"></param>
    public EventSubscriber(Storager storager)
    {
        this.storager = storager;
    }

    /// <summary>
    /// subscribe message callback
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IUnsubscriber Subscribe<TEvent>(Action<TEvent> subscribe)
    {
        _ = subscribe ?? throw new ArgumentNullException(nameof(subscribe));

        var channel = storager.GetChannel(typeof(TEvent));

        var subscribeToken = Guid.NewGuid();

        channel[subscribeToken] = subscribe;

        return new Unsubscriber(channel, subscribeToken);
    }

    /// <summary>
    /// subscribe message callback by channel name
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channelName"></param>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public IUnsubscriber Subscribe<TEvent>(string channelName, Action<TEvent> subscribe)
    {
        if (string.IsNullOrWhiteSpace(channelName))
        {
            throw new ArgumentException($"“{nameof(channelName)}” is null or white space.", nameof(channelName));
        }

        _ = subscribe ?? throw new ArgumentNullException(nameof(subscribe));

        var channel = storager.GetChannel(typeof(TEvent), channelName);

        var subscribeToken = Guid.NewGuid();

        channel[subscribeToken] = subscribe;

        return new Unsubscriber(channel, subscribeToken);
    }
}

/// <summary>
/// a class of <see cref="Unsubscriber"/>
/// </summary>
internal class Unsubscriber : IUnsubscriber
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    WeakReference channelReference = default!;

    Guid subscribeToken;

    /// <summary>
    /// create a new instance of <see cref="Unsubscriber"/>
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="subscribeToken"></param>
    public Unsubscriber(Channel channel, Guid subscribeToken)
    {
        this.channelReference = new WeakReference(channel);
        this.subscribeToken = subscribeToken;
    }

    void IDisposable.Dispose()
    {
        Unsubscribe();
    }

    /// <summary>
    /// unsubscribe
    /// </summary>
    public void Unsubscribe()
    {
        if (channelReference is not null && channelReference.Target is Channel channel)
        {
            channel.TryRemove(this.subscribeToken, out _);
            channelReference.Target = default!;
        }

        channelReference = null!;
    }
}
