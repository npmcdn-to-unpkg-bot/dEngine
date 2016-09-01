// ScrollBarVisibility.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enumeration for ScrollBar visiblity.
    /// </summary>
    public enum ScrollBarVisibility
    {
        /// <summary>
        /// The scrollbar is only visible when content overflows.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Unused.
        /// </summary>
        Disabled = 1,

        /// <summary>
        /// The scroll bar is never visible, and margins are not applied to the content.
        /// </summary>
        Hidden = 2,

        /// <summary>
        /// The scrollbar is always visible.
        /// </summary>
        Visible = 3
    }
}