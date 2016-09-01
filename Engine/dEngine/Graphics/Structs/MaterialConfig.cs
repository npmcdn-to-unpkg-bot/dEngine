// MaterialConfig.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 169

namespace dEngine.Graphics.Structs
{
    internal struct MaterialConfig
    {
        public float TextureTiling;
        public float DiffuseScale;
        public float SpecularScale;
        public float GlossScale;
        public float ReflectionScale;
        public float NormalShadowScale;
        public float SpecularLod;
        public float GlossLod;
        public float NormalDetailingTiling;
        public float NormalDetailingScale;
        public float FarTiling;
        public float DiffuseCutoff;
        public float NormalCutoff;
        public float SpecularCutoff;
        public float BumpIntensity;
        public float WangTilesScale;
        public int UseBlendColour;
        public int UseConstantNormal;
        public int UseConstantDiffuse;
        public int Unused;
    }
}