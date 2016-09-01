// PlayerGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Container for the player's GUIs.
    /// </summary>
    [TypeId(193)]
    [ExplorerOrder(3)]
    [Uncreatable]
    public class PlayerGui : GuiContainerBase
    {
        internal void FetchStarterGuis()
        {
            foreach (var gui in Game.StarterGui.Children)
                gui.Clone().Parent = this;
        }
    }
}