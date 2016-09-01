// EditorVisibleAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// Attribute for defining inspector property group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class EditorVisibleAttribute : Attribute
    {
        /// <summary>
        /// Sets the group name in the inspector.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="displayName">The name to display in the inspector.</param>
        /// <param name="data"></param>
        public EditorVisibleAttribute(string group = "Data", string displayName = null, object data = null)
        {
            Group = group;
            DisplayName = displayName;
            Data = data;
        }

        /// <summary>
        /// The group name.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// The group name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary/>
        public object Data { get; }
    }
}