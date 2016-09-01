// ModuleScript.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// A script which returns a module object when passed as an argument to require()
    /// </summary>
    [TypeId(56)]
    [ToolboxGroup("Scripting")]
    [ExplorerOrder(5)]
    public sealed class ModuleScript : LuaSourceContainer
    {
        /// <inheritdoc />
        public ModuleScript()
        {
            Source = "local module = {in.\n\nreturn module\n\n";

            RunService.Service.SimulationEnded.Connect(ResetReturnValue);
        }

        internal LuaResult ReturnValue { get; set; }

        private void ResetReturnValue()
        {
            ReturnValue = null;
        }

        /// <summary />
        public override void Destroy()
        {
            base.Destroy();
            RunService.Service.SimulationEnded.Disconnect(ResetReturnValue);
        }
    }
}