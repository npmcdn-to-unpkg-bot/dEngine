// CameraData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;
using SharpDX;

namespace dEngine.Graphics.Structs
{
    /// <summary>
    /// Camera data structure for shaders.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 16, Size = 240)]
    internal struct CameraData : IConstantBufferData
    {
#pragma warning disable 169
        [FieldOffset(0)] public Matrix ViewMatrix;
        [FieldOffset(64)] public Matrix ViewProjectionMatrix;
        [FieldOffset(128)] public Matrix InverseViewProjection;
        [FieldOffset(192)] public Vector3 Position;
        [FieldOffset(204)] private readonly float _pad0;
        [FieldOffset(208)] public float ClipNear;
        [FieldOffset(212)] public float ClipFar;
        [FieldOffset(216)] public float Unused0;
        [FieldOffset(220)] public float Unused1;
        [FieldOffset(224)] public SharpDX.Vector4 ScreenParams; // Width, Height,
#pragma warning restore 169

        public static int Stride = Marshal.SizeOf<CameraData>();
    }
}