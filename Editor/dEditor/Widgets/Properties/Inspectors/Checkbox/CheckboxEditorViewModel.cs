// CheckboxEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.Checkbox
{
    public class CheckboxEditorViewModel : EditorBase<bool>, ILabelled
    {
        public CheckboxEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }

        public bool? CheckValue
        {
            get
            {
                if (AreMultipleValuesSame)
                    return Value;
                return null;
            }
            set { Value = value.GetValueOrDefault(); }
        }

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            if (propertyName == nameof(Value))
                NotifyOfPropertyChange(nameof(CheckValue));

            base.NotifyOfPropertyChange(propertyName);
        }
    }
}