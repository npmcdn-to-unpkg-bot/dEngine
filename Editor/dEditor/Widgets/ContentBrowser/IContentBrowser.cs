// IContentBrowser.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEditor.Framework;

namespace dEditor.Widgets.ContentBrowser
{
    public interface IContentBrowser : IWidget
    {
        bool IsDirectoryTreeVisible { get; set; }
        bool ShowFolders { get; set; }
        bool ShowNonContent { get; set; }
    }
}