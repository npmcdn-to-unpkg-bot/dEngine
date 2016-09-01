// ShadingModel.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    public enum ShadingModel
    {
        Standard,
        Unlit,
        Subsurface,
        Skin,
        ClearCoat,
        SubsurfaceProfile,
        TwoSided,
        Hair,
        Cloth,
        Eye,
        Classic = 255
    }
}