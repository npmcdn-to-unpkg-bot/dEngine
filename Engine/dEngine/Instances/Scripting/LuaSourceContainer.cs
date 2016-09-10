// LuaSourceContainer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for an object containing Lua source code.
    /// </summary>
    [TypeId(209)]
    public abstract class LuaSourceContainer : LinkedSourceContainer
    {
        /// <summary />
        protected LuaSourceContainer()
        {
            Source = "print 'Hello world!'";
        }

        /// <summary>
        /// Gets whether or not the script is currently running code.
        /// </summary>
        public bool IsRunning { get; internal set; }

        internal Player CurrentEditor { get; set; }

        internal LuaThread LuaThread { get; set; }
        internal ScriptDebugger Debugger { get; set; }

        internal ScriptIdentity Identity { get; set; }

        internal void Print(string message, LogLevel level)
        {
            Logger.Log(message, level);
        }
    }
}