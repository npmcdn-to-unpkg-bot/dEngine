// CustomTreeItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using dEngine.Instances;

namespace dEditor.Framework.Controls.CustomTreeView
{
    public abstract class CustomTreeItem : Screen, IComparable<CustomTreeItem>
    {
        public abstract string Name { get; set; }
        public abstract Uri Icon { get; set; }
        public abstract bool IsExpanded { get; set; }
        public abstract bool IsSelected { get; set; }
        public abstract Instance Instance { get; set; }
        public new abstract CustomTreeItem Parent { get; set; }
        public abstract ObservableCollection<CustomTreeItem> Items { get; set; }

        public abstract int CompareTo(CustomTreeItem other);

        public static implicit operator WeakReference<CustomTreeItem>(CustomTreeItem item)
        {
            return new WeakReference<CustomTreeItem>(item);
        }

        public static implicit operator CustomTreeItem(WeakReference<CustomTreeItem> weakRef)
        {
            CustomTreeItem item;
            return weakRef.TryGetTarget(out item) ? item : null;
        }
    }
}