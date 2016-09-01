// IntersectOperation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A part representing a Subtract CSG operation - a portion common to both objects.
    /// </summary>
    [TypeId(144)]
    [Uncreatable]
    [ExplorerOrder(3)]
    public sealed class IntersectOperation : PartOperation
    {
    }
}