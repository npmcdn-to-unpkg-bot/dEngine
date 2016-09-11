// ReplicatedStorage.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Instances.Attributes;

namespace dEngine.Services
{
    /// <summary>
    /// A storage service that replicates its descendants.
    /// </summary>
    /// <remarks>
    /// Objects stored in this container will be baked into both the client and the server files.
    /// </remarks>
    [TypeId(9)]
    [ExplorerOrder(6)]
    [ToolboxGroup("Containers")]
    public sealed class ReplicatedStorage : Service
    {
        /// <summary/>
        public ReplicatedStorage()
        {
            Service = this;
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<ReplicatedStorage>();
        }

        internal static ReplicatedStorage Service;
    }
}