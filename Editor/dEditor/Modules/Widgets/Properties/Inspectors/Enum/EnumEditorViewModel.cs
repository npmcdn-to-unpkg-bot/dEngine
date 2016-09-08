// EnumEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Enum
{
    public class EnumEditorViewModel : EditorBase<System.Enum>, ILabelled
    {
        private readonly List<EnumItem> _items;

        public EnumEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
            _items = System.Enum.GetValues(propDesc.PropertyType).Cast<object>().Select(x => new EnumItem
            {
                Value = x,
                Text = System.Enum.GetName(propDesc.PropertyType, x)
            }).ToList();
        }

        public IEnumerable<EnumItem> Items => _items;
    }

    public class EnumItem : PropertyChangedBase
    {
        private string _text;
        public object Value { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange();
            }
        }
    }
}