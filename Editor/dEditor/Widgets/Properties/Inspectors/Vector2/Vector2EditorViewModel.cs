// Vector2EditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.Vector2
{
    public class Vector2EditorViewModel : EditorBase<dEngine.Vector2>, ILabelled
    {
        public Vector2EditorViewModel(object obj, Inst.CachedProperty desc) : base(obj, desc)
        {
        }

        public string StringValue
        {
            get { return Value.ToString(); }
            set
            {
                var nums = value.Split(',');
                Value = new dEngine.Vector2(float.Parse(nums[0]), float.Parse(nums[1]));
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