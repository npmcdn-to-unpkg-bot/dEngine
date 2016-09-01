// Vertex2D.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;

namespace dEngine.Graphics.Structs
{
#pragma warning disable 1591
    /// <summary>
    /// Vertex data structure for 2D guis.
    /// </summary>
    public struct Vertex2D
    {
        public Vector2 Position;
        public Vector2 TexCoord;

        public Vertex2D(Vector2 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;
        }

        public static int Stride = Marshal.SizeOf<Vertex2D>();
    }
#pragma warning restore 1591
}