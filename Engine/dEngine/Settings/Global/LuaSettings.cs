// LuaSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Runtime.CompilerServices;
using dEngine.Instances.Attributes;

namespace dEngine.Settings.Global
{
    /// <summary>
    /// Settings for the Lua runtime.
    /// </summary>
    [TypeId(185)]
    public class LuaSettings : Settings
    {
        private static bool _debugEngineEnabled;
        private static double _defaultWaitTime;
        private static bool _areScriptStartsReported;

        /// <summary>
        /// If true, script starts will be logged.
        /// </summary>
        [EditorVisible("Diagnostics")]
        public static bool AreScriptStartsReported
        {
            get { return _areScriptStartsReported; }
            set
            {
                _areScriptStartsReported = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The amount of time to wait for parameterless wait() calls.
        /// </summary>
        [EditorVisible("Settings")]
        public static double DefaultWaitTime
        {
            get { return _defaultWaitTime; }
            set
            {
                _defaultWaitTime = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Determines whether scripts will be compiled with a debug engine.
        /// </summary>
        [EditorVisible("Debug")]
        public static bool DebugEngineEnabled
        {
            get { return _debugEngineEnabled; }
            set
            {
                _debugEngineEnabled = value;
                NotifyChangedStatic();
            }
        }

        private static void NotifyChangedStatic([CallerMemberName] string propertyName = null)
        {
            Engine.Settings?.LuaSettings?.NotifyChanged(propertyName);
        }

        /// <inheritdoc />
        public override void RestoreDefaults()
        {
            AreScriptStartsReported = false;
            DebugEngineEnabled = true;
            DefaultWaitTime = 0.03;
        }
    }
}