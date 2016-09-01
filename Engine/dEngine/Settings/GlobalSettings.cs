// GlobalSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Settings.Global;

#pragma warning disable 1591

namespace dEngine.Settings
{
    /// <summary>
    /// Global settings container.
    /// </summary>
    [TypeId(184)]
    [Uncreatable]
    public class GlobalSettings : GenericSettings, ISingleton
    {
        /// <inheritdoc />
        public GlobalSettings()
        {
            LuaSettings = CreateSettings<LuaSettings>("Lua");
            RenderSettings = CreateSettings<RenderSettings>("Render");
            DebugSettings = CreateSettings<DebugSettings>("Diagnostics");
            NetworkSettings = CreateSettings<NetworkSettings>("Network");
            PhysicsSettings = CreateSettings<PhysicsSettings>("Physics");
            SoundSettings = CreateSettings<SoundSettings>("Sound");
        }

        public LuaSettings LuaSettings { get; }
        public RenderSettings RenderSettings { get; }
        public DebugSettings DebugSettings { get; }
        public NetworkSettings NetworkSettings { get; }
        public PhysicsSettings PhysicsSettings { get; }
        public SoundSettings SoundSettings { get; set; }

        internal static object GetExisting()
        {
            return Engine.Settings;
        }
    }
}