// Point.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace dEngine.Graphics.Structs
{
    public struct Point
    {
        public Vector3 Position;
        public Colour Colour;

        public static int Stride = Marshal.SizeOf<Point>(); // 12 + 16

        public Point(ref BulletSharp.Math.Vector3 position, ref BulletSharp.Math.Vector3 colour)
        {
            Position = new Vector3(position.X, position.Y, position.Z);
            Colour = new Colour(colour.X, colour.Y, colour.Z);
        }
    }
}