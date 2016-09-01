// WindowMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    public enum WindowMode
    {
        /// <summary>
        /// The game will be in fullscreen mode. This has improved performance over the other modes.
        /// </summary>
        Fullscreen,

        /// <summary>
        /// The game will be in a regular, resizeable window.
        /// </summary>
        Windowed,

        /// <summary>
        /// The game will be in a borderless window and take up the whole screen, but will not enter proper fullscreen mode.
        /// This may make switching between windows easier, but comes at a performance cost.
        /// </summary>
        Borderless
    }
}