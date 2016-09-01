// ObjectValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds an <see cref="Instance" />.
    /// </summary>
    [TypeId(87)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class ObjectValue : ValueContainer
    {
        private Instance _value;

        /// <inheritdoc />
        public ObjectValue()
        {
            _value = null;
        }

        /// <summary>
        /// The value this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Instance Value
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