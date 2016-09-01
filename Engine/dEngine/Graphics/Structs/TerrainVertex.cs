// TerrainVertex.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine.Graphics.Structs
{
    internal struct TerrainVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 Material0;
        public Vector4 Material1;

        public TerrainVertex(ref Vector3 position, ref Vector3 normal)
        {
            Position = position;
            Normal = normal;
            Material0 = new Vector4(0, 0, 0, 0);
            Material1 = new Vector4(0, 0, 0, 0);
        }

        public const int Stride = 4*3 + 4*3 + 4*4 + 4*4;

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}