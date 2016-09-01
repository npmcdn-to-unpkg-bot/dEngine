// PathStatus.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Services;

namespace dEngine
{
    /// <summary>
    /// Enum for the result of <see cref="PathfindingService.ComputeRawPathAsync" />.
    /// </summary>
    public enum PathStatus
    {
        /// <summary>
        /// Path was found successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Path doesn't exist, returns path to closest point.
        /// </summary>
        ClosestNoPath,

        /// <summary>
        /// Goal is out of MaxDistance range, returns path to closest point within MaxDistance.
        /// </summary>
        ClosestOutOfRange,

        /// <summary>
        /// Failed to compute path; the starting point is not empty.
        /// </summary>
        FailStartNotEmpty,

        /// <summary>
        /// Failed to compute path; the finish point is not empty.
        /// </summary>
        FailFinishNotEmpty
    }
}