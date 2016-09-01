// NodeSlotAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Materials;

#pragma warning disable 1591

namespace dEngine.Instances.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NodeSlotAttribute : Attribute
    {
        public NodeSlotAttribute(string displayName, SlotType type, InOut mode)
        {
            DisplayName = displayName;
            Type = type;
            Mode = mode;
        }

        public string DisplayName { get; set; }
        public SlotType Type { get; set; }
        public InOut Mode { get; set; }
        public MaterialDomain[] DisableDomains { get; set; }
        public ShadingModel[] DisabledModels { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NodeSlotColourAttribute : Attribute
    {
        public NodeSlotColourAttribute(int r, int g, int b)
        {
            Colour = Colour.fromRGB(r, g, b);
        }

        public Colour Colour { get; set; }
    }
}