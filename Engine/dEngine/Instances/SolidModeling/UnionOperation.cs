// UnionOperation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// A part representing a Union CSG operation - a merger of two objects into one.
    /// </summary>
    [TypeId(142)]
    [Uncreatable]
    [ExplorerOrder(3)]
    public sealed class UnionOperation : PartOperation
    {
        /// <inheritdoc />
        public UnionOperation()
        {
            SolidModelingManager.Operations.TryAdd(this);
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            SolidModelingManager.Operations.TryRemove(this);
        }
    }
}