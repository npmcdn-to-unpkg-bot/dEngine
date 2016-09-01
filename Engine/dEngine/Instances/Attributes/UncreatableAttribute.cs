// UncreatableAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// Prevents scripts from creating an instance of this type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UncreatableAttribute : Attribute
    {
    }
}