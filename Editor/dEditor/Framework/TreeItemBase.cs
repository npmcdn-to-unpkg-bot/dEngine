// TreeItemBase.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using Caliburn.Micro;
using dEditor.Utility;

namespace dEditor.Framework
{
    public abstract class TreeItemBase : PropertyChangedBase, IComparable<TreeItemBase>
    {
        private Uri _icon;
        private bool _isExpanded;
        private bool _isSelected;
        private string _name;

        protected TreeItemBase()
        {
            Children = new SortedObservableCollection<TreeItemBase>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public Uri Icon
        {
            get { return _icon; }
            set
            {
                if (Equals(value, _icon)) return;
                _icon = value;
                NotifyOfPropertyChange(() => Icon);
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (Equals(value, _isExpanded)) return;
                _isExpanded = value;
                NotifyOfPropertyChange();
            }
        }

        public SortedObservableCollection<TreeItemBase> Children { get; }

        public virtual int CompareTo(TreeItemBase other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}