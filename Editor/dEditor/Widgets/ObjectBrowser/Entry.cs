// Entry.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEditor.Widgets.ObjectBrowser
{
    public abstract class Entry
    {
        public string Name { get; protected set; }
        public Uri Icon { get; protected set; }
    }
}