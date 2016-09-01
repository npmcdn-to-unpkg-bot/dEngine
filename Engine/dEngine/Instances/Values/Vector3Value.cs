// Vector3Value.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="Vector3" />.
    /// </summary>
    [TypeId(85)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class Vector3Value : ValueContainer
    {
        private Vector3 _value;

        /// <inheritdoc />
        public Vector3Value()
        {
            _value = Vector3.Zero;
        }

        /// <summary>
        /// The value that this containers holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 Value
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