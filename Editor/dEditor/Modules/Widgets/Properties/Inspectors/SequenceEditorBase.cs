// SequenceEditorBase.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors
{
    public abstract class SequenceEditorBase<T> : EditorBase<Sequence<T>> where T : struct
    {
        protected SequenceEditorBase(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }
    }
}