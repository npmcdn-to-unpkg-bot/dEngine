// SimulationState.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// The session state.
    /// </summary>
    public enum SimulationState
    {
        /// <summary>
        /// The session is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The session is running.
        /// </summary>
        Running,

        /// <summary>
        /// The session is paused.
        /// </summary>
        Paused
    }
}