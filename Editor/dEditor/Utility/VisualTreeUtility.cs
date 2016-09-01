// VisualTreeUtility.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace dEditor.Utility
{
    public static class VisualTreeUtility
    {
        public static IEnumerable<T> GetVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if ((child != null) && child is T)
                        yield return (T)child;

                    foreach (var childOfChild in GetVisualChildren<T>(child))
                        yield return childOfChild;
                }
        }

        public static T FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            var parent = parentObject as T;
            if (parent != null)
                return parent;

            return FindParent<T>(parentObject);
        }

        public static void ForEach(Visual root, Action<Visual> func)
        {
            Where(root, x =>
            {
                func(x);
                return false;
            });
        }

        public static IEnumerable<Visual> Where(Visual root, Func<Visual, bool> predicate)
        {
            var list = new List<Visual>();

            Action<Visual> recursiveAction = null;

            recursiveAction = visual =>
            {
                var visualChildCount = VisualTreeHelper.GetChildrenCount(visual);
                for (var i = 0; i < visualChildCount; i++)
                {
                    var child = (Visual)VisualTreeHelper.GetChild(visual, i);

                    if (child == null) continue;

                    if (predicate(child))
                        list.Add(child);

                    recursiveAction(child);
                }
            };

            recursiveAction(root);

            return list;
        }

        public static Visual First(Visual visual, Func<Visual, bool> predicate)
        {
            var visualChildCount = VisualTreeHelper.GetChildrenCount(visual);
            for (var i = 0; i < visualChildCount; i++)
            {
                var child = (Visual)VisualTreeHelper.GetChild(visual, i);

                if (child == null) continue;

                if (predicate(child))
                    return child;

                var desc = First(child, predicate);

                if (desc != null)
                    return desc;
            }

            return null;
        }
    }
}