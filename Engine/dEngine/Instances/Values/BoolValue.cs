// BoolValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a boolean.
    /// </summary>
    [TypeId(50)]
    [ToolboxGroup("Values")]
    [ExplorerOrder(3)]
    public sealed class BoolValue : ValueContainer
    {
        private bool _value;

        /// <inheritdoc />
        public BoolValue()
        {
            _value = false;
        }

        /// <summary>
        /// The value this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public bool Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyChanged();
            }
        }
    }
}