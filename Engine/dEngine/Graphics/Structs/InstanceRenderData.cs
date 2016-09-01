// InstanceRenderData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;
using SharpDX;

#pragma warning disable 1591

namespace dEngine.Graphics.Structs
{
    /// <summary>
    /// Instance data structure for shaders.
    /// </summary>
    public struct InstanceRenderData
    {
        internal static int Stride = Marshal.SizeOf<InstanceRenderData>();

        /// <summary>
        /// The transparency of the instance.
        /// </summary>
        public float Transparency;

        /// <summary>
        /// The size of the instance.
        /// </summary>
        public Vector3 Size;

        /// <summary>
        /// The base colour.
        /// </summary>
        public Colour Colour;

        /// <summary>
        /// The shading model.
        /// </summary>
        public ShadingModel ShadingModel;

        /// <summary>
        /// The emissive multiplier.
        /// </summary>
        public float Emissive;

        /// <summary>
        /// Roughness
        /// </summary>
        public float Smoothness;

        /// <summary>
        /// Roughness
        /// </summary>
        public float Metallic;

        /// <summary>
        /// The model matrix.
        /// </summary>
        public Matrix ModelMatrix;
    }
}