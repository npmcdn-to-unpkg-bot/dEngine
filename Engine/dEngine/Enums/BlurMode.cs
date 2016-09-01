// BlurMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Blurring methods to use in <see cref="AmbientOcclusionEffect" />.
    /// </summary>
    public enum BlurMode
    {
        None,
        Gaussian,
        HighQualityBilateral
    }
}