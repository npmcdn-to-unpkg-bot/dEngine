// ContentType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Enum for content types.
    /// </summary>
    public enum ContentType : byte
    {
        Unknown,
        StaticMesh,
        SkeletalMesh,
        Model,
        Texture,
        Sound,
        Animation,
        Text,
        Cubemap,
        Video,
        Material,
        Instance
    }
}