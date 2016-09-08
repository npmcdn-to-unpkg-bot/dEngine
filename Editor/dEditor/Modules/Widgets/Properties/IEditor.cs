// IEditor.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.Generic;
using dEditor.Framework;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties
{
    public interface IEditor
    {
        string DisplayName { get; set; }
        string Description { get; }
        bool IsReadOnly { get; }
        PropertiesViewModel PropertiesWidget { get; set; }
        Dictionary<object, BoundPropertyInfo> Objects { get; }
        BoundPropertyInfo AddObject(object obj, Inst.CachedProperty descriptor);
        void RemoveObject(object obj);
    }
}