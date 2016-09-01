// NumberRangeEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.NumberRange
{
    public class NumberRangeEditorViewModel : EditorBase<dEngine.NumberRange>, ILabelled
    {
        public NumberRangeEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }

        public Visibility MinVisiblity { get; set; }
        public Visibility MaxVisiblity { get; set; }

        public double MinValue
        {
            get { return Value.Min; }
            set { Value = new dEngine.NumberRange((float)value, Value.Max); }
        }

        public double MaxValue
        {
            get { return Value.Max; }
            set { Value = new dEngine.NumberRange(Value.Min, (float)value); }
        }
    }
}