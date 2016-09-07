// ContentBrowserViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Framework.Services;
using dEditor.Utility;
using Microsoft.Win32;

namespace dEditor.Widgets.ContentBrowser
{
    [Export(typeof(IContentBrowser))]
    public sealed class ContentBrowserViewModel : Widget, IContentBrowser
    {
        private List<ContentItem> _contents;
        private bool _directoryTreeVisible;
        private IEnumerable<DirectoryTreeItem> _rootDirectories;
        private IList<ContentItem> _selectedContents;
        private DirectoryTreeItem _selectedDirectory;
        private bool _showFolders;
        private bool _showNonContent;

        public ContentBrowserViewModel()
        {
            DisplayName = "Content Browser";
            UpdateDirectoryTree();
            IsDirectoryTreeVisible = true;
            ShowFolders = true;
            ShowNonContent = false;
            SelectedContents = new List<ContentItem>();
            Editor.Current.ProjectChanged += OnProjectChanged;
        }

        public string FolderFilter { get; set; }
        public string FileFilter { get; set; }

        public DirectoryTreeItem SelectedDirectory
        {
            get { return _selectedDirectory; }
            set
            {
                if (value == null) return;
                if (Equals(value, _selectedDirectory)) return;
                _selectedDirectory = value;
                value.IsSelected = true;
                UpdateContents();
                NotifyOfPropertyChange();
            }
        }

        public IList<ContentItem> SelectedContents
        {
            get { return _selectedContents; }
            set
            {
                if (Equals(value, _selectedContents)) return;
                _selectedContents = value;
                NotifyOfPropertyChange();
            }
        }

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public bool IsEnabled => Project.Current != null;

        public IEnumerable<DirectoryTreeItem> RootDirectories
        {
            get { return _rootDirectories; }
            set
            {
                _rootDirectories = value;
                NotifyOfPropertyChange();
            }
        }

        public List<ContentItem> Contents
        {
            get { return _contents; }
            set
            {
                _contents = value;
                NotifyOfPropertyChange();
            }
        }

        public bool AnySelected => SelectedContents?.Count > 0;

        [Export]
        public bool IsDirectoryTreeVisible
        {
            get { return _directoryTreeVisible; }
            set
            {
                if (value == _directoryTreeVisible) return;
                _directoryTreeVisible = value;
                NotifyOfPropertyChange();
            }
        }

        [Export]
        public bool ShowFolders
        {
            get { return _showFolders; }
            set
            {
                if (value == _showFolders) return;
                _showFolders = value;
                UpdateContents();
                NotifyOfPropertyChange();
            }
        }

        [Export]
        public bool ShowNonContent
        {
            get { return _showNonContent; }
            set
            {
                if (value == _showNonContent) return;
                _showNonContent = value;
                UpdateContents();
                NotifyOfPropertyChange();
            }
        }

        public override void SaveState(BinaryWriter writer)
        {
            writer.Write(ShowFolders);
            writer.Write(ShowNonContent);
            writer.Write(IsDirectoryTreeVisible);
        }

        public override void LoadState(BinaryReader reader)
        {
            ShowFolders = reader.ReadBoolean();
            ShowNonContent = reader.ReadBoolean();
            IsDirectoryTreeVisible = reader.ReadBoolean();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
                Editor.Current.ProjectChanged -= OnProjectChanged;
        }

        private void OnProjectChanged(Project project)
        {
            NotifyOfPropertyChange(nameof(IsEnabled));
            UpdateDirectoryTree();
        }

        public void UpdateDirectoryTree()
        {
            if (Project.Current == null)
            {
                RootDirectories = null;
                return;
            }

            var contentDir = new DirectoryTreeItem("Content", new DirectoryInfo(Project.Current.ContentPath));
            RootDirectories = new List<DirectoryTreeItem> { contentDir };
            SelectedDirectory = contentDir;
        }

        public void UpdateContents()
        {
            if (SelectedDirectory == null)
                return;

            var dir = SelectedDirectory.Directory;
            var parentTreeItem = GetDirectoryItem(d => d.Directory.EqualsDir(dir.Parent));

            var contents = new List<ContentItem>();

            if (parentTreeItem != null)
                contents.Add(new UpDirectoryItem(parentTreeItem));


            if (ShowFolders)
                contents.AddRange(
                    dir.GetDirectories()
                        .Select(d => new ContentItem(d))
                        .OrderBy(c => c.Name));

            var filters = ShowNonContent
                ? new[] { "*.*" }
                : new[] { "*.model", "*.audio", "*.animation", "*.cubemap", "*.texture", "*.mesh" };

            foreach (var filter in filters)
                contents.AddRange(dir.GetFiles(filter, SearchOption.TopDirectoryOnly).Select(f => new ContentItem(f)));

            Contents = contents;
        }

        public void ShowImportFileDialog()
        {
            var filter = "";

            foreach (var kv in ContentManager.Formats)
            {
                var formatsComma = "";
                var formatsColon = "";

                foreach (var format in kv.Value)
                {
                    formatsComma += $"*{format}, ";
                    formatsColon += $"*{format}; ";
                }

                formatsComma = formatsComma.Substring(0, formatsColon.Length - 2);
                formatsColon = formatsColon.Substring(0, formatsColon.Length - 2);

                filter += $"{kv.Key} ({formatsComma})|{formatsColon}|";
            }

            filter = filter.Substring(0, filter.Length-1);

            var dialog = new OpenFileDialog
            {
                Filter = filter
            };

            if (dialog.ShowDialog() == true)
            {
                ContentManager.Import(dialog.FileNames, SelectedDirectory.Path);
            }
        }

        public void ShowDirectoryTree()
        {
            IsDirectoryTreeVisible = true;
        }

        public void HideDirectoryTree()
        {
            IsDirectoryTreeVisible = false;
        }

        private DirectoryTreeItem GetDirectoryItem(IEnumerable<DirectoryTreeItem> collection, Func<DirectoryTreeItem, bool> predicate)
        {
            foreach (var item in collection)
            {
                if (predicate(item))
                    return item;
                var childItem =  GetDirectoryItem(item.SubFolders, predicate);
                if (childItem != null)
                    return childItem;
            }

            return null;
        }

        public DirectoryTreeItem GetDirectoryItem(Func<DirectoryTreeItem, bool> predicate)
        {
            return GetDirectoryItem(_rootDirectories, predicate);
        }
    }
}