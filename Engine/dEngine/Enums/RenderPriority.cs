// RenderPriority.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    public enum RenderPriority
    {
        First = 0,
        Input = 100,
        Camera = 200,
        Character = 300,
        Last = 2000
    }
}