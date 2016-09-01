// PartOperation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Data;
using dEngine.Instances.Attributes;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace dEngine.Instances
{
    /// <summary>
    /// A base class for CSG operations.
    /// </summary>
    // TODO: redo CSG
    [TypeId(141)]
    [ToolboxGroup("Bricks")]
    public abstract class PartOperation : Part
    {
        private bool _applyMaterial;
        private CollisionFidelity _collisionFidelity;
        private Geometry _geometry;
        private StaticMesh _staticMesh;
        private MemoryStream _stream;

        /// <inheritdoc />
        protected PartOperation()
        {
            _stream = new MemoryStream();
            _applyMaterial = false;
            _collisionFidelity = CollisionFidelity.ConvexHull;
        }

        /// <summary>
        /// Determines whether <see cref="Part.Material" /> and <see cref="Part.BrickColour" /> properties are applied.
        /// </summary>
        public bool ApplyMaterial
        {
            get { return _applyMaterial; }
            set
            {
                if (value == _applyMaterial)
                    return;

                _applyMaterial = value;
                NotifyChanged(nameof(ApplyMaterial));
            }
        }

        /// <summary>
        /// Determines the quality of the collision mesh for this operation.
        /// </summary>
        public CollisionFidelity CollisionFidelity
        {
            get { return _collisionFidelity; }
            set
            {
                if (value == _collisionFidelity)
                    return;

                _collisionFidelity = value;
                NotifyChanged(nameof(CollisionFidelity));
            }
        }

        internal class CSGPair
        {
            internal CSGPair()
            {
            }

            internal CSGPair(Part a, Part b)
            {
                A = a;
                B = b;
            }

            [InstMember(1)] internal readonly Part A;
            [InstMember(2)] internal readonly Part B;
        }
    }
}