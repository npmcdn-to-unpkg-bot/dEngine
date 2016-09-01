// RenderConstants.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX.DXGI;

namespace dEngine.Graphics
{
    internal static class RenderConstants
    {
        internal const int FrameCount = 2;
        internal const Format BackBufferFormat = Format.R8G8B8A8_UNorm_SRgb;
        internal const int MaterialCount = 18;
        internal const int MaxBonesPerVertex = 4;
    }
}