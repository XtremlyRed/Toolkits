using System.Collections.Concurrent;
using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
/// a <see langword="class"/> of <see cref="EventChannel{TEvent}"/>
/// </summary>
/// <typeparam name="TEvent"></typeparam>
internal class EventChannel<TEvent> : IEvent<TEvent>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    WeakReference weakReference;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string channelName;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Action<TEvent>? subscribe;

    internal EventChannel(WeakReference weakReference, string channelName)
    {
        this.weakReference = weakReference;
        this.channelName = channelName;
    }

    /// <summary>
    /// publish <typeparamref name="TEvent"/>
    /// </summary>
    /// <param name="event"></param>
    public void Publish(TEvent @event)
    {
        if (subscribe is not null)
        {
            subscribe(@event);
        }
    }

    /// <summary>
    /// subscribe <typeparamref name="TEvent"/>
    /// </summary>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    public IUnsubscriber Subscribe(Action<TEvent> subscribe)
    {
        this.subscribe = subscribe;
        return new Unsubscriber(this.weakReference, this.channelName)!;
    }
}

/// <summary>
/// a class of <see cref="Unsubscriber"/>
/// </summary>
internal class Unsubscriber : IUnsubscriber
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    WeakReference weakReference;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string channelName;

    /// <summary>
    /// create a new instance of <see cref="Unsubscriber"/>
    /// </summary>
    /// <param name="weakReference"></param>
    /// <param name="channelName"></param>
    internal Unsubscriber(WeakReference weakReference, string channelName)
    {
        this.weakReference = weakReference;
        this.channelName = channelName;
    }

    /// <summary>
    /// dispose
    /// </summary>
    public void Dispose()
    {
        Unsubscribe();
    }

    /// <summary>
    /// unsubscribe
    /// </summary>
    public void Unsubscribe()
    {
        if (weakReference?.Target is ConcurrentDictionary<string, object> channel)
        {
            channel.TryRemove(this.channelName, out _);
            weakReference.Target = default!;
        }
        channelName = null!;
        weakReference = null!;
    }
}
