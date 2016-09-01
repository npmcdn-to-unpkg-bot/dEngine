// TimeEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.Time
{
    public class TimeEditorViewModel : EditorBase<TimeSpan>, ILabelled
    {
        public TimeEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }

        public string SelectedTime
        {
            get { return Value.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                Value = new TimeSpan(value);
            }
        }
    }
}