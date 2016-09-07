// ContentItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Utility;
using dEditor.Widgets.ContentBrowser.ContextMenus;
using dEngine;
using dEngine.Data;

namespace dEditor.Widgets.ContentBrowser
{
    public class ContentItem : PropertyChangedBase
    {
        private Uri _icon;
        private ContextMenu _contextMenu;

        protected ContentItem()
        {
            UpdateIcon();
        }

        public ContentItem(FileInfo file)
        {
            Name = file.Name;
            Extension = file.Extension;
            File = file;
            UpdateIcon();
            using (var stream = file.Open(FileMode.Open, FileAccess.Read))
            {
                Type = AssetBase.PeekContent(stream);
            }
            ContextMenu = new FileContextMenu();
        }

        public ContentItem(DirectoryInfo directory)
        {
            Name = directory.Name;
            Directory = directory;
            UpdateIcon();
            Type = null;
            ContextMenu = new FolderContextMenu();
        }

        public ContentItem(string name)
        {
            Name = name;
        }

        public ContextMenu ContextMenu
        {
            get { return _contextMenu; }
            set
            {
                if (Equals(value, _contextMenu)) return;
                _contextMenu = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name { get; private set; }
        public string Extension { get; }
        public ContentType? Type { get; }
        public FileInfo File { get; }
        public DirectoryInfo Directory { get; }

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

        public bool IsFile => File != null;
        public bool IsFolder => Directory != null;
        public bool IsContent => Type != null;

        protected virtual void Open()
        {
            var contentBrowser = IoC.Get<IContentBrowser>();

            if (IsFolder)
                contentBrowser.SelectedDirectory =
                    contentBrowser.GetDirectoryItem(item => item.Directory.EqualsDir(Directory));
            else
                switch (Type)
                {
                    case ContentType.StaticMesh:
                    case ContentType.SkeletalMesh:
                    case ContentType.Model:
                    case ContentType.Texture:
                    case ContentType.Sound:
                    case ContentType.Animation:
                    case ContentType.Cubemap:
                    case ContentType.Video:
                    case ContentType.Material:
                        throw new NotImplementedException();
                    case ContentType.Unknown:
                    case null:
                        Process.Start(File.FullName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        public void OnMouseLeftButtonDown(MouseButtonEventArgs args)
        {
            if (args.ClickCount == 2)
                Open();
        }

        protected virtual string GetIconName()
        {
            string iconName;

            if (IsFolder)
                iconName = "Folder_256x";
            else
                switch (Extension)
                {
                    case ".game":
                        iconName = "dEngine_256x";
                        break;
                    case ".place":
                        iconName = "Web_256x";
                        break;
                    case ".mesh":
                        iconName = "3DScene_256x";
                        break;
                    case ".model":
                        iconName = "Bricks_256x";
                        break;
                    case ".texture":
                        iconName = "Image_256x";
                        break;
                    case ".sound":
                        iconName = "SoundFile_256x";
                        break;
                    case ".animation":
                        iconName = "UseCaseDiagram_256x";
                        break;
                    case ".script":
                        iconName = "LuaFile_256x";
                        break;
                    case ".cubemap":
                        iconName = "ResourceTemplate_256x";
                        break;
                    case ".video":
                        iconName = "PlayVideo_256x";
                        break;
                    case ".skeleton":
                        iconName = "UseCaseDiagram_256x";
                        break;
                    case ".material":
                        iconName = "Member_256x";
                        break;
                    default:
                        iconName = "File_256x";
                        break;
                }

            return iconName;
        }

        public void UpdateIcon()
        {
            var iconName = GetIconName();
            Icon = new Uri($"/dEditor;component/Content/Icons/Toolbar/{iconName}.png", UriKind.Relative);
        }
    }
}