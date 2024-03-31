namespace Toolkits;

#if !NET451

/// <summary>
///
/// </summary>
/// 2023/12/14 14:22
public static class SynchronizationContextExtensions
{
    /// <summary>
    /// Posts the specified synchronization context.
    /// </summary>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// 2023/11/29 10:17
    public static void Post(this SynchronizationContext synchronizationContext, Action action)
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));
        synchronizationContext.Post(o => ((Action)o!)!(), action);
    }

    /// <summary>
    /// Posts the specified synchronization context.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue">The target value.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    /// 2023/12/14 10:44
    public static void Post<T>(
        this SynchronizationContext synchronizationContext,
        T targetValue,
        Action<T> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        synchronizationContext.Post(
            o =>
            {
                if (o is (T tv, Action<T> at))
                {
                    at.Invoke(tv);
                }
            },
            (targetValue, callback)
        );
    }

    /// <summary>
    /// Posts the specified target value1.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="callback">The callback.</param>
    /// 2024/2/2 10:03
    /// <exception cref="System.ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static void Post<T1, T2>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        Action<T1, T2> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        synchronizationContext.Post(
            o =>
            {
                if (o is (T1 t1, T2 t2, Action<T1, T2> at))
                {
                    at.Invoke(t1, t2);
                }
            },
            (targetValue1, targetValue2, callback)
        );
    }

    /// <summary>
    /// Posts the specified target value1.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="targetValue3">The target value3.</param>
    /// <param name="callback">The callback.</param>
    /// 2024/2/2 10:03
    /// <exception cref="System.ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static void Post<T1, T2, T3>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        T3 targetValue3,
        Action<T1, T2, T3> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        synchronizationContext.Post(
            o =>
            {
                if (o is (T1 t1, T2 t2, T3 t3, Action<T1, T2, T3> at))
                {
                    at.Invoke(t1, t2, t3);
                }
            },
            (targetValue1, targetValue2, targetValue3, callback)
        );
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="action">The action.</param>
    /// 2023/12/14 14:27
    /// <exception cref="System.ArgumentNullException">
    /// action
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task PostAsync(
        this SynchronizationContext synchronizationContext,
        Func<Task> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        synchronizationContext.Post(
            async o =>
            {
                if (o is not (Func<Task> a, TaskCompletionSource<bool> tcs))
                {
                    return;
                }

                await a()
                    .ContinueWith(t =>
                    {
                        if (t.Exception is null)
                        {
                            tcs.SetResult(true);
                            return;
                        }
                        tcs.SetException(t.Exception);
                    });
            },
            (action, taskCompletionSource)
        );

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue">The target value.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    public static async Task PostAsync<T>(
        this SynchronizationContext synchronizationContext,
        T targetValue,
        Func<T, Task> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

        synchronizationContext.Post(
            async o =>
            {
                if (o is (T tv, Func<T, Task> at, TaskCompletionSource<int> tcs))
                {
                    await at(tv)
                        .ContinueWith(t =>
                        {
                            if (t.Exception is null)
                            {
                                tcs.SetResult(0);
                                return;
                            }
                            tcs.SetException(t.Exception);
                        });
                }
            },
            (targetValue, callback, taskCompletionSource)
        );

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="callback">The callback.</param>
    /// 2024/2/2 10:05
    /// <exception cref="System.ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task PostAsync<T1, T2>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        Func<T1, T2, Task> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

        synchronizationContext.Post(
            async o =>
            {
                if (o is (T1 t1, T2 t2, Func<T1, T2, Task> at, TaskCompletionSource<int> tcs))
                {
                    await at(t1, t2)
                        .ContinueWith(t =>
                        {
                            if (t.Exception is null)
                            {
                                tcs.SetResult(0);
                                return;
                            }
                            tcs.SetException(t.Exception);
                        });
                }
            },
            (targetValue1, targetValue2, callback, taskCompletionSource)
        );

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="targetValue3">The target value3.</param>
    /// <param name="callback">The callback.</param>
    /// 2024/2/2 10:05
    /// <exception cref="System.ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task PostAsync<T1, T2, T3>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        T3 targetValue3,
        Func<T1, T2, T3, Task> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

        synchronizationContext.Post(
            async o =>
            {
                if (
                    o is

                    (T1 t1, T2 t2, T3 t3, Func<T1, T2, T3, Task> at, TaskCompletionSource<int> tcs)
                )
                {
                    await at(t1, t2, t3)
                        .ContinueWith(t =>
                        {
                            if (t.Exception is null)
                            {
                                tcs.SetResult(0);
                                return;
                            }
                            tcs.SetException(t.Exception);
                        });
                }
            },
            (targetValue1, targetValue2, targetValue3, callback, taskCompletionSource)
        );

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static async Task<TResult> PostAsync<TResult>(
        this SynchronizationContext synchronizationContext,
        Func<TResult> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            o =>
            {
                if (o is not (Func<TResult> a, TaskCompletionSource<TResult> tcs))
                {
                    return;
                }

                try
                {
                    TResult result = a();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            },
            (action, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    /// 2024/2/21 18:01
    /// <exception cref="ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        Func<T1, TResult> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            o =>
            {
                if (o is (T1 t1, Func<T1, TResult> at, TaskCompletionSource<TResult> tcs))
                {
                    try
                    {
                        tcs.SetResult(at(t1));
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            },
            (targetValue1, callback, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    /// 2024/2/21 17:48
    /// <exception cref="ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, T2, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        Func<T1, T2, TResult> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            o =>
            {
                if (
                    o is (T1 t1, T2 t2, Func<T1, T2, TResult> at, TaskCompletionSource<TResult> tcs)
                )
                {
                    try
                    {
                        tcs.SetResult(at(t1, t2));
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            },
            (targetValue1, targetValue2, callback, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="targetValue1">The target value1.</param>
    /// <param name="targetValue2">The target value2.</param>
    /// <param name="targetValue3">The target value3.</param>
    /// <param name="callback">The callback.</param>
    /// <returns></returns>
    /// 2024/2/21 18:10
    /// <exception cref="ArgumentNullException">
    /// callback
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, T2, T3, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 targetValue1,
        T2 targetValue2,
        T3 targetValue3,
        Func<T1, T2, T3, TResult> callback
    )
    {
        _ = callback ?? throw new ArgumentNullException(nameof(callback));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            o =>
            {
                if (
                    o is

                    (
                        T1 t1,
                        T2 t2,
                        T3 t3,
                        Func<T1, T2, T3, TResult> at,
                        TaskCompletionSource<TResult> tcs
                    )
                )
                {
                    try
                    {
                        tcs.SetResult(at(t1, t2, t3));
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            },
            (targetValue1, targetValue2, targetValue3, callback, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the specified synchronization context.
    /// </summary>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// 2023/11/29 10:17
    public static async Task PostAsync(
        this SynchronizationContext synchronizationContext,
        Action action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        synchronizationContext.Post(
            o =>
            {
                if (o is not (Action a, TaskCompletionSource<bool> tcs))
                {
                    return;
                }

                try
                {
                    a();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            },
            (action, taskCompletionSource)
        );

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static async Task<T> PostAsync<T>(
        this SynchronizationContext synchronizationContext,
        Func<Task<T>> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();

        synchronizationContext.Post(
            async o =>
            {
                if (o is not (Func<Task<T>> inputAction, TaskCompletionSource<T> input))
                {
                    return;
                }

                await inputAction()
                    .ContinueWith(t =>
                    {
                        if (t.Exception is null)
                        {
                            input.SetResult(t.Result);
                            return;
                        }
                        input.SetException(t.Exception);
                    });
            },
            (action, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// 2024/3/1 15:41
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 t1,
        Func<T1, Task<TResult>> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            async o =>
            {
                if (
                    o
                    is not
                    (
                        T1 t1,
                        Func<T1, Task<TResult>> inputAction,
                        TaskCompletionSource<TResult> input
                    )
                )
                {
                    return;
                }

                await inputAction(t1)
                    .ContinueWith(t =>
                    {
                        if (t.Exception is null)
                        {
                            input.SetResult(t.Result);
                            return;
                        }
                        input.SetException(t.Exception);
                    });
            },
            (t1, action, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// 2024/2/22 9:10
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, T2, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 t1,
        T2 t2,
        Func<T1, T2, Task<TResult>> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();

        synchronizationContext.Post(
            async o =>
            {
                if (
                    o
                    is not
                    (
                        T1 t1,
                        T2 t2,
                        Func<T1, T2, Task<TResult>> inputAction,
                        TaskCompletionSource<TResult> input
                    )
                )
                {
                    return;
                }

                await inputAction(t1, t2)
                    .ContinueWith(t =>
                    {
                        if (t.Exception is null)
                        {
                            input.SetResult(t.Result);
                            return;
                        }
                        input.SetException(t.Exception);
                    });
            },
            (t1, t2, action, taskCompletionSource)
        );

        return await taskCompletionSource.Task;
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="synchronizationContext">The synchronization context.</param>
    /// <param name="t1">The t1.</param>
    /// <param name="t2">The t2.</param>
    /// <param name="t3">The t3.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    /// 2024/2/22 9:08
    /// <exception cref="ArgumentNullException">
    /// action
    /// or
    /// synchronizationContext
    /// </exception>
    public static async Task<TResult> PostAsync<T1, T2, T3, TResult>(
        this SynchronizationContext synchronizationContext,
        T1 t1,
        T2 t2,
        T3 t3,
        Func<T1, T2, T3, Task<TResult>> action
    )
    {
        _ = action ?? throw new ArgumentNullException(nameof(action));
        _ =
            synchronizationContext
            ?? throw new ArgumentNullException(nameof(synchronizationContext));

        SynchronizationContextAction<T1, T2, T3, TResult> refParameter = new(t1, t2, t3, action);

        synchronizationContext.Post(
            static async o =>
            {
                if (o is not SynchronizationContextAction<T1, T2, T3, TResult> input)
                {
                    return;
                }

                await input
                    .Callback(input.TP1, input.TP2, input.TP3)
                    .ContinueWith(
                        static (t, state) =>
                        {
                            if (
                                state is not SynchronizationContextAction<T1, T2, T3, TResult> input
                            )
                            {
                                return;
                            }

                            if (t.Exception is null)
                            {
                                input.TaskCompletionSource.SetResult(t.Result);
                                return;
                            }
                            input.TaskCompletionSource.SetException(t.Exception);
                        },
                        input
                    );
            },
            refParameter
        );

        return await refParameter.TaskCompletionSource.Task;
    }

    private record SynchronizationContextAction<T1, T2, T3, TResult>(
        T1 TP1,
        T2 TP2,
        T3 TP3,
        Func<T1, T2, T3, Task<TResult>> Callback
    )
    {
        public TaskCompletionSource<TResult> TaskCompletionSource =
            new TaskCompletionSource<TResult>();
    }
}


#endif
