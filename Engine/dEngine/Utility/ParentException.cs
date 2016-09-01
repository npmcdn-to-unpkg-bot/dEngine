// ParentException.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;

namespace dEngine.Utility
{
    /// <summary>
    /// An exception which is thrown when <see cref="Instance.Parent" /> is set to an invalid state.
    /// </summary>
    public class ParentException : Exception
    {
        /// <summary>
        /// Creates a new exception.
        /// </summary>
        public ParentException(string message) : base(message)
        {
        }
    }
}