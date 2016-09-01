// WordWrapping.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Word wrapping modes.
    /// </summary>
    public enum WordWrapping
    {
        /// <summary>
        /// Indicates that words are broken across lines to avoid text overflowing the layout box.
        /// </summary>
        Wrap = 0,

        /// <summary>
        /// Indicates that words are kept within the same line even when it overflows the layout box. This option is often used
        /// with scrolling to reveal overflow text.
        /// </summary>
        NoWrap = 1,

        /// <summary>
        /// Words are broken across lines to avoid text overflowing the layout box. Emergency wrapping occurs if the word is larger
        /// than the maximum width.
        /// </summary>
        EmergencyBreak = 2,

        /// <summary>
        /// When emergency wrapping, only wrap whole words, never breaking words when the layout width is too small for even a
        /// single word.
        /// </summary>
        WholeWord = 3,

        /// <summary>
        /// Wrap between any valid character clusters.
        /// </summary>
        Character = 4
    }
}