// MetaData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.Generic;

namespace dEngine.Instances
{
    /// <summary>
    /// Interface for objects to provide metadata which can be read without deserializing the entire hierarchy.
    /// </summary>
    public interface IMetaData
    {
        /// <summary>
        /// Returns a dictionary of metadata.
        /// </summary>
        Dictionary<string, string> GetMetaData();
    }
}