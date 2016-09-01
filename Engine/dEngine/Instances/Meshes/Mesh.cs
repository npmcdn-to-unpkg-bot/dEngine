// Mesh.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Instances.Attributes;
using dEngine.Instances.Materials;
using dEngine.Utility;

namespace dEngine.Instances
{
    /// <summary>
    /// A base class for meshes.
    /// </summary>
    [TypeId(25)]
    [ToolboxGroup("Brick equipment")]
    public abstract class Mesh : Instance
    {
        /// <summary>
        /// The geometry to render.
        /// </summary>
        protected Geometry _geometry;

        private Vector3 _offset;
        private Part _parentPart;
        private Vector3 _scale;
        private bool _usePartSize;

        /// <inheritdoc />
        protected Mesh()
        {
            Material = Material.Smooth;

            Scale = Vector3.One;
            Renderer.Meshes.TryAdd(this);
        }

        internal Geometry Geometry => _geometry;

        /// <summary>
        /// The offset of the mesh from its origin.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Vector3 Offset
        {
            get { return _offset; }
            set
            {
                if (value == _offset) return;
                _offset = value;
                GeometryUpdated?.Invoke();
                NotifyChanged();
            }
        }

        /// <summary>
        /// The scale of the mesh.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                if (value == _scale)
                    return;

                _scale = value;
                GeometryUpdated?.Invoke();
                NotifyChanged(nameof(Scale));
            }
        }

        /// <summary>
        /// Determines whether the mesh uses the parent's <see cref="Part.Size" /> property.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public bool UsePartSize
        {
            get { return _usePartSize; }
            set
            {
                if (value == _usePartSize)
                    return;

                _usePartSize = value;
                GeometryUpdated?.Invoke();
                NotifyChanged(nameof(Scale));
            }
        }

        /// <summary>
        /// The material to use for this mesh.
        /// </summary>
        [InstMember(4)]
        [EditorVisible]
        [ContentId(ContentType.Material)]
        public Material Material { get; set; }

        /// <inheritdoc />
        protected override void OnAncestryChanged(Instance child, Instance parent)
        {
            base.OnAncestryChanged(child, parent);

            if (child == this)
            {
                if (_parentPart != null)
                    _parentPart.ChildMesh = null;

                _parentPart = parent as Part;
                if (_parentPart != null)
                    _parentPart.ChildMesh = this;
            }
        }

        /// <summary>
        /// </summary>
        protected void InvokeGeometryUpdated()
        {
            GeometryUpdated?.Invoke();
        }

        /// <summary>
        /// </summary>
        internal void SetGeometry(Geometry geometry)
        {
            _geometry = geometry;

            Material = _geometry?.Material;

            GeometryChanged?.Invoke(geometry);
        }

        internal event Action GeometryUpdated;
        internal event Action<Geometry> GeometryChanged;
    }
}