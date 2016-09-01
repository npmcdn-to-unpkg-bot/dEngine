// WarningAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// An attribute which defines a warning in the documentation.
    /// </summary>
    [AttributeUsage(
         AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Struct,
         AllowMultiple = true, Inherited = false)]
    public class WarningAttribute : Attribute
    {
        /// <summary />
        public WarningAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}