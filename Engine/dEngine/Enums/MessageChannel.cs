// MessageChannel.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;

namespace dEngine
{
    /// <summary>
    /// Enum for message networking channels.
    /// </summary>
    public enum MessageChannel
    {
        /// <summary>
        /// The standard channel.
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Channel for replication messages.
        /// </summary>
        Replication = 1,

        /// <summary>
        /// Channel for <see cref="RemoteFunction" /> and <see cref="RemoteEvent" />.
        /// </summary>
        Remote = 2
    }
}