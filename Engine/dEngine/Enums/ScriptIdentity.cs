// ScriptIdentity.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

#pragma warning disable 1591

namespace dEngine
{
    [Flags]
    public enum ScriptIdentity
    {
        Clr = 1 << 0,
        Script = 1 << 1,
        Mod = 1 << 2,
        Plugin = 1 << 3,
        Editor = 1 << 4,
        CoreScript = 1 << 5,
        Server = 1 << 56
    }
}