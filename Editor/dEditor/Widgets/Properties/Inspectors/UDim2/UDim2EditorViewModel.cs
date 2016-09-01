// UDim2EditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Linq;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.UDim2
{
    public class UDim2EditorViewModel : EditorBase<dEngine.UDim2>, ILabelled
    {
        public UDim2EditorViewModel(object obj, Inst.CachedProperty desc) : base(obj, desc)
        {
        }

        public string StringValue
        {
            get { return Value.ToString(); }
            set
            {
                var floats = value.Split(',').Select(float.Parse).ToArray();
                Value = new dEngine.UDim2(floats[0], (int)floats[1], floats[2], (int)floats[3]);
                NotifyOfPropertyChange(() => StringValue);
            }
        }

        public override void NotifyOfPropertyChange(string propertyName)
        {
            if (propertyName == "Value")
                NotifyOfPropertyChange(() => StringValue);
            base.NotifyOfPropertyChange(propertyName);
        }
    }
}