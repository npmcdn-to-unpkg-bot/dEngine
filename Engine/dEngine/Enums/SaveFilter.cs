// SaveFilter.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances;
using dEngine.Services;

namespace dEngine
{
    /// <summary>
    /// Enum for filtering the save of <see cref="DataModel" /> and <see cref="Workspace" />
    /// </summary>
    public enum SaveFilter
    {
        /// <summary>
        /// Only save the Workspace.
        /// </summary>
        SaveWorld,

        /// <summary>
        /// Only save the DataModel.
        /// </summary>
        SaveGame,

        /// <summary>
        /// Save DataModel with Workspace to the same stream. (No callbacks invoked.)
        /// </summary>
        SaveTogether
    }
}