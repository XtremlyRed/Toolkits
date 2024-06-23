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
    /// get event
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IEvent<TEvent> GetEvent<TEvent>();

    /// <summary>
    /// get event <see langword="by"/> <paramref name="channelName"/>
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="channelName"></param>
    /// <returns></returns>
    IEvent<TEvent> GetEvent<TEvent>(string channelName);
}

/// <summary>
/// a <see langword="interface"/> of <see cref="IEvent{TEvent}"/>
/// </summary>
public interface IEvent<TEvent>
{
    /// <summary>
    /// subscribe message callback
    /// </summary>
    /// <param name="subscribe"></param>
    /// <returns></returns>
    IUnsubscriber Subscribe(Action<TEvent> subscribe);

    /// <summary>
    /// publish @event
    /// </summary>
    /// <param name="event"></param>
    void Publish(TEvent @event);
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
