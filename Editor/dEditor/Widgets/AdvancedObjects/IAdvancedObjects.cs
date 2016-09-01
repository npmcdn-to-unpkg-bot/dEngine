// IAdvancedObjects.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEditor.Framework;

namespace dEditor.Widgets.AdvancedObjects
{
    public interface IAdvancedObjects : IWidget
    {
        bool SelectInsertedObject { get; set; }
    }
}