using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="interface"/> of <see cref="IPublisher"/>
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// publish @event
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    void Publish<TEvent>(TEvent @event);

    /// <summary>
    /// publish @event by channel name
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channel"></param>
    /// <param name="event"></param>
    void Publish<TEvent>(string channel, TEvent @event);
}

/// <summary>
/// a class of <see cref="EventPublisher"/>
/// </summary>
internal class EventPublisher : IPublisher
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Storager storager;

    /// <summary>
    /// create a new instance of <see cref="EventPublisher"/>
    /// </summary>
    /// <param name="storager">@event storager</param>
    public EventPublisher(Storager storager)
    {
        this.storager = storager;
    }

    /// <summary>
    /// publish @event
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    public void Publish<TEvent>(TEvent @event)
    {
        var channel = storager.GetChannel(typeof(TEvent));

        InnerPublish(channel, @event);
    }

    /// <summary>
    /// publish @event by channel name
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channelName"></param>
    /// <param name="event"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Publish<TEvent>(string channelName, TEvent @event)
    {
        if (string.IsNullOrWhiteSpace(channelName))
        {
            throw new ArgumentException($"“{nameof(channelName)}” is null or white space.", nameof(channelName));
        }

        var channel = storager.GetChannel(typeof(TEvent), channelName);

        InnerPublish(channel, @event);
    }

    /// <summary>
    /// inner publish @event
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channel"></param>
    /// <param name="event"></param>
    private void InnerPublish<TEvent>(Channel channel, TEvent @event)
    {
        if (channel is null || channel.Count == 0)
        {
            return;
        }

        foreach (var item in channel)
        {
            if (item.Value is Action<TEvent> callback)
            {
                callback(@event);
            }
        }
    }
}
