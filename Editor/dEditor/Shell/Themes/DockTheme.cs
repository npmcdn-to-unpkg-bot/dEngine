// DockTheme.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using Xceed.Wpf.AvalonDock.Themes;

namespace dEditor.Shell.Themes
{
    public class DockTheme : Theme
    {
        public override Uri GetResourceUri()
        {
            return new Uri(
                "/dEditor;component/Shell/Themes/DockTheme.xaml",
                UriKind.Relative);
        }
    }
}