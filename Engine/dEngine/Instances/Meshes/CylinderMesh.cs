// CylinderMesh.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A CylinderMesh changes the appearance of its parent <see cref="Part" />, regardless of <see cref="Part.Size" /> and
    /// <see cref="Part.Shape" /> properties.
    /// </summary>
    [TypeId(166)]
    [ExplorerOrder(3)]
    public sealed class CylinderMesh : Mesh
    {
        /// <inheritdoc />
        public CylinderMesh()
        {
            _geometry = Primitives.CylinderGeometry;
            UsePartSize = true;
        }
    }
}