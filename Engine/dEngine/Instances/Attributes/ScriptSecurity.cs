// ScriptSecurity.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// Defines the required script identity to access this member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    [Serializable]
    public class ScriptSecurityAttribute : Attribute, IValueAttribute
    {
        /// <summary />
        internal ScriptSecurityAttribute(ScriptIdentity identity)
        {
            Identity = identity;
        }

        /// <summary />
        public ScriptIdentity Identity { get; }

        string IValueAttribute.GetValue()
        {
            return Identity.ToString();
        }
    }
}