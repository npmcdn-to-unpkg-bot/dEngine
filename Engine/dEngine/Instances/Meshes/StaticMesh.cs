// StaticMesh.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A regular mesh.
    /// </summary>
    [TypeId(28)]
    [ExplorerOrder(3)]
    public sealed class StaticMesh : FileMesh
    {
        private bool _simulatePhysics;

        /// <summary>
        /// Determines if the parent <see cref="Part" />'s physics model is overridden by the mesh.
        /// </summary>
        [InstMember(1)]
        public bool SimulatePhysics
        {
            get { return _simulatePhysics; }
            set
            {
                if (value == _simulatePhysics)
                    return;

                _simulatePhysics = value;
                InvokeGeometryUpdated();

                NotifyChanged(nameof(SimulatePhysics));
            }
        }
    }
}