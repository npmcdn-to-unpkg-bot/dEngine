// EnumItemEntry.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEditor.Widgets.ObjectBrowser
{
    public class EnumItemEntry : MemberEntry
    {
        public EnumItemEntry(Type enumType, string key)
        {
            Name = key;
            Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/EnumItem_16x.png", UriKind.Relative);
            EnumType = enumType;
        }

        public Type EnumType { get; }
    }
}