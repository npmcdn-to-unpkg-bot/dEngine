// ExtensionMethods.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dEditor.Utility
{
    public static class ExtensionMethods
    {
        public static TValue Unanimous<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> getter)
            where TValue : class
        {
            TValue first = null;

            using (var enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    if (first == null)
                        first = getter(item);
                    else if (getter(item) != first)
                        return null;
                }
            }

            return first;
        }

        public static string GetManifestResourceString(this Assembly assembly, string name)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Converts WPF colour to WinForms
        /// </summary>
        /// <param name="color">Цвет WPF</param>
        /// <returns>Цвет Windows Forms</returns>
        public static Color ToColor(this System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts WinsForms colour to WPF
        /// </summary>
        /// <param name="color">Цвет WindowsForms</param>
        /// <returns>Цвет WPF</returns>
        public static System.Windows.Media.Color ToColor(this Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static int Remove<T>(
            this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
                coll.Remove(itemToRemove);

            return itemsToRemove.Count;
        }
    }
}