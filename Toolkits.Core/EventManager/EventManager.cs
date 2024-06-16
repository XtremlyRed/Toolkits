using System.Collections.Concurrent;
using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
/// a class of <see cref="EventManager"/>
/// </summary>
public class EventManager : IEventManager
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ConcurrentDictionary<Type, object> typeEvents = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> eventStorage = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string commonKey = Guid.NewGuid().ToString();

    /// <summary>
    /// create a new instance of <see cref="EventManager"/>
    /// </summary>
    public EventManager() { }

    /// <summary>
    /// get event
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEvent<TEvent> GetEvent<TEvent>()
    {
        return GetEvent<TEvent>(commonKey);
    }

    /// <summary>
    /// get event <see langword="by"/> <see cref="IEvent{TEvent}"/>
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channelName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEvent<TEvent> GetEvent<TEvent>(string channelName)
    {
        var eventType = typeof(TEvent);
        if (eventStorage.TryGetValue(eventType, out var channelStorage) == false)
        {
            lock (eventStorage)
            {
                if (eventStorage.TryGetValue(eventType, out channelStorage) == false)
                {
                    eventStorage[eventType] = channelStorage = new ConcurrentDictionary<string, object>();
                }
            }
        }

        if (channelStorage.TryGetValue(channelName, out var eventChannel) == false)
        {
            lock (eventStorage)
            {
                if (channelStorage.TryGetValue(channelName, out eventChannel) == false)
                {
                    var weak = new WeakReference(channelStorage);
                    channelStorage[channelName] = eventChannel = new EventChannel<TEvent>(weak, channelName);
                }
            }
        }

        if (eventChannel is not IEvent<TEvent> @event)
        {
            throw new InvalidOperationException($"channel name : {channelName} may be registered by other types");
        }

        return @event;
    }
}
