// ContentIdAttribute.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// An attribute for telling the level editor what this string property represents a content url.
    /// </summary>
    [LevelEditorRelated]
    public class ContentIdAttribute : Attribute
    {
        /// <summary>
        /// </summary>
        public ContentIdAttribute(ContentType type)
        {
            ContentType = type;
        }

        /// <summary>
        /// The type of content this string represents.
        /// </summary>
        public ContentType ContentType { get; }
    }
}