// BlockMesh.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A BlockMesh changes the appearance of its parent <see cref="Part" />, regardless of <see cref="Part.Size" /> and
    /// <see cref="Part.Shape" /> properties.
    /// </summary>
    [TypeId(27)]
    [ExplorerOrder(3)]
    public sealed class BlockMesh : Mesh
    {
        /// <inheritdoc />
        public BlockMesh()
        {
            _geometry = Primitives.CubeGeometry;
            UsePartSize = true;
        }
    }
}