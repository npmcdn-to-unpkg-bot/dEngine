// ScalingMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Enum for image scaling algorithms.
    /// </summary>
    public enum ScalingMode
    {
        NearestNeighbor,
        Linear,
        Cubic,
        MultiSampleLinear,
        Anisotropic,
        HighQualityCubic
    }
}