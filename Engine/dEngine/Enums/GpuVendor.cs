// GpuVendor.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum for GPU vendors.
    /// </summary>
    public enum GpuVendor
    {
        /// <summary>
        /// The GPU vendor is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The GPU was made by NVIDIA.
        /// </summary>
        Nvidia = 0x10DE,

        /// <summary>
        /// The GPU was made by AMD.
        /// </summary>
        AMD = 0x1002,

        /// <summary>
        /// The GPU was made by Intel.
        /// </summary>
        Intel = 0x8086
    }
}