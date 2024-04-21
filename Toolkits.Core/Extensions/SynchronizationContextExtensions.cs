using System.Runtime.InteropServices;

namespace Toolkits.Core;

/// <summary>
///
/// </summary>
/// 2023/12/14 14:22
public static class SynchronizationContextExtensions
{
    record PostMapBase<T>
    {
        private TaskCompletionSource<T> TaskCompletionSource = new();

        public async Task WaitAsync()
        {
            await TaskCompletionSource.Task;
        }

        public async Task<T> WaitResultAsync()
        {
            return await TaskCompletionSource.Task;
        }

        public void SetResult(T value)
        {
            TaskCompletionSource.SetResult(value);
        }

        public void SetException(Exception exception)
        {
            TaskCompletionSource.SetException(exception);
        }
    }

    record PostActionMap(Action Action) : PostMapBase<bool> { }

    /// <summary>
    /// Posts the specified action.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// context
    /// </exception>
    public static void Post(this SynchronizationContext context, Action action)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        context.Post(o => ((Action)o!)!(), action);
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// context
    /// </exception>
    public static async Task PostAsync(this SynchronizationContext context, Action action)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var postMap = new PostActionMap(action);

        context.Post(
            o =>
            {
                if (o is PostActionMap postMap)
                {
                    try
                    {
                        postMap.Action();
                        postMap.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        postMap.SetException(ex);
                    }
                }
            },
            postMap
        );

        await postMap.WaitAsync();
    }

    record PostFuncMapAsync(Func<Task> Action) : PostMapBase<bool> { }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// context
    /// </exception>
    public static async Task PostAsync(this SynchronizationContext context, Func<Task> action)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var postMap = new PostFuncMapAsync(action);

        context.Post(
            o =>
            {
                if (o is PostFuncMapAsync postMap)
                {
                    try
                    {
                        postMap.Action();
                        postMap.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        postMap.SetException(ex);
                    }
                }
            },
            postMap
        );

        await postMap.WaitAsync();
    }

    record PostFuncMapAsync<T>(Func<Task<T>> Action) : PostMapBase<T> { }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// context
    /// </exception>
    public static async Task<T> PostAsync<T>(
        this SynchronizationContext context,
        Func<Task<T>> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var postMap = new PostFuncMapAsync<T>(action);

        context.Post(
            async o =>
            {
                if (o is PostFuncMapAsync<T> postMap)
                {
                    try
                    {
                        var result = await postMap.Action();
                        postMap.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        postMap.SetException(ex);
                    }
                }
            },
            postMap
        );

        return await postMap.WaitResultAsync();
    }

    record PostFuncMap<T>(Func<T> Action) : PostMapBase<T> { }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context">The context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// context
    /// </exception>
    public static async Task<T> PostAsync<T>(this SynchronizationContext context, Func<T> action)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ = context ?? throw new ArgumentNullException(nameof(context));

        var postMap = new PostFuncMap<T>(action);

        context.Post(
            o =>
            {
                if (o is PostFuncMap<T> postMap)
                {
                    try
                    {
                        var result = postMap.Action();
                        postMap.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        postMap.SetException(ex);
                    }
                }
            },
            postMap
        );

        return await postMap.WaitResultAsync();
    }
    ///// <summary>
    ///// Posts the specified synchronization context.
    ///// </summary>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    ///// 2023/11/29 10:17
    //public static void Post(this SynchronizationContext context, Action action)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));
    //    context.Post(o => ((Action)o!)!(), action);
    //}

    ///// <summary>
    ///// Posts the specified synchronization context.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue">The target value.</param>
    ///// <param name="callback">The callback.</param>
    ///// <returns></returns>
    ///// 2023/12/14 10:44
    //public static void Post<T>(
    //    this SynchronizationContext context,
    //    T targetValue,
    //    Action<T> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    context.Post(
    //        o =>
    //        {
    //            if (o is PostMap<T> postMap)
    //            {
    //                postMap.Callback(postMap.Data);
    //            }
    //        },
    //        new PostMap<T>(targetValue, callback)
    //    );
    //}

    //record PostMap<T>(T Data, Action<T> Callback);

    ///// <summary>
    ///// Posts the specified target value1.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="callback">The callback.</param>
    ///// 2024/2/2 10:03
    ///// <exception cref="System.ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static void Post<T1, T2>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    Action<T1, T2> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    context.Post(
    //        static o =>
    //        {
    //            if (o is PostMap<T1, T2> postMap)
    //            {
    //                postMap.Callback(postMap.Data, postMap.Data2);
    //            }
    //        },
    //        new PostMap<T1, T2>(targetValue1, targetValue2, callback)
    //    );
    //}

    //record PostMap<T, T2>(T Data, T2 Data2, Action<T, T2> Callback);

    ///// <summary>
    ///// Posts the specified target value1.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="T3">The type of the 3.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="targetValue3">The target value3.</param>
    ///// <param name="callback">The callback.</param>
    ///// 2024/2/2 10:03
    ///// <exception cref="System.ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static void Post<T1, T2, T3>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    T3 targetValue3,
    //    Action<T1, T2, T3> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    context.Post(
    //        static o =>
    //        {
    //            if (o is PostMap<T1, T2, T3> postMap)
    //            {
    //                postMap.Callback.Invoke(postMap.Data1, postMap.Data2, postMap.Data3);
    //            }
    //        },
    //        new PostMap<T1, T2, T3>(targetValue1, targetValue2, targetValue3, callback)
    //    );
    //}

    //record PostMap<T, T2, T3>(T Data1, T2 Data2, T3 Data3, Action<T, T2, T3> Callback);

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="action">The action.</param>
    ///// 2023/12/14 14:27
    ///// <exception cref="System.ArgumentNullException">
    ///// action
    ///// or
    ///// context
    ///// </exception>
    //public static async Task PostAsync(
    //    this SynchronizationContext context,
    //    Func<Task> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (o is not (Func<Task> a, TaskCompletionSource<bool> tcs))
    //            {
    //                return;
    //            }

    //            await a()
    //                .ContinueWith(t =>
    //                {
    //                    if (t.Exception is null)
    //                    {
    //                        tcs.SetResult(true);
    //                        return;
    //                    }
    //                    tcs.SetException(t.Exception);
    //                });
    //        },
    //        (action, taskCompletionSource)
    //    );

    //    await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue">The target value.</param>
    ///// <param name="callback">The callback.</param>
    ///// <returns></returns>
    //public static async Task PostAsync<T>(
    //    this SynchronizationContext context,
    //    T targetValue,
    //    Func<T, Task> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (o is (T tv, Func<T, Task> at, TaskCompletionSource<int> tcs))
    //            {
    //                await at(tv)
    //                    .ContinueWith(t =>
    //                    {
    //                        if (t.Exception is null)
    //                        {
    //                            tcs.SetResult(0);
    //                            return;
    //                        }
    //                        tcs.SetException(t.Exception);
    //                    });
    //            }
    //        },
    //        (targetValue, callback, taskCompletionSource)
    //    );

    //    await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="callback">The callback.</param>
    ///// 2024/2/2 10:05
    ///// <exception cref="System.ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static async Task PostAsync<T1, T2>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    Func<T1, T2, Task> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (o is (T1 t1, T2 t2, Func<T1, T2, Task> at, TaskCompletionSource<int> tcs))
    //            {
    //                await at(t1, t2)
    //                    .ContinueWith(t =>
    //                    {
    //                        if (t.Exception is null)
    //                        {
    //                            tcs.SetResult(0);
    //                            return;
    //                        }
    //                        tcs.SetException(t.Exception);
    //                    });
    //            }
    //        },
    //        (targetValue1, targetValue2, callback, taskCompletionSource)
    //    );

    //    await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="T3">The type of the 3.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="targetValue3">The target value3.</param>
    ///// <param name="callback">The callback.</param>
    ///// 2024/2/2 10:05
    ///// <exception cref="System.ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static async Task PostAsync<T1, T2, T3>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    T3 targetValue3,
    //    Func<T1, T2, T3, Task> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (
    //                o is

    //                (T1 t1, T2 t2, T3 t3, Func<T1, T2, T3, Task> at, TaskCompletionSource<int> tcs)
    //            )
    //            {
    //                await at(t1, t2, t3)
    //                    .ContinueWith(t =>
    //                    {
    //                        if (t.Exception is null)
    //                        {
    //                            tcs.SetResult(0);
    //                            return;
    //                        }
    //                        tcs.SetException(t.Exception);
    //                    });
    //            }
    //        },
    //        (targetValue1, targetValue2, targetValue3, callback, taskCompletionSource)
    //    );

    //    await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="TResult"></typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    //public static async Task<TResult> PostAsync<TResult>(
    //    this SynchronizationContext context,
    //    Func<TResult> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        o =>
    //        {
    //            if (o is not (Func<TResult> a, TaskCompletionSource<TResult> tcs))
    //            {
    //                return;
    //            }

    //            try
    //            {
    //                TResult result = a();
    //                tcs.SetResult(result);
    //            }
    //            catch (Exception ex)
    //            {
    //                tcs.SetException(ex);
    //            }
    //        },
    //        (action, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="callback">The callback.</param>
    ///// <returns></returns>
    ///// 2024/2/21 18:01
    ///// <exception cref="ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, TResult>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    Func<T1, TResult> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        o =>
    //        {
    //            if (o is (T1 t1, Func<T1, TResult> at, TaskCompletionSource<TResult> tcs))
    //            {
    //                try
    //                {
    //                    tcs.SetResult(at(t1));
    //                }
    //                catch (Exception ex)
    //                {
    //                    tcs.SetException(ex);
    //                }
    //            }
    //        },
    //        (targetValue1, callback, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="callback">The callback.</param>
    ///// <returns></returns>
    ///// 2024/2/21 17:48
    ///// <exception cref="ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, T2, TResult>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    Func<T1, T2, TResult> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        o =>
    //        {
    //            if (
    //                o is (T1 t1, T2 t2, Func<T1, T2, TResult> at, TaskCompletionSource<TResult> tcs)
    //            )
    //            {
    //                try
    //                {
    //                    tcs.SetResult(at(t1, t2));
    //                }
    //                catch (Exception ex)
    //                {
    //                    tcs.SetException(ex);
    //                }
    //            }
    //        },
    //        (targetValue1, targetValue2, callback, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="T3">The type of the 3.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="targetValue1">The target value1.</param>
    ///// <param name="targetValue2">The target value2.</param>
    ///// <param name="targetValue3">The target value3.</param>
    ///// <param name="callback">The callback.</param>
    ///// <returns></returns>
    ///// 2024/2/21 18:10
    ///// <exception cref="ArgumentNullException">
    ///// callback
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, T2, T3, TResult>(
    //    this SynchronizationContext context,
    //    T1 targetValue1,
    //    T2 targetValue2,
    //    T3 targetValue3,
    //    Func<T1, T2, T3, TResult> callback
    //)
    //{
    //    _ = callback ?? throw new ArgumentNullException(nameof(callback));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        o =>
    //        {
    //            if (
    //                o is

    //                (
    //                    T1 t1,
    //                    T2 t2,
    //                    T3 t3,
    //                    Func<T1, T2, T3, TResult> at,
    //                    TaskCompletionSource<TResult> tcs
    //                )
    //            )
    //            {
    //                try
    //                {
    //                    tcs.SetResult(at(t1, t2, t3));
    //                }
    //                catch (Exception ex)
    //                {
    //                    tcs.SetException(ex);
    //                }
    //            }
    //        },
    //        (targetValue1, targetValue2, targetValue3, callback, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the specified synchronization context.
    ///// </summary>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    ///// 2023/11/29 10:17
    //public static async Task PostAsync(
    //    this SynchronizationContext context,
    //    Action action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

    //    context.Post(
    //        o =>
    //        {
    //            if (o is not (Action a, TaskCompletionSource<bool> tcs))
    //            {
    //                return;
    //            }

    //            try
    //            {
    //                a();
    //                tcs.SetResult(true);
    //            }
    //            catch (Exception ex)
    //            {
    //                tcs.SetException(ex);
    //            }
    //        },
    //        (action, taskCompletionSource)
    //    );

    //    await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    //public static async Task<T> PostAsync<T>(
    //    this SynchronizationContext context,
    //    Func<Task<T>> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (o is not (Func<Task<T>> inputAction, TaskCompletionSource<T> input))
    //            {
    //                return;
    //            }

    //            await inputAction()
    //                .ContinueWith(t =>
    //                {
    //                    if (t.Exception is null)
    //                    {
    //                        input.SetResult(t.Result);
    //                        return;
    //                    }
    //                    input.SetException(t.Exception);
    //                });
    //        },
    //        (action, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="t1">The t1.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    ///// 2024/3/1 15:41
    ///// <exception cref="ArgumentNullException">
    ///// action
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, TResult>(
    //    this SynchronizationContext context,
    //    T1 t1,
    //    Func<T1, Task<TResult>> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (
    //                o
    //                is not
    //                (
    //                    T1 t1,
    //                    Func<T1, Task<TResult>> inputAction,
    //                    TaskCompletionSource<TResult> input
    //                )
    //            )
    //            {
    //                return;
    //            }

    //            await inputAction(t1)
    //                .ContinueWith(t =>
    //                {
    //                    if (t.Exception is null)
    //                    {
    //                        input.SetResult(t.Result);
    //                        return;
    //                    }
    //                    input.SetException(t.Exception);
    //                });
    //        },
    //        (t1, action, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="t1">The t1.</param>
    ///// <param name="t2">The t2.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    ///// 2024/2/22 9:10
    ///// <exception cref="ArgumentNullException">
    ///// action
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, T2, TResult>(
    //    this SynchronizationContext context,
    //    T1 t1,
    //    T2 t2,
    //    Func<T1, T2, Task<TResult>> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

    //    context.Post(
    //        async o =>
    //        {
    //            if (
    //                o
    //                is not
    //                (
    //                    T1 t1,
    //                    T2 t2,
    //                    Func<T1, T2, Task<TResult>> inputAction,
    //                    TaskCompletionSource<TResult> input
    //                )
    //            )
    //            {
    //                return;
    //            }

    //            await inputAction(t1, t2)
    //                .ContinueWith(t =>
    //                {
    //                    if (t.Exception is null)
    //                    {
    //                        input.SetResult(t.Result);
    //                        return;
    //                    }
    //                    input.SetException(t.Exception);
    //                });
    //        },
    //        (t1, t2, action, taskCompletionSource)
    //    );

    //    return await taskCompletionSource.Task;
    //}

    ///// <summary>
    ///// Posts the asynchronous.
    ///// </summary>
    ///// <typeparam name="T1">The type of the 1.</typeparam>
    ///// <typeparam name="T2">The type of the 2.</typeparam>
    ///// <typeparam name="T3">The type of the 3.</typeparam>
    ///// <typeparam name="TResult">The type of the result.</typeparam>
    ///// <param name="context">The synchronization context.</param>
    ///// <param name="t1">The t1.</param>
    ///// <param name="t2">The t2.</param>
    ///// <param name="t3">The t3.</param>
    ///// <param name="action">The action.</param>
    ///// <returns></returns>
    ///// 2024/2/22 9:08
    ///// <exception cref="ArgumentNullException">
    ///// action
    ///// or
    ///// context
    ///// </exception>
    //public static async Task<TResult> PostAsync<T1, T2, T3, TResult>(
    //    this SynchronizationContext context,
    //    T1 t1,
    //    T2 t2,
    //    T3 t3,
    //    Func<T1, T2, T3, Task<TResult>> action
    //)
    //{
    //    _ = action ?? throw new ArgumentNullException(nameof(action));
    //    _ =
    //        context
    //        ?? throw new ArgumentNullException(nameof(context));

    //    SynchronizationContextAction<T1, T2, T3, TResult> refParameter = new(t1, t2, t3, action);

    //    context.Post(
    //        static async o =>
    //        {
    //            if (o is not SynchronizationContextAction<T1, T2, T3, TResult> input)
    //            {
    //                return;
    //            }

    //            await input
    //                .Callback(input.TP1, input.TP2, input.TP3)
    //                .ContinueWith(
    //                    static (t, state) =>
    //                    {
    //                        if (
    //                            state is not SynchronizationContextAction<T1, T2, T3, TResult> input
    //                        )
    //                        {
    //                            return;
    //                        }

    //                        if (t.Exception is null)
    //                        {
    //                            input.TaskCompletionSource.SetResult(t.Result);
    //                            return;
    //                        }
    //                        input.TaskCompletionSource.SetException(t.Exception);
    //                    },
    //                    input
    //                );
    //        },
    //        refParameter
    //    );

    //    return await refParameter.TaskCompletionSource.Task;
    //}

    //private record SynchronizationContextAction<T1, T2, T3, TResult>(
    //    T1 TP1,
    //    T2 TP2,
    //    T3 TP3,
    //    Func<T1, T2, T3, Task<TResult>> Callback
    //)
    //{
    //    public TaskCompletionSource<TResult> TaskCompletionSource =
    //        new TaskCompletionSource<TResult>();
    //}
}
