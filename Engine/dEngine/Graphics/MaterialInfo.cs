// MaterialInfo.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.InteropServices;
using dEngine.Instances.Materials;

#pragma warning disable 1591

namespace dEngine.Graphics
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct MaterialInfo
    {
        public float IsLit;
        public float CastShadows;
        public float IsNeon;

        public MaterialInfo(Material materialEnum, bool isLit, bool castsShadows, bool isNeon)
        {
            IsLit = isLit ? 1 : 0;
            CastShadows = castsShadows ? 1 : 0;
            IsNeon = isNeon ? 1 : 0;
        }
    }
}