// Slot.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace dEngine.Instances.Materials
{
    public class Slot
    {
        private static readonly Colour DefaultColour = new Colour(0, 0, 0);

        internal Slot(Node node, string name, SlotType type, InOut mode, Colour? colour = null)
        {
            Name = name;
            Colour = colour ?? DefaultColour;
            Type = type;
            Mode = mode;
            Node = node;

            Index = mode == InOut.In ? node.InSlotCount++ : node.OutSlotCount++;
        }

        /// <summary>
        /// The node this slot is attached to.
        /// </summary>
        public Node Node { get; }

        internal int Index { get; }

        internal Slot Input { get; set; }
        internal Slot Output { get; set; }

        public string Name { get; }
        public Colour Colour { get; }
        public SlotType Type { get; }
        public InOut Mode { get; }

        public bool ConnectTo([CanBeNull] Slot output)
        {
            if (Mode != InOut.Out)
                throw new InvalidOperationException("Input slots cannot be connected.");

            if (output != null)
            {
                if (output.Mode != InOut.In)
                    return false;
                output.Input = this;
                output.Node.SlotConnected.Fire(output);
            }

            Output = output;
            Node.SlotConnected.Fire(this);

            return true;
        }
    }
}