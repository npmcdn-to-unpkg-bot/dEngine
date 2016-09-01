// RangeAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// An attribute for hinting at the intended range of a property.
    /// </summary>
    public class RangeAttribute : Attribute
    {
        /// <summary />
        public RangeAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public double Max { get; }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public double Min { get; }
    }
}