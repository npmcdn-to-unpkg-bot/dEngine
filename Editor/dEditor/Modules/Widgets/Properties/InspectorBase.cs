// InspectorBase.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using Caliburn.Micro;

namespace dEditor.Modules.Widgets.Properties
{
    public abstract class InspectorBase : PropertyChangedBase, IComparable<InspectorBase>
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool IsReadOnly { get; }

        public int CompareTo(InspectorBase other)
        {
            return string.CompareOrdinal(Name, other.Name);
        }
    }
}