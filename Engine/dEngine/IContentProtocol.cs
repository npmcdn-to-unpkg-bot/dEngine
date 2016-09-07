// IContentProtocol.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// Interface for custom content protocols.
    /// </summary>
    public interface IContentProtocol
    {
        /// <summary>
        /// Return a stream from the given content path.
        /// </summary>
        Stream Fetch(Uri uri);
    }
}