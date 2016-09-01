// ImageExtendMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum for bitmap extend modes.
    /// </summary>
    public enum ExtendMode
    {
        /// <summary>
        /// Clamps to the border of the image.
        /// </summary>
        Clamp = 0,

        /// <summary>
        /// Wraps the image.
        /// </summary>
        Wrap = 1,

        /// <summary>
        /// Mirrors the image.
        /// </summary>
        Mirror = 2,

        /// <summary>
        /// Stretches the image.
        /// </summary>
        Stretch = 3
    }
}