// ITextureFormat.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX.Direct3D11;

namespace dEngine.Utility.Texture
{
    internal interface ITextureFormat
    {
        Texture2D Texture2D { get; }
    }
}