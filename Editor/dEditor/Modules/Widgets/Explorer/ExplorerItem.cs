﻿// ExplorerItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using Caliburn.Micro;
using dEditor.Framework.Services;
using dEditor.Utility;
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEditor.Modules.Widgets.Explorer
{
    public class ExplorerItem : PropertyChangedBase, IComparable<ExplorerItem>
    {
        private readonly Uri _baseIcon;
        private bool _isExpanded;

        public ExplorerItem(Instance instance)
        {
            _baseIcon = IconProvider.GetIconUri(instance.GetType());
            var folder = instance as Folder;

            Instance = instance;
            ExplorerOrder = instance.GetType().GetCustomAttribute<ExplorerOrderAttribute>()?.Order ?? 4;
            Items = new SortedObservableCollection<ExplorerItem>();
            Items.CollectionChanged += ItemsOnCollectionChanged;
            Icon = folder == null ? _baseIcon : IconProvider.ModifyIconHue(_baseIcon, folder.Hue);

            SetParent(instance.Parent);

            instance.Changed.Event += OnInstancePropertyChanged;
        }

        public int ExplorerOrder { get; }
        public Instance Instance { get; }
        public string Name => Instance.Name;

        public Uri Icon { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value == _isExpanded) return;
                _isExpanded = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsSelected => Instance.IsSelected;
        public SortedObservableCollection<ExplorerItem> Items { get; }

        public ExplorerItem Parent { get; private set; }
        public int ChildIndex { get; set; }
        public bool IsLastChild { get; set; }

        public int CompareTo(ExplorerItem other)
        {
            var orderCompare = ExplorerOrder.CompareTo(other.ExplorerOrder);
            if (orderCompare != 0)
                return orderCompare;
            var nameCompare = string.Compare(Instance.Name, other.Instance.Name, StringComparison.Ordinal);
            if (nameCompare != 0)
                return nameCompare;
            var idCompare = Instance.ObjectId.CompareTo(other.Instance.ObjectId);
            if (idCompare != 0)
                return idCompare;
            return 0;
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var i = 0;
            foreach (var item in Items)
            {
                item.ChildIndex = i++;
                item.IsLastChild = i == Items.Count;
                //item.NotifyOfPropertyChange(nameof(Name));
            }
        }

        private void OnInstancePropertyChanged(string prop)
        {
            switch (prop)
            {
                case "Name":
                    NotifyOfPropertyChange(nameof(Name));
                    break;
                case "Parent":
                    SetParent(Instance.Parent);
                    break;
                case "IsSelected":
                    NotifyOfPropertyChange(nameof(IsSelected));
                    break;
                case "Hue":
                    Icon = IconProvider.ModifyIconHue(_baseIcon, ((Folder)Instance).Hue);
                    break;
            }
        }

        private void SetParent(Instance parent)
        {
            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                ExplorerItem treeItem = null;

                if (ExplorerOrder == -1)
                    return;

                if (parent != null)
                {
                    WeakReference<ExplorerItem> weak;
                    ExplorerViewModel.ExplorerItems.TryGetValue(parent, out weak);
                    if (weak == null)
                        return;
                    Debug.Assert(weak != null, "weak != null");
                    weak.TryGetTarget(out treeItem);
                }

                Parent?.Items.Remove(this);
                Parent = treeItem;
                treeItem?.Items.Add(this);
            });
        }

        public static ExplorerItem TryCreate(Instance instance)
        {
            ExplorerItem item = null;
            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                var explorerItems = ExplorerViewModel.ExplorerItems;
                if (explorerItems.ContainsKey(instance))
                    return;
                item = new ExplorerItem(instance);
                explorerItems.TryAdd(instance, new WeakReference<ExplorerItem>(item));

                if (instance is DataModel)
                    IoC.Get<IExplorer>().RootItem = item;
            });
            return item;
        }

        public override string ToString()
        {
            return Instance.ToString();
        }
    }
}