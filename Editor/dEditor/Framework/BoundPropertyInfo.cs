// BoundPropertyInfo.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;
using dEngine.Serializer.V1;

namespace dEditor.Framework
{
    public class BoundPropertyInfo : IEquatable<BoundPropertyInfo>
    {
        private object _value;

        public BoundPropertyInfo(object o, Inst.CachedProperty property, string displayName = null)
        {
            Object = o;
            Property = property;

            IsReadOnly = (property.Set == null) || !property.IsSetterPublic;

            DisplayName = displayName ?? property.EditorVisible?.DisplayName ?? property.Name;

            UpdateValue();

            var instance = o as Instance;
            if (instance != null)
                instance.Changed.Event += OnInstanceChanged;
        }

        public object Object { get; }

        public Inst.CachedProperty Property { get; }

        public object Value
        {
            get { return _value; }
            set { Property?.FastSet(Object, value); }
        }

        public bool IsReadOnly { get; private set; }
        public string DisplayName { get; private set; }

        public bool Equals(BoundPropertyInfo other)
        {
            return Property == other.Property;
        }

        ~BoundPropertyInfo()
        {
            var instance = Object as Instance;
            if (instance != null)
                instance.Changed.Event -= OnInstanceChanged;
        }

        private void OnInstanceChanged(string s)
        {
            if (s == Property.Name)
                UpdateValue();
        }

        private void UpdateValue()
        {
            _value = Property.FastGet(Object);
        }
    }
}