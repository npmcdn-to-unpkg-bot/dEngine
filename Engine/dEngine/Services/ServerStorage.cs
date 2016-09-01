// ServerStorage.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEngine.Services
{
    /// <summary>
    /// A service for storing objects that are not replicated to the client.
    /// </summary>
    /// <remarks>
    /// Objects stored in this container will not be baked into the client.
    /// </remarks>
    [TypeId(89)]
    [ExplorerOrder(6)]
    [ToolboxGroup("Containers")]
    public sealed class ServerStorage : Service
    {
        internal static object GetExisting()
        {
            return DataModel.GetService<ServerStorage>();
        }
    }
}