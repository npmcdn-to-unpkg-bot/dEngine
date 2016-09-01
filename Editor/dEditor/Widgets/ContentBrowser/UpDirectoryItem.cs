// UpDirectoryItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Linq;
using Caliburn.Micro;

namespace dEditor.Widgets.ContentBrowser
{
    public class UpDirectoryItem : ContentItem
    {
        private readonly DirectoryTreeItem _parentDirectory;

        public UpDirectoryItem(DirectoryTreeItem parentDirectory) : base()
        {
            _parentDirectory = parentDirectory;
        }

        protected override string GetIconName()
        {
            return "Level03_256x";
        }

        protected override void Open()
        {
            var contentBrowser = IoC.Get<IContentBrowser>();
            contentBrowser.SelectedDirectory = _parentDirectory;
        }
    }
}