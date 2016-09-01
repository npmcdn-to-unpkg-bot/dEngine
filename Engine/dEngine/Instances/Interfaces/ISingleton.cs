// ISingleton.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine.Instances
{
    /// <summary>
    /// An interface for types which can only have one instance.
    /// Implementations must have a static "GetExisting()" object, which returns the singleton instance.
    /// </summary>
    public interface ISingleton
    {
    }
}