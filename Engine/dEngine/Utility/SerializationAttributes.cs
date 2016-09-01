// SerializationAttributes.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

#pragma warning disable 1591

namespace dEngine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InstMemberAttribute : Attribute
    {
        public InstMemberAttribute(short id)
        {
            Tag = id;
        }

        public short Tag { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InstBeforeSerializationAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InstAfterSerializationAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InstBeforeDeserializationAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InstAfterDeserializationAttribute : Attribute
    {
    }
}