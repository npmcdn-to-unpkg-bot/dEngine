// UnreliableSizeBehaviour.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Enum of behaviours for large, unreliable messages.
    /// </summary>
    public enum UnreliableSizeBehaviour
    {
        /// <summary>
        /// Sending an unreliable message will ignore MTU and send everything in a single packet.
        /// </summary>
        IgnoreMTU = 0,

        /// <summary>
        /// Use normal fragmentation for unreliable messages - if a fragment is dropped, memory for received fragments are never
        /// reclaimed.
        /// </summary>
        NormalFragmentation = 1,

        /// <summary>
        /// Alternate behaviour; just drops unreliable messages above MTU.
        /// </summary>
        DropAboveMTU = 2
    }
}