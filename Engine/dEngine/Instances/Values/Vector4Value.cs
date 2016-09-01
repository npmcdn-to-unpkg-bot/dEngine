// Vector4Value.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="Vector4" />.
    /// </summary>
    [TypeId(230)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class Vector4Value : ValueContainer
    {
        private Vector4 _value;

        /// <inheritdoc />
        public Vector4Value()
        {
            _value = Vector4.Zero;
        }

        /// <summary>
        /// The value that this containers holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector4 Value
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