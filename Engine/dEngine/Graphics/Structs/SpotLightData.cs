// SpotLightData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;

namespace dEngine.Graphics.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 64)]
    internal struct SpotLightData
    {
        internal Vector3 Position;
        internal float Padding0;
        internal Colour Colour;
        internal Vector3 Direction;
        internal float Padding1;
        internal Vector3 Up;
        internal float Padding2;
        internal float Range;
        internal float Aperture;
        internal uint Id;
    }
}