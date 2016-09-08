// ObjectEntry.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using Caliburn.Micro;
using dEditor.Framework.Services;

namespace dEditor.Modules.Widgets.AdvancedObjects
{
    public class ObjectEntry : PropertyChangedBase
    {
        private bool _isVisible;

        public ObjectEntry(Type type)
        {
            Name = type.Name;
            Icon = IconProvider.GetIconUri(type);
        }

        public string Name { get; }
        public Uri Icon { get; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                //NotifyOfPropertyChange();
            }
        }
    }
}