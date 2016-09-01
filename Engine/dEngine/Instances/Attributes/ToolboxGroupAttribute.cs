// ToolboxGroupAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// Attribute for group names in the toolbox.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ToolboxGroupAttribute : Attribute, IValueAttribute
    {
        /// <summary>
        /// Sets the group name.
        /// </summary>
        public ToolboxGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }

        /// <summary>
        /// The group name.
        /// </summary>
        public string GroupName { get; }

        string IValueAttribute.GetValue()
        {
            return GroupName;
        }
    }
}