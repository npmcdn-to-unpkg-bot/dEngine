// ColourValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="Colour" />.
    /// </summary>
    [TypeId(101)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class ColourValue : ValueContainer
    {
        private Colour _value;

        /// <inheritdoc />
        public ColourValue()
        {
            _value = Colour.White;
        }

        /// <summary>
        /// The value that this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Colour Value
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