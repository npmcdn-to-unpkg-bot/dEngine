// RayValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="Ray" />.
    /// </summary>
    [ExplorerOrder(3)]
    [TypeId(223)]
    public class RayValue : ValueContainer
    {
        private Ray _value;

        /// <inheritdoc />
        public RayValue()
        {
            _value = Ray.Zero;
        }

        /// <summary>
        /// The value that this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Ray Value
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