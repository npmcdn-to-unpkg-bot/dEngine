// SimulationType.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Types of simulation.
    /// </summary>
    public enum SimulationType
    {
        /// <summary>
        /// The game is being simulated as a player.
        /// </summary>
        Play,

        /// <summary>
        /// The game is being simulated as a server.
        /// </summary>
        Run
    }
}