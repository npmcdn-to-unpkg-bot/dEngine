// ObjectTypeEntry.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using dEditor.Framework.Services;
using dEngine.Instances;

namespace dEditor.Modules.Widgets.ObjectBrowser
{
    public class ObjectTypeEntry : Entry
    {
        public ObjectTypeEntry(Type type)
        {
            Name = type.Name;
            Type = type;

            if (typeof(Instance).IsAssignableFrom(type))
                Icon = IconProvider.GetIconUri(type);
            else if (type.IsEnum)
                Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/Enumerator_orange_16x.png", UriKind.Relative);
            else
                Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/Class_yellow_16x.png", UriKind.Relative);
        }

        public Type Type { get; }
    }
}