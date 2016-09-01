// InstanceReference.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances;

namespace dEngine
{
    /// <summary>
    /// A reference to an instance for networking.
    /// </summary>
    public struct InstanceReference : IDataType
    {
        /// <summary>
        /// The id of the instance to reference.
        /// </summary>
        [InstMember(1)] public string InstanceId;

        internal InstanceReference(Instance instance)
        {
            InstanceId = instance.InstanceId;
        }

        public void Load(BinaryReader reader)
        {
            InstanceId = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(InstanceId);
        }
    }
}