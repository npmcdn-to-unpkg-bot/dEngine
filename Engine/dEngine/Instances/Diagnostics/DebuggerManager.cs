// DebuggerManager.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using dEngine.Instances.Attributes;
using dEngine.Settings.Global;

namespace dEngine.Instances.Diagnostics
{
    /// <summary>
    /// Manager for <see cref="ScriptDebugger" />s.
    /// </summary>
    [Uncreatable]
    [TypeId(216)]
    public class DebuggerManager : Instance
    {
        private readonly object _locker = new object();
        private readonly List<ScriptDebugger> _debuggers;

        /// <summary />
        public DebuggerManager()
        {
            _debuggers = new List<ScriptDebugger>();
        }

        /// <summary>
        /// Gets whether or not debugging is enabled.
        /// </summary>
        public bool DebuggingEnabled => LuaSettings.DebugEngineEnabled;

        /// <summary>
        /// Registers a script with a debugger.
        /// </summary>
        public ScriptDebugger AddDebugger(LuaSourceContainer script)
        {
            var debugger = new ScriptDebugger(script);
            lock (_locker)
            {
                _debuggers.Add(debugger);
            }
            return debugger;
        }

        /// <summary>
        /// Returns an array of all <see cref="ScriptDebugger" />s.
        /// </summary>
        public ScriptDebugger[] GetDebuggers()
        {
            lock (_locker)
            {
                return _debuggers.ToArray();
            }
        }

        /// <summary>
        /// Resumes the debug manager.
        /// </summary>
        public void Resume()
        {
        }

        /// <summary>
        /// Steps into the current line.
        /// </summary>
        public void StepIn()
        {
        }

        /// <summary>
        /// Steps out of the current line.
        /// </summary>
        public void StepOut()
        {
        }

        /// <summary>
        /// Step over the current line.
        /// </summary>
        public void StepOver()
        {
        }
    }
}