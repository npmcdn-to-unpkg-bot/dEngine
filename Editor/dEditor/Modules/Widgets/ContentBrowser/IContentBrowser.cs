// IContentBrowser.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using dEditor.Framework;

namespace dEditor.Modules.Widgets.ContentBrowser
{
    public interface IContentBrowser : IWidget
    {
        bool IsDirectoryTreeVisible { get; set; }
        bool ShowFolders { get; set; }
        bool ShowNonContent { get; set; }
        DirectoryTreeItem SelectedDirectory { get; set; }
        IEnumerable<DirectoryTreeItem> RootDirectories { get; set; }
        DirectoryTreeItem GetDirectoryItem(Func<DirectoryTreeItem, bool> predicate);
    }
}