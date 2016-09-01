// MaterialNodeCollection.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.IO;
using dEngine.Instances.Materials;
using dEngine.Serializer.V1;

#pragma warning disable 1591

namespace dEngine
{
    public class MaterialNodeCollection : List<Node>, IDataType
    {
        public void Save(BinaryWriter writer)
        {
            var count = Count;
            writer.Write(count);
            for (var i = 0; i < count; i++)
                Inst.Serialize(this[i], writer.BaseStream);
        }

        public void Load(BinaryReader reader)
        {
            Clear();
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var item = (Node)Inst.Deserialize(reader.BaseStream);
                Add(item);
            }
        }
    }
}