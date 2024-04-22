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
            static o =>
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
}
