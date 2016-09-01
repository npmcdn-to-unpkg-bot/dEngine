// BoundPropertyDescriptor.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.ComponentModel;

namespace dEditor.Widgets.Properties
{
    public class BoundPropertyDescriptor
    {
        public BoundPropertyDescriptor(object propertyOwner, PropertyDescriptor propertyDescriptor)
        {
            PropertyOwner = propertyOwner;
            PropertyDescriptor = propertyDescriptor;
        }

        public PropertyDescriptor PropertyDescriptor { get; }
        public object PropertyOwner { get; }

        public object Value
        {
            get { return PropertyDescriptor.GetValue(PropertyOwner); }
            set { PropertyDescriptor.SetValue(PropertyOwner, value); }
        }

        public event EventHandler ValueChanged
        {
            add { PropertyDescriptor.AddValueChanged(PropertyOwner, value); }
            remove { PropertyDescriptor.RemoveValueChanged(PropertyOwner, value); }
        }

        public static BoundPropertyDescriptor FromProperty(object propertyOwner, string propertyName)
        {
            // TODO: ContentCache all this.
            var properties = TypeDescriptor.GetProperties(propertyOwner);
            return new BoundPropertyDescriptor(propertyOwner, properties.Find(propertyName, false));
        }
    }
}