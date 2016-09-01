// PeerType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Enum for peer types.
    /// </summary>
    [Flags]
    public enum PeerType
    {
        Client = 1,
        Server = 2,
        Hybrid = Client | Server
    }
}