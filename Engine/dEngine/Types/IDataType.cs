// IDataType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;

namespace dEngine
{
    /// <summary>
    /// Interface for objects which are not inherited from that can be serialized.
    /// </summary>
    public interface IDataType
    {
        /// <summary>
        /// Serializes the type to binary.
        /// </summary>
        void Save(BinaryWriter writer);

        /// <summary>
        /// Deserializes the type from binary.
        /// </summary>
        void Load(BinaryReader reader);
    }
}