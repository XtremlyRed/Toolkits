using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Toolkits.Core;

namespace Toolkits.Core;

/// <summary>
/// Messenger
/// </summary>
public class DelegateRepeater : IDelegateRepeater
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly ConcurrentDictionary<string, InvokeInfo> subscribMaps = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static Type voidType = typeof(void);

    /// <summary>
    /// Unregister all delegate callback from subscriber by subscribeToken
    /// </summary>
    /// <param name="unregisterToken"></param>
    /// <exception cref="ArgumentNullException">unregisterToken</exception>
    public virtual void Unregister(string unregisterToken)
    {
        _ = unregisterToken ?? throw new ArgumentNullException(nameof(unregisterToken));

        using var ig = subscribMaps.GetEnumerator();

        while (ig.MoveNext())
        {
            var current = ig.Current;

            if (current.Key is null || current.Key != unregisterToken)
            {
                continue;
            }

            ig.Dispose();

            subscribMaps.TryRemove(current.Key, out var item);

            item?.Dispose();

            return;
        }
    }

    /// <summary>
    /// Publish
    /// </summary>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <Exception cref="ArgumentNullException"></Exception>
    /// <Exception cref="ArgumentException"></Exception>
    public virtual void Publish(string publishToken, params object[] delegateParamters)
    {
        _ = publishToken ?? throw new ArgumentNullException(nameof(publishToken));

        if (subscribMaps.TryGetValue(publishToken, out InvokeInfo? mapper) && mapper is not null)
        {
            if (mapper.ReturnType == voidType)
            {
                throw new ArgumentException("return type mismatch");
            }

            if (mapper.Arguments != null && delegateParamters != null)
            {
                if (mapper.Arguments.Length != delegateParamters.Length)
                {
                    throw new ArgumentException("inconsistent number of parameters");
                }
            }

            mapper.Invoke(delegateParamters ?? new object[] { null! });

            return;
        }

        throw new ArgumentException($"{nameof(publishToken)} unsubscribed");
    }

    /// <summary>
    ///  Publish
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    /// <Exception cref="ArgumentException"></Exception>
    public virtual TResult Publish<TResult>(string publishToken, params object[] delegateParamters)
    {
        _ = publishToken ?? throw new ArgumentNullException(nameof(publishToken));

        if (subscribMaps.TryGetValue(publishToken, out InvokeInfo? mapper) && mapper is not null)
        {
            if (mapper.ReturnType == typeof(TResult))
            {
                throw new ArgumentException("return type mismatch");
            }

            if (mapper.Arguments != null && delegateParamters != null)
            {
                if (mapper.Arguments.Length != delegateParamters.Length)
                {
                    throw new ArgumentException("inconsistent number of parameters");
                }
            }

            object? invokerValue = mapper.Invoke(delegateParamters ?? new object[] { null! });

            return invokerValue is TResult returnValue ? returnValue : default!;
        }

        throw new ArgumentException($"{nameof(publishToken)} unsubscribed");
    }

    /// <summary>
    ///  PublishAsync
    /// </summary>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public virtual async Task PublishAsync(string publishToken, params object[] delegateParamters)
    {
        _ = publishToken ?? throw new ArgumentNullException(nameof(publishToken));

        await Task.Run(() => Publish(publishToken, delegateParamters));
    }

    /// <summary>
    ///  PublishAsync
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public virtual async Task<TResult> PublishAsync<TResult>(string publishToken, params object[] delegateParamters)
    {
        _ = publishToken ?? throw new ArgumentNullException(nameof(publishToken));

        return await Task.Run(() => Publish<TResult>(publishToken, delegateParamters));
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TResult>(string token, Func<TResult> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe(string token, Action subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #region 1
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1>(string token, Action<TMessage1> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TResult>(string token, Func<TMessage1, TResult> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 2
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2>(string token, Action<TMessage1, TMessage2> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TResult>(string token, Func<TMessage1, TMessage2, TResult> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 3
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3>(string token, Action<TMessage1, TMessage2, TMessage3> subscribeDelegate)
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 4
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 5
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 6
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 7
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 8
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 9
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult>(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 10
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10>(
        string token,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TResult
    >(
        string token,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TResult> subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 11
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 12
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 13
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 14
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 15
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion
    #region 16
    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TMessage16">parameter 16</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TMessage16
    >(
        string token,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TMessage16
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TMessage16">parameter 16</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="token">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    public virtual void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TMessage16,
        TResult
    >(
        string token,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TMessage16,
            TResult
        > subscribeDelegate
    )
    {
        _ = token ?? throw new ArgumentNullException(nameof(token));

        MethodInfo method = subscribeDelegate?.Method ?? throw new ArgumentNullException(nameof(subscribeDelegate));
        if (subscribMaps.TryGetValue(token, out InvokeInfo? mapper) == false)
        {
            subscribMaps[token] = mapper = new InvokeInfo();
        }
        mapper?.Update(token, method, subscribeDelegate);
    }

    #endregion




    private class InvokeInfo : IDisposable
    {
        private object? Handler;
        private MethodInfo? Method;

        public void Update(string token, MethodInfo method, object handler)
        {
            Token = token;
            Arguments = method.GetParameters().Select(i => i.ParameterType).ToArray();
            ReturnType = method.ReturnType;
            Handler = handler;
            Method = Handler.GetType().GetMethod(nameof(MethodInfo.Invoke));
        }

        public Type[]? Arguments { get; private set; } = default!;
        public string Token { get; private set; } = default!;
        public Type ReturnType { get; private set; } = default!;

        public void Dispose()
        {
            Token = null!;
            Arguments = null!;
            ReturnType = null!;
            Method = null;
            Handler = null;
        }

        public object? Invoke(params object[] delegateParamters)
        {
            object? invokerValue = Method?.Invoke(Handler, delegateParamters);
            return invokerValue;
        }
    }
}
