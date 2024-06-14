using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="interface"/> of <see cref="IEventManager"/>
/// </summary>
public interface IEventManager
{
    /// <summary>
    /// get message suscriber
    /// </summary>
    ISubscriber Subscriber { get; }

    /// <summary>
    /// get message publisher
    /// </summary>
    IPublisher Publisher { get; }
}

/// <summary>
/// a class of <see cref="EventManager"/>
/// </summary>
public class EventManager : IEventManager
{
    /// <summary>
    /// create a new instance of <see cref="EventManager"/>
    /// </summary>
    public EventManager()
    {
        var storage = new Storager();

        Subscriber = new EventSubscriber(storage);
        Publisher = new EventPublisher(storage);
    }

    /// <summary>
    /// get message subscrier
    /// </summary>
    public ISubscriber Subscriber { get; }

    /// <summary>
    /// get message publisher
    /// </summary>
    public IPublisher Publisher { get; }
}

/// <summary>
/// a class of <see cref="Storager"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal class Storager
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ConcurrentDictionary<Type, EventChannel> storage = new ConcurrentDictionary<Type, EventChannel>();

    /// <summary>
    /// <see langword="get"/> message channel <see langword="by"/> message type
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    public Channel GetChannel(Type eventType)
    {
        var block = GetEventChannel(eventType);

        block.Channel ??= new Channel();

        return block.Channel;
    }

    /// <summary>
    /// <see langword="get"/> message channel <see langword="by"/> message type and message channel name
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="channelName"></param>
    /// <returns></returns>
    public Channel GetChannel(Type eventType, string channelName)
    {
        var block = GetEventChannel(eventType);

        if (block.Channels.TryGetValue(channelName, out var channel) == false)
        {
            block.Channels[channelName] = channel = new Channel();
        }

        return channel;
    }

    /// <summary>
    /// get current message channel
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    private EventChannel GetEventChannel(Type eventType)
    {
        if (storage.TryGetValue(eventType, out var block) == false)
        {
            lock (storage)
            {
                if (storage.TryGetValue(eventType, out var block2) == false)
                {
                    storage[eventType] = block2 = new EventChannel();
                }

                block = block2;
            }
        }

        return block;
    }
}

/// <summary>
/// message channel
/// </summary>
internal class EventChannel
{
    /// <summary>
    /// type message channel
    /// </summary>
    public Channel Channel = new Channel();

    /// <summary>
    /// message name channel
    /// </summary>
    public ConcurrentDictionary<string, Channel> Channels = new();
}

/// <summary>
/// message channel
/// </summary>
internal class Channel : ConcurrentDictionary<Guid, object>
{
    /// <summary>
    /// channel name
    /// </summary>
    public string? Name { get; set; }
}
