// NumberEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Windows.Controls;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Number
{
    public class NumberEditorViewModel<T> : EditorBase<T>, ILabelled where T : struct
    {
        public NumberEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
            var rangeAttr = propDesc.Range;

            if (rangeAttr != null)
            {
                Lower = rangeAttr.Min;
                Upper = rangeAttr.Max;
                TextBoxWidth = 50;
                IsRanged = true;
            }

            EnableHistory = false;
        }

        public Dock Dock => IsRanged ? Dock.Left : Dock.Top;
        public double Lower { get; set; }
        public double Upper { get; set; }
        public bool IsRanged { get; }
        public double TextBoxWidth { get; } = double.NaN;

        public double Increment => (typeof(T) == typeof(float)) || (typeof(T) == typeof(double)) ? 0.05f : 1;

        public double DoubleValue
        {
            get { return Convert.ToDouble(Value); }
            set { Value = (T)Convert.ChangeType(value, typeof(T)); }
        }

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            if (propertyName == nameof(Value))
                NotifyOfPropertyChange(() => DoubleValue);

            base.NotifyOfPropertyChange(propertyName);
        }
    }
}