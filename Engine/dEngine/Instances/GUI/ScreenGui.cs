// ScreenGui.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A gui container for elements to be drawn to the screen.
    /// </summary>
    /// <remarks>
    /// A <see cref="ScreenGui" /> must be parented to a <see cref="Camera" /> to be rendered.
    /// </remarks>
    [TypeId(67)]
    [ToolboxGroup("GUI")]
    [ExplorerOrder(14)]
    public class ScreenGui : LayerCollector
    {
    }
}