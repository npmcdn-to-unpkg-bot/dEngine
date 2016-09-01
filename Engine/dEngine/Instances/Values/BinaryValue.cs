// BinaryValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> for holding binary data.
    /// </summary>
    [TypeId(145)]
    [ExplorerOrder(3)]
    public sealed class BinaryValue : ValueContainer
    {
        [InstMember(1)]
        [EditorVisible]
        internal BinaryData Value { get; set; }

        public BinaryData GetValue()
        {
            return Value;
        }

        internal void SetValue(Stream stream)
        {
            Value = new BinaryData(stream);
        }

        internal void SetValue(byte[] bytes)
        {
            Value = new BinaryData(bytes);
        }
    }
}