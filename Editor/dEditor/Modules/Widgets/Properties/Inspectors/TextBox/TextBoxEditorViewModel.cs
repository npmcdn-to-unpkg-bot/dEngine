// TextBoxEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.TextBox
{
    public class TextBoxEditorViewModel : EditorBase<string>, ILabelled
    {
        public TextBoxEditorViewModel(object obj, Inst.CachedProperty desc) : base(obj, desc)
        {
            EnableHistory = false;
        }
    }
}