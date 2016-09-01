// PointLightData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX;

namespace dEngine.Graphics.Structs
{
    internal struct PointLightData
    {
        public Vector3 Position;
        public float Range;
        public Color3 Colour;
        public float Falloff;

        public const int Stride = 32;
    }
}