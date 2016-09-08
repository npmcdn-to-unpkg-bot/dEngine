// Vertex.cs - dEngine
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
    internal struct Vertex : IDataType
    {
        [InstMember(1)] public Vector3 Position;
        [InstMember(2)] public Vector3 Normal;
        [InstMember(3)] public Vector2 TexCoord;
        [InstMember(4)] public Colour Colour;
        [InstMember(5)] public Vector3 Tangent;
        [InstMember(6)] public Vector3 BiTangent;

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoord, Colour colour, Vector3 tangent,
            Vector3 biTangent)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
            Colour = colour;
            Tangent = tangent;
            BiTangent = biTangent;
        }

        public static int Stride = Marshal.SizeOf<Vertex>();

        public void Load(BinaryReader reader)
        {
            Position.Load(reader);
            Normal.Load(reader);
            TexCoord.Load(reader);
            Colour.Load(reader);
            Tangent.Load(reader);
            BiTangent.Load(reader);
        }

        public void Save(BinaryWriter writer)
        {
            Position.Save(writer);
            Normal.Save(writer);
            TexCoord.Save(writer);
            Colour.Save(writer);
            Tangent.Save(writer);
            BiTangent.Save(writer);
        }
    }
#pragma warning restore 1591
}