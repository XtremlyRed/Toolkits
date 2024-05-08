using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Toolkits.Wpf;

/// <summary>
/// visual tree assist
/// </summary>
public static partial class VisualTreeAssist
{
    /// <summary>
    /// Find child elements of an element
    /// </summary>
    /// <typeparam name="Target">Child element type</typeparam>
    /// <param name="dependencyObject"></param>
    /// <returns></returns>
    public static Target? FindChild<Target>(this DependencyObject dependencyObject)
        where Target : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
            if (child is not null and Target)
            {
                return (Target)child;
            }
            else
            {
                Target? childOfChild = FindChild<Target>(child!);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }
        return null!;
    }

    /// <summary>
    /// find visual children from <paramref name="dependencyObject"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="dependencyObject"></param>
    /// <returns></returns>
    public static IEnumerable<Target>? FindChildren<Target>(this DependencyObject dependencyObject)
        where Target : DependencyObject
    {
        if (dependencyObject != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                if (child is not null and Target)
                {
                    yield return (Target)child;
                }

                foreach (Target childOfChild in FindChildren<Target>(child!)!)
                {
                    yield return childOfChild;
                }
            }
        }
    }

    /// <summary>
    /// find visual parent from <paramref name="dependencyObject"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="dependencyObject"></param>
    /// <returns></returns>
    public static Target? FindParent<Target>(this DependencyObject dependencyObject)
        where Target : DependencyObject
    {
        while (true)
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(dependencyObject);
            if (dobj is null)
            {
                return default!;
            }

            if (dobj is Target target)
            {
                return target;
            }

            dependencyObject = dobj;
        }
    }

    /// <summary>
    /// find visual parent from <paramref name="dependencyObject"/>
    /// </summary>
    /// <param name="dependencyObject"></param>
    /// <param name="parentType"></param>
    /// <returns></returns>
    public static DependencyObject? FindParent(this DependencyObject dependencyObject, Type parentType)
    {
        while (true)
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(dependencyObject);
            if (dobj is null)
            {
                return default;
            }

            var currentType = dobj.GetType();

            if (currentType == parentType || currentType.BaseType == parentType)
            {
                return dobj;
            }

            dependencyObject = dobj;
        }
    }

    /// <summary>
    /// find visual parent from <paramref name="dependencyObject"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="dependencyObject"></param>
    /// <param name="elementName"></param>
    /// <returns></returns>
    public static Target? FindParent<Target>(this DependencyObject dependencyObject, string elementName)
        where Target : DependencyObject
    {
        DependencyObject dobj = VisualTreeHelper.GetParent(dependencyObject);
        return dobj != null
            ? dobj is Target && ((FrameworkElement)dobj).Name.Equals(elementName)
                ? (Target)dobj
                : FindParent<Target>(dobj, elementName)
            : null;
    }

    /// <summary>
    /// Find an element with a specified name
    /// </summary>
    /// <typeparam name="Target">Element Type</typeparam>
    /// <param name="dependencyObject"></param>
    /// <param name="elementName">Element name in xaml</param>
    /// <returns></returns>
    public static Target? FindChild<Target>(DependencyObject dependencyObject, string elementName)
        where Target : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
            if (child != null && child is Target && ((System.Windows.FrameworkElement)child).Name.Equals(elementName))
            {
                return (Target)child;
            }
            else
            {
                IEnumerator? j = FindChildren<Target>(child!)!.GetEnumerator();
                while (j.MoveNext())
                {
                    Target? childOfChild = (Target)j.Current!;

                    if (childOfChild != null && !(childOfChild! as FrameworkElement)!.Name.Equals(elementName))
                    {
                        FindChild<Target>(childOfChild, elementName);
                    }
                    else
                    {
                        return childOfChild;
                    }
                }
            }
        }
        return null;
    }
}
