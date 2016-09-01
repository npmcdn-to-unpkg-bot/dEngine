// EnumValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds an enum value.
    /// </summary>
    [TypeId(195)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class EnumValue : ValueContainer
    {
        private static readonly ConcurrentDictionary<string, Enum[]> _values =
            new ConcurrentDictionary<string, Enum[]>();

        [InstMember(1)] private string _typeFullName;

        [InstMember(2)] private int _value;

        /// <inheritdoc />
        public EnumValue()
        {
            Type = typeof(Axis);
            Value = AlignmentX.Right;
        }

        /// <summary>
        /// The type of enum to store.
        /// </summary>
        [EditorVisible("Data", null, "Enum")]
        public Type Type
        {
            get { return Type.GetType(_typeFullName); }
            set
            {
                _typeFullName = value.FullName;

                var type = Type;
                Debug.Assert(type != null, "Type was non-existent.");

                if (!_values.ContainsKey(_typeFullName))
                {
                    var values = Enum.GetValues(type);
                    var valueList = new Enum[values.Length];

                    for (var i = 0; i < values.Length; i++)
                        valueList[i] = (Enum)values.GetValue(i);

                    _values[_typeFullName] = valueList;
                }

                Value = _values[_typeFullName][0];
            }
        }

        /// <summary>
        /// The value that the container holds.
        /// </summary>
        [EditorVisible]
        public Enum Value
        {
            get { return _values[_typeFullName][_value]; }
            set
            {
                _value = (int)(object)value;
                NotifyChanged();
            }
        }
    }
}