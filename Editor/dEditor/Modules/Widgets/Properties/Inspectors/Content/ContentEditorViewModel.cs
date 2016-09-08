// ContentEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Content
{
    public class ContentEditorViewModel : EditorBase<string>, ILabelled
    {
        public ContentEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }
    }
}