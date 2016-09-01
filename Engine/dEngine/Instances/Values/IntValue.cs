// IntValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds an integer.
    /// </summary>
    [TypeId(207)]
    [ToolboxGroup("Values")]
    [ExplorerOrder(3)]
    public sealed class IntValue : ValueContainer
    {
        private int _value;

        /// <inheritdoc />
        public IntValue()
        {
            _value = 0;
        }

        /// <summary>
        /// The value this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyChanged(nameof(Value));
            }
        }
    }
}