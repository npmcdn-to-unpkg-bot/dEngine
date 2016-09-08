// Vector4EditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Linq;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Vector4
{
    public class Vector4EditorViewModel : EditorBase<dEngine.Vector4>, ILabelled
    {
        public Vector4EditorViewModel(object obj, Inst.CachedProperty desc) : base(obj, desc)
        {
        }

        public string StringValue
        {
            get { return Value.ToString(); }
            set
            {
                var nums = value.Split(',').Select(float.Parse).ToArray();
                Value = new dEngine.Vector4(nums[0], nums[1], nums[2], nums[3]);
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