// CameraType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum for types of camera movement.
    /// </summary>
    public enum CameraType
    {
        /// <summary>
        /// </summary>
        Fixed,

        /// <summary>
        /// </summary>
        Attach,

        /// <summary>
        /// </summary>
        Watch,

        /// <summary>
        /// </summary>
        Track,

        /// <summary>
        /// </summary>
        Follow,

        /// <summary>
        /// </summary>
        Custom,

        /// <summary>
        /// Uses the raw CFrame of the camera.
        /// </summary>
        Scriptable
    }
}