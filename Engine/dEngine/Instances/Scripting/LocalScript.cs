// LocalScript.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;

namespace dEngine.Instances
{
    /// <summary>
    /// A script which is executed by the client.
    /// </summary>
    /// <remarks>
    /// A <see cref="LocalScript" /> must be a descendant of <see cref="Players.LocalPlayer" /> or its character.
    /// </remarks>
    [TypeId(109)]
    [ToolboxGroup("Scripting")]
    [ExplorerOrder(4)]
    public sealed class LocalScript : Script
    {
        /// <inheritdoc />
        protected override bool CheckIfAncestorValid()
        {
            return IsDescendantOf(Players.Service.LocalPlayer) || IsDescendantOf(Players.Service.LocalPlayer?.Character);
        }
    }
}