// CustomMessageType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// Enum for custom network messages.
    /// </summary>
    public enum CustomMessageType
    {
        Unknown = 0,

        /// <summary>
        /// A request to replicate an object
        /// </summary>
        ReplicateObject = 1,

        /// <summary>
        /// A request to replicate a property
        /// </summary>
        ReplicateProperty = 2,

        /// <summary></summary>
        RequestObject = 4,

        /// <summary>
        /// A request to fire a <see cref="RemoteEvent" />.
        /// </summary>
        RemoteEventFire = 5,

        /// <summary>
        /// A request to invoke a <see cref="RemoteFunction" />.
        /// </summary>
        RemoteFunctionInvoke = 6,

        /// <summary>
        /// A response to a <see cref="RemoteFunctionInvoke" />.
        /// </summary>
        RemoteFunctionReturn = 7
    }
}