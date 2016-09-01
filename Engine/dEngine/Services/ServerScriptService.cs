// ServerScriptService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEngine.Services
{
    /// <summary>
    /// A container service who executes scripts on the server.
    /// </summary>
    /// <remarks>
    /// Objects stored in this container will not be baked into the client.
    /// </remarks>
    /// <seealso cref="Workspace" />
    [TypeId(79)]
    [ExplorerOrder(6)]
    [ToolboxGroup("Containers")]
    public sealed class ServerScriptService : Service
    {
        internal static ServerScriptService Service;
        
        /// <summary/>
        public ServerScriptService()
        {
            Service = this;
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<ServerScriptService>();
        }
    }
}