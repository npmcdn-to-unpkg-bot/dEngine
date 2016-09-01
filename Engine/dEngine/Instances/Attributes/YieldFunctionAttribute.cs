// YieldFunctionAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// Marker for methods which yield.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class YieldFunctionAttribute : Attribute
    {
    }
}