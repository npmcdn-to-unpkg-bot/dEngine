// TypeIdAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// An attribute for specifying the sub-type ID for classes which inherit from <see cref="Instance" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TypeIdAttribute : Attribute
    {
        /// <inheritdoc />
        public TypeIdAttribute(short typeId)
        {
            Id = typeId;
        }

        /// <summary>
        /// The ID.
        /// </summary>
        public short Id { get; }
    }
}