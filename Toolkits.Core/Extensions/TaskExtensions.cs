using System.Runtime.CompilerServices;

namespace Toolkits;

/// <summary>
/// task  extensions
/// </summary>
/// 2024/1/29 14:01
public static class TaskExtensions
{
    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/12/12 13:52
    public static TaskAwaiter GetAwaiter(this TimeSpan tasks)
    {
        return Task.Delay(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/11/27 8:02
    public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/11/27 8:02
    public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
    {
        _ = tasks ?? throw new ArgumentNullException(nameof(tasks));

        return Task.WhenAll(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="taskOpt">The task opt.</param>
    /// <returns></returns>
    /// 2024/1/2 16:00
    public static TaskAwaiter<T> GetAwaiter<T>(
        this (Task<T> task, TimeSpan timeSpan, string timeoutMessage) taskOpt
    )
    {
        static async Task<T> waitHandle(Task<T> executeTask, TimeSpan delay, string message)
        {
            using CancellationTokenSource cancellationTokenSource = new();

            Task delayTask = Task.Delay(delay, cancellationTokenSource.Token);
            _ = await Task.WhenAny(executeTask, delayTask);

            if (delayTask.IsCompleted == false)
            {
                cancellationTokenSource.Cancel();
                return executeTask.Result;
            }

            throw new TimeoutException(message);
        }

        return waitHandle(taskOpt.task, taskOpt.timeSpan, taskOpt.timeoutMessage).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="taskOpt">The task opt.</param>
    /// <returns></returns>
    /// 2024/1/2 16:00
    public static TaskAwaiter<T> GetAwaiter<T>(this (Task<T> task, TimeSpan timeSpan) taskOpt)
    {
        static async Task<T> waitHandle(Task<T> executeTask, TimeSpan delay)
        {
            using CancellationTokenSource cancellationTokenSource = new();
            Task delayTask = Task.Delay(delay, cancellationTokenSource.Token);
            _ = await Task.WhenAny(executeTask, delayTask);

            if (delayTask.IsCompleted == false)
            {
                cancellationTokenSource.Cancel();
                return executeTask.Result;
            }

            throw new TimeoutException();
        }

        return waitHandle(taskOpt.task, taskOpt.timeSpan).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="taskOpt">The task opt.</param>
    /// <returns></returns>
    /// 2024/1/2 16:01
    public static TaskAwaiter GetAwaiter(
        this (Task task, TimeSpan timeSpan, string timeoutMessage) taskOpt
    )
    {
        Task task1 = taskOpt.task;

        static async Task waitHandle(Task taskResult, TimeSpan delay, string message)
        {
            using CancellationTokenSource cancellationTokenSource = new();

            Task completedTask = await Task.WhenAny(
                taskResult,
                Task.Delay(delay, cancellationTokenSource.Token)
            );

            if (taskResult.IsCompleted)
            {
                cancellationTokenSource.Cancel();
                return;
            }

            throw new TimeoutException(message);
        }

        return waitHandle(task1, taskOpt.timeSpan, taskOpt.timeoutMessage).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <param name="taskOpt">The task opt.</param>
    /// <returns></returns>
    /// 2024/1/2 16:01
    public static TaskAwaiter GetAwaiter(this (Task task, TimeSpan timeSpan) taskOpt)
    {
        Task task1 = taskOpt.task;

        static async Task waitHandle(Task taskResult, TimeSpan delay)
        {
            using CancellationTokenSource cancellationTokenSource = new();

            Task completedTask = await Task.WhenAny(
                taskResult,
                Task.Delay(delay, cancellationTokenSource.Token)
            );

            if (taskResult.IsCompleted)
            {
                cancellationTokenSource.Cancel();
                return;
            }

            throw new TimeoutException();
        }

        return waitHandle(task1, taskOpt.timeSpan).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    /// 2023/11/27 8:07
    public static TaskAwaiter<(T, T1)> GetAwaiter<T, T1>(this (Task<T>, Task<T1>) tasks)
    {
        _ = tasks.Item1 ?? throw new ArgumentNullException("task 1");
        _ = tasks.Item2 ?? throw new ArgumentNullException("task 2");

        static async Task<(T, T1)> composite((Task<T>, Task<T1>) tasks)
        {
            await Task.WhenAll(tasks.Item1, tasks.Item2);
            return (tasks.Item1.Result, tasks.Item2.Result);
        }

        return composite(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    public static TaskAwaiter<(T, T1, T2)> GetAwaiter<T, T1, T2>(
        this (Task<T>, Task<T1>, Task<T2>) tasks
    )
    {
        _ = tasks.Item1 ?? throw new ArgumentNullException("task 1");
        _ = tasks.Item2 ?? throw new ArgumentNullException("task 2");
        _ = tasks.Item3 ?? throw new ArgumentNullException("task 3");

        static async Task<(T, T1, T2)> composite((Task<T>, Task<T1>, Task<T2>) tasks)
        {
            await Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3);
            return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result);
        }

        return composite(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    public static TaskAwaiter<(T, T1, T2, T3)> GetAwaiter<T, T1, T2, T3>(
        this (Task<T>, Task<T1>, Task<T2>, Task<T3>) tasks
    )
    {
        _ = tasks.Item1 ?? throw new ArgumentNullException("task 1");
        _ = tasks.Item2 ?? throw new ArgumentNullException("task 2");
        _ = tasks.Item3 ?? throw new ArgumentNullException("task 3");
        _ = tasks.Item4 ?? throw new ArgumentNullException("task 4");

        static async Task<(T, T1, T2, T3)> composite((Task<T>, Task<T1>, Task<T2>, Task<T3>) tasks)
        {
            await Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4);
            return (tasks.Item1.Result, tasks.Item2.Result, tasks.Item3.Result, tasks.Item4.Result);
        }

        return composite(tasks).GetAwaiter();
    }

    /// <summary>
    /// Gets the awaiter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <typeparam name="T3">The type of the 3.</typeparam>
    /// <typeparam name="T4">The type of the 4.</typeparam>
    /// <param name="tasks">The tasks.</param>
    /// <returns></returns>
    public static TaskAwaiter<(T, T1, T2, T3, T4)> GetAwaiter<T, T1, T2, T3, T4>(
        this (Task<T>, Task<T1>, Task<T2>, Task<T3>, Task<T4>) tasks
    )
    {
        _ = tasks.Item1 ?? throw new ArgumentNullException("task 1");
        _ = tasks.Item2 ?? throw new ArgumentNullException("task 2");
        _ = tasks.Item3 ?? throw new ArgumentNullException("task 3");
        _ = tasks.Item4 ?? throw new ArgumentNullException("task 4");
        _ = tasks.Item5 ?? throw new ArgumentNullException("task 5");

        static async Task<(T, T1, T2, T3, T4)> composite(
            (Task<T>, Task<T1>, Task<T2>, Task<T3>, Task<T4>) tasks
        )
        {
            await Task.WhenAll(tasks.Item1, tasks.Item2, tasks.Item3, tasks.Item4, tasks.Item5);
            return (
                tasks.Item1.Result,
                tasks.Item2.Result,
                tasks.Item3.Result,
                tasks.Item4.Result,
                tasks.Item5.Result
            );
        }

        return composite(tasks).GetAwaiter();
    }
}
