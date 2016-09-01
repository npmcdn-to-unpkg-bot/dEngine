// BinaryData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;

namespace dEngine
{
    /// <summary>
    /// Stores binary data.
    /// </summary>
    /// <remarks>
    /// Maximum value will be 2GB.
    /// </remarks>
    public class BinaryData : IDataType
    {
        private Stream stream;

        /// <summary />
        public BinaryData()
        {
        }

        /// <summary>
        /// Creates a <see cref="BinaryData" /> from the given bytes.
        /// </summary>
        public BinaryData(byte[] bytes)
        {
            stream = new MemoryStream(bytes);
        }

        /// <summary>
        /// Creates a <see cref="BinaryData" /> from the given stream.
        /// </summary>
        public BinaryData(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            writer.Write(stream.Length);
            stream.CopyTo(writer.BaseStream);
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            stream = new MemoryStream(reader.ReadBytes(count));
        }
    }
}