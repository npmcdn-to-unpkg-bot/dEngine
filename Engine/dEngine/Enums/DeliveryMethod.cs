// DeliveryMethod.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Networking delivery methods.
    /// </summary>
    public enum DeliveryMethod
    {
        /// <summary></summary>
        Unknown = 0,

        /// <summary>
        /// No guarantees, except for preventing duplicates.
        /// </summary>
        Unreliable = 1,

        /// <summary>
        /// Late messages will be dropped if newer ones were already received.
        /// </summary>
        UnreliableSequenced = 2,

        /// <summary>
        /// All messages will arrive, but not necessarily in the same order.
        /// </summary>
        ReliableUnordered = 34,

        /// <summary>
        /// All messages will arrive, but late ones will be dropped.
        /// </summary>
        ReliableSequenced = 35,

        /// <summary>
        /// All messages will arrive, and they will do so in the same order.
        /// </summary>
        ReliableOrdered = 67
    }
}