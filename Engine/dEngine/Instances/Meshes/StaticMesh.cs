﻿// StaticMesh.cs - dEngine
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU Lesser General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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