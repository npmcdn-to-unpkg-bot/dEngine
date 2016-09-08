// Vector3EditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Vector3
{
    public class Vector3EditorViewModel : EditorBase<dEngine.Vector3>, ILabelled
    {
        public Vector3EditorViewModel(object obj, Inst.CachedProperty desc) : base(obj, desc)
        {
        }

        public string StringValue
        {
            get { return Value.ToString(); }
            set
            {
                var nums = value.Split(',');
                Value = new dEngine.Vector3(float.Parse(nums[0]), float.Parse(nums[1]), float.Parse(nums[2]));
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