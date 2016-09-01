// DirectoryTreeItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.ObjectModel;
using System.IO;
using Caliburn.Micro;

namespace dEditor.Widgets.ContentBrowser
{
    public class DirectoryTreeItem : PropertyChangedBase
    {
        private readonly bool _isRoot;
        private Uri _icon;
        private bool _isExpanded;
        private bool _isSelected;

        private DirectoryTreeItem(string name, DirectoryInfo directory, bool isRoot)
        {
            Directory = directory;
            _isRoot = isRoot;
            Name = name;
            Path = directory.FullName;
            UpdateSubFolders();
            UpdateIcon();
        }

        public DirectoryTreeItem(string name, DirectoryInfo directory) : this(name, directory, true)
        {
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public Uri Icon
        {
            get { return _icon; }
            set
            {
                if (Equals(value, _icon)) return;
                _icon = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value == _isExpanded) return;
                _isExpanded = value;
                UpdateIcon();
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<DirectoryTreeItem> SubFolders { get; private set; }

        public double FontSize => _isRoot ? 14 : 12;
        public double Height => _isRoot ? 28 : 22;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        public DirectoryInfo Directory { get; }

        private void UpdateIcon()
        {
            string path;

            if (_isExpanded)
                path = _isRoot
                    ? "/dEditor;component/Content/Icons/Toolbar/folder_Open_16xLG.png"
                    : "/dEditor;component/Content/Icons/Toolbar/FolderOpen_16x.png";
            else
                path = _isRoot
                    ? "/dEditor;component/Content/Icons/Toolbar/folder_Closed_16xLG.png"
                    : "/dEditor;component/Content/Icons/Toolbar/Folder_16x.png";

            Icon =
                new Uri(path, UriKind.Relative);
        }

        public void UpdateSubFolders()
        {
            SubFolders = new ObservableCollection<DirectoryTreeItem>();

            foreach (var subDir in Directory.GetDirectories())
                SubFolders.Add(new DirectoryTreeItem(subDir.Name, subDir, false));
        }
    }
}