// FontFamilyEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.FontFamily
{
    public class FontFamilyEditorViewModel : EditorBase<dEngine.FontFamily>, ILabelled
    {
        public FontFamilyEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }

        public System.Windows.Media.FontFamily FontFamilyValue
        {
            get { return new System.Windows.Media.FontFamily(Value); }
            set { Value = new dEngine.FontFamily(value.ToString()); }
        }
    }
}