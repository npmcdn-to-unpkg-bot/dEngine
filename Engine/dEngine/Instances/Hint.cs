// Hint.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Displays a black text bar at the top of the screen.
    /// </summary>
    [TypeId(228)]
    [ExplorerOrder(1)]
    [Obsolete]
    public sealed class Hint : Message
    {
        /// <summary />
        public Hint()
        {
            _label.BackgroundColour = Colour.Black;
            _label.TextColour = Colour.White;
            _label.TextStrokeColour = Colour.Transparent;
            _label.Size = new UDim2(1, 0, 0, 20);
        }
    }
}