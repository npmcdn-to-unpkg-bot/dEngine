// InputMethod.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum for mouse input methods.
    /// </summary>
    public enum InputMethod
    {
        /// <summary>
        /// Uses WinForms events. (Win32 API)
        /// </summary>
        Windows,

        /// <summary>
        /// Uses the DirectInput API.
        /// </summary>
        DirectInput,

        /// <summary>
        /// Uses the RawInput API.
        /// </summary>
        RawInput
    }
}