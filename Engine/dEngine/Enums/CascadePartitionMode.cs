// CascadePartitionMode.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// The cascade partitioning method.
    /// </summary>
    public enum CascadePartitionMode
    {
        /// <summary>
        /// Use manually defined splits.
        /// </summary>
        Manual,

        /// <summary>
        /// Use a logarithmic algorithm to determine splits.
        /// </summary>
        Logarithmic,

        /// <summary>
        /// Use the PSSM algorithm to determine splits.
        /// </summary>
        ParallelSplit
    }
}