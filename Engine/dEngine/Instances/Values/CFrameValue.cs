// CFrameValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="CFrame" />.
    /// </summary>
    [TypeId(84)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class CFrameValue : ValueContainer
    {
        private CFrame _value;

        /// <inheritdoc />
        public CFrameValue()
        {
            _value = CFrame.Identity;
        }

        /// <summary>
        /// The value that the container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public CFrame Value
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