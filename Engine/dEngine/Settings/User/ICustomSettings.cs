// ICustomSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using Neo.IronLua;

#pragma warning disable 1591

namespace dEngine.Settings.User
{
    public interface ICustomSettings
    {
        LuaTable GetCustomSettings();
    }
}