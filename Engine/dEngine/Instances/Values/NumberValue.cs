// NumberValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="double" />.
    /// </summary>
    [TypeId(86)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class NumberValue : ValueContainer
    {
        private double _value;

        /// <inheritdoc />
        public NumberValue()
        {
            _value = 0;
        }

        /// <summary>
        /// The value this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public double Value
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