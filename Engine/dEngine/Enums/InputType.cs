// InputType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Enum for input types.
    /// </summary>
    public enum InputType
    {
        None = 0,
        MouseButton1 = 1,
        MouseButton2 = 2,
        MouseButton3 = 3,
        MouseWheel = 4,
        MouseMovement = 5,
        Touch = 7,
        Keyboard = 8,
        Focus = 9,
        Accelerometer = 10,
        Gyro = 11,
        GamepadButton = 12,
        GamepadLeftStick = 13,
        GamepadRightStick = 14,
        GamepadLeftTrigger = 15,
        GamepadRightTrigger = 16,
        MouseButton4 = 17,
        MouseButton5 = 18
    }
}