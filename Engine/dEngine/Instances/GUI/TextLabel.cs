// TextLabel.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A gui element which draws text.
    /// </summary>
    [TypeId(70)]
    [ToolboxGroup("GUI")]
    [ExplorerOrder(19)]
    public class TextLabel : TextElement
    {
        /// <inheritdoc />
        public TextLabel()
        {
            Size = new UDim2(0, 200, 0, 50);
        }
    }
}