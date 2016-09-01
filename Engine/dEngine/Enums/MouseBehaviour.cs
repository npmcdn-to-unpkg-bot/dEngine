// MouseBehaviour.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum for mouse behaviour.
    /// </summary>
    public enum MouseBehaviour
    {
        /// <summary>
        /// Cursor can move freely.
        /// </summary>
        Default,

        /// <summary>
        /// Cursor is locked to the center of the screen.
        /// </summary>
        LockCenter,

        /// <summary>
        /// Cursor is locked at its current position.
        /// </summary>
        LockCurrentPosition
    }
}