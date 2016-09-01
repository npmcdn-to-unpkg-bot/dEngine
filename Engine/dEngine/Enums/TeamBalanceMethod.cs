// TeamBalanceMethod.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Algorthims for balancing teams.
    /// </summary>
    public enum TeamBalanceMethod
    {
        /// <summary>
        /// Randomize teams.
        /// </summary>
        Random,

        /// <summary>
        /// Attempt to fairly balance teams using the TrueSkill algorithm.
        /// </summary>
        TrueSkill
    }
}