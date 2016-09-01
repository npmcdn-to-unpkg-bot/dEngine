// TypeEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Linq;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.Type
{
    public class TypeEditorViewModel : EditorBase<System.Type>, ILabelled
    {
        public TypeEditorViewModel(object obj, Inst.CachedProperty propDesc, object data) : base(obj, propDesc)
        {
            var filter = (string)data;

            switch (filter)
            {
                case "Enum":
                    Items = Inst.TypeDictionary.Values.Where(t => t.IsEnum).Select(t => t.Type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), "Unsupported type filter.");
            }
        }

        public IEnumerable<System.Type> Items { get; }
    }
}