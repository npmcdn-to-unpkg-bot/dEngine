// ExplorerOrderAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

#pragma warning disable 1591

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// The order of the instance in the explorer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExplorerOrderAttribute : Attribute
    {
        public int Order;

        public ExplorerOrderAttribute(int index)
        {
            Order = index;
        }
    }
}