// WeightedVertex.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using System.Runtime.InteropServices;

namespace dEngine.Graphics.Structs
{
#pragma warning disable 1591
    /// <summary>
    /// Vertex data structure for shaders.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size =
         sizeof(uint)*RenderConstants.MaxBonesPerVertex + sizeof(float)*RenderConstants.MaxBonesPerVertex)]
    internal struct WeightedVertex : IDataType
    {
        [InstMember(1)][FieldOffset(0)] public uint[] BoneIDs;
        [InstMember(2)][FieldOffset(sizeof(uint)*RenderConstants.MaxBonesPerVertex)] public float[] BoneWeights;

        public WeightedVertex(uint[] boneIds, float[] boneWeights)
        {
            BoneIDs = boneIds;
            BoneWeights = boneWeights;
        }

        public static int Stride = Marshal.SizeOf<WeightedVertex>();

        public void Load(BinaryReader reader)
        {
            var boneCount = RenderConstants.MaxBonesPerVertex;

            BoneIDs = new uint[RenderConstants.MaxBonesPerVertex];
            BoneWeights = new float[RenderConstants.MaxBonesPerVertex];

            for (var i = 0; i < boneCount; i++)
                BoneIDs[i] = reader.ReadUInt32();

            for (var i = 0; i < boneCount; i++)
                BoneWeights[i] = reader.ReadSingle();
        }

        public void Save(BinaryWriter writer)
        {
            var boneCount = RenderConstants.MaxBonesPerVertex;

            for (var i = 0; i < boneCount; i++)
                writer.Write(BoneIDs[i]);

            for (var i = 0; i < boneCount; i++)
                writer.Write(BoneWeights[i]);
        }
    }
#pragma warning restore 1591
}