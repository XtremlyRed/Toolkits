using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Toolkits;

/// <summary>
/// enumerable extensions
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Determines whether [is null or empty].
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        if (source is null)
        {
            return true;
        }

        if (source is ICollection<TSource> collection)
        {
            return collection.Count == 0;
        }

        if (source is ICollection collection2)
        {
            return collection2.Count == 0;
        }

        foreach (object? _ in source)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Determines whether [is not null or empty].
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <returns>
    ///   <c>true</c> if [is not null or empty] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource>? source)
    {
        if (source is null)
        {
            return false;
        }

        if (source is ICollection<TSource> collection)
        {
            return collection.Count != 0;
        }

        if (source is ICollection collection2)
        {
            return collection2.Count != 0;
        }

        foreach (object? _ in source)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Wheres if.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="condition">if set to <c>true</c> [condition].</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    /// 2024/2/1 10:59
    /// <exception cref="System.ArgumentNullException">
    /// source
    /// or
    /// filter
    /// </exception>
    public static IEnumerable<Target> WhereIf<Target>(
        this IEnumerable<Target> source,
        bool condition,
        Func<Target, bool> filter
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        if (condition)
        {
            return source.Where(filter);
        }

        return source;
    }

    /// <summary>
    /// Get the position of an element in the collection and only return the position of the first matching element
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    /// 2024/3/6 9:02
    /// <exception cref="ArgumentNullException">
    /// source
    /// or
    /// filter
    /// </exception>
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> filter)
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        int index = 0;
        foreach (TSource? item in source)
        {
            if (filter(item))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    ///  Get the position of elements in the collection and return the positions of all matching elements
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    /// 2024/3/6 9:03
    /// <exception cref="ArgumentNullException">
    /// source
    /// or
    /// filter
    /// </exception>
    public static IEnumerable<int> IndexOfMany<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> filter
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        int index = 0;
        foreach (TSource? item in source)
        {
            if (filter(item))
            {
                yield return index;
            }

            index++;
        }

        yield break;
    }

    /// <summary>
    /// paging
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    /// 2024/2/1 11:00
    /// <exception cref="System.ArgumentNullException">source</exception>
    public static IEnumerable<Target> Paginate<Target>(
        this IEnumerable<Target> source,
        int pageIndex,
        int pageSize
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));

        return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    [DebuggerNonUserCode]
    public static void ForEach<Target>(this IEnumerable<Target> source, Action<Target> action)
    {
        if (source is null || action is null)
        {
            return;
        }

        foreach (Target item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// Fors the each asynchronous.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task ForEachAsync<Target>(
        this IEnumerable<Target> source,
        Func<Target, Task> action
    )
    {
        if (source is null || action is null)
        {
            return;
        }

        foreach (Target item in source)
        {
            await action(item);
        }
    }

    /// <summary>
    /// Fors the each asynchronous.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task ForEachAsync<Target>(
        this IEnumerable<Target> source,
        Func<Target, int, Task> action
    )
    {
        if (source is null || action is null)
        {
            return;
        }
        int index = 0;

        using IEnumerator<Target> er = source.GetEnumerator();

        while (er.MoveNext())
        {
            await action(er.Current, index);
            index++;
        }
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    [DebuggerNonUserCode]
    public static void ForEach<Target>(this IEnumerable<Target> source, Action<Target, int> action)
    {
        if (source is null || action is null)
        {
            return;
        }
        int index = 0;

        using IEnumerator<Target> er = source.GetEnumerator();

        while (er.MoveNext())
        {
            action(er.Current, index);
            index++;
        }
    }

    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    [DebuggerNonUserCode]
    public static void ForEach<Target>(this IEnumerable source, Action<Target> action)
    {
        if (source is null || action is null)
        {
            return;
        }
        foreach (object item in source)
        {
            if (item is Target target)
            {
                action(target);
            }
        }
    }

    /// <summary>
    /// Tries for each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    /// <param name="exceptionCallback">The exception callback.</param>
    [DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void TryForEach<Target>(
        this IEnumerable<Target> source,
        Action<Target> action,
        Action<Exception> exceptionCallback
    )
    {
        if (source is null || action is null)
        {
            return;
        }

        foreach (Target item in source)
        {
            try
            {
                action(item);
            }
            catch (Exception ex)
            {
                if (exceptionCallback is null)
                {
                    throw;
                }
                exceptionCallback.Invoke(ex);
            }
        }
    }

    /// <summary>
    /// Tries for each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    /// <param name="exceptionCallback">The exception callback.</param>
    [DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void TryForEach<Target>(
        this IEnumerable<Target> source,
        Action<Target, int> action,
        Action<Exception> exceptionCallback
    )
    {
        if (source is null || action is null)
        {
            return;
        }
        int index = 0;

        using IEnumerator<Target> er = source.GetEnumerator();

        while (er.MoveNext())
        {
            try
            {
                action(er.Current, index);
            }
            catch (Exception ex)
            {
                if (exceptionCallback is null)
                {
                    throw;
                }
                exceptionCallback.Invoke(ex);
            }
            finally
            {
                index++;
            }
        }
    }

    /// <summary>
    /// Tries for each.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    /// <param name="exceptionCallback">The exception callback.</param>
    [DebuggerNonUserCode]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void TryForEach<Target>(
        this IEnumerable source,
        Action<Target> action,
        Action<Exception> exceptionCallback
    )
    {
        if (source is null || action is null)
        {
            return;
        }
        foreach (object item in source)
        {
            if (item is Target target)
            {
                try
                {
                    action(target);
                }
                catch (Exception ex)
                {
                    if (exceptionCallback is null)
                    {
                        throw;
                    }
                    exceptionCallback.Invoke(ex);
                }
            }
        }
    }

    #region Sort

    /// <summary>
    /// Sorts the specified comparer.
    /// </summary>
    /// <typeparam name="Target">The type of the arget.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="comparer">The comparer.</param>
    /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
    /// <exception cref="ArgumentNullException">
    /// source
    /// or
    /// comparer
    /// </exception>
    public static void Sort<Target>(
        this List<Target> source,
        Func<Target, IComparable> comparer,
        bool isDescending = false
    )
    {
        _ = source ?? throw new ArgumentNullException(nameof(source));
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));

        if (isDescending)
        {
            source.Sort(new DescendingSortIComparer<Target>(comparer));
        }
        else
        {
            source.Sort(new SortIComparer<Target>(comparer));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private record DescendingSortIComparer<T>(Func<T, IComparable> Selector) : IComparer<T>
    {
        public int Compare(T? x, T? y)
        {
            if (x is null || y is null)
            {
                return 0;
            }

            return -Selector(x).CompareTo(Selector(y));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private record SortIComparer<T>(Func<T, IComparable> Selector) : IComparer<T>
    {
        public int Compare(T? x, T? y)
        {
            if (x is null || y is null)
            {
                return 0;
            }
            return Selector(x).CompareTo(Selector(y));
        }
    }

    #endregion

#if !NET6_0_OR_GREATER

    /// <summary>
    /// Segments the specified segment capacity.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="targets">The targets.</param>
    /// <param name="segmentSize">The segment capacity.</param>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IEnumerable<TSource[]> Chunk<TSource>(
        this IEnumerable<TSource> targets,
        int segmentSize
    )
    {
        if (targets is null || segmentSize < 1)
        {
            yield break;
        }

        using IEnumerator<TSource> enumerator = targets.GetEnumerator();

        int currentIndex = 0;
        TSource[] ARRAY = new TSource[segmentSize];
        while (enumerator.MoveNext())
        {
            ARRAY[currentIndex] = enumerator.Current;
            if (++currentIndex == segmentSize)
            {
                yield return ARRAY;

                ARRAY = new TSource[segmentSize];
                currentIndex = 0;
            }
        }

        if (currentIndex > 0)
        {
            Array.Resize(ref ARRAY, currentIndex);

            yield return ARRAY;
        }
    }
#endif

    private static readonly ConcurrentDictionary<Type, FieldInfo> fieldMaps = new();

    /// <summary>
    /// Ases the array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <returns></returns>
    /// 2023/12/18 13:50
    public static T[] ToReferenceArray<T>(this List<T> list)
    {
        if (list is null || list.Count == 0)
        {
#if NET451
            return new T[0];
#else
            return Array.Empty<T>();
#endif
        }

        if (fieldMaps.TryGetValue(typeof(T), out FieldInfo? fieldInfo) == false)
        {
            fieldMaps[typeof(T)] = fieldInfo = typeof(List<T>).GetField(
                "_items",
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;
        }

        if (fieldInfo is null)
        {
#if NET451
            return new T[0];
#else
            return Array.Empty<T>();
#endif
        }

        object? value = fieldInfo.GetValue(list);

        if (value is not T[] tArray)
        {
#if NET451
            return new T[0];
#else
            return Array.Empty<T>();
#endif
        }

        return tArray;
    }

    /// <summary>
    /// Clears the specified source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    public static void Clear<T>(this IProducerConsumerCollection<T> source)
    {
        if (source is null || source.Count == 0)
        {
            return;
        }

        lock (source)
        {
            while (source.TryTake(out _)) { }
        }
    }
}
