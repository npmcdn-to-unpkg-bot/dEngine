// InstanceEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using dEditor.Framework.Services;
using dEngine;
using dEngine.Serializer.V1;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Instance
{
    public class InstanceEditorViewModel : EditorBase<dEngine.Instances.Instance>, ILabelled
    {
        public InstanceEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
        }

        public Uri Icon => IconProvider.GetIconUri(Value?.GetType());

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            if (propertyName == nameof(Value))
                NotifyOfPropertyChange(nameof(Icon));

            base.NotifyOfPropertyChange(propertyName);
        }

        public void OnLeftMouseButtonDown()
        {
            if (AreMultipleValuesSame)
                Game.Selection.Select(Value, true);
        }
    }
}