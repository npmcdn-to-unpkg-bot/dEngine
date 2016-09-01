// FileMesh.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Data;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary/>
    [TypeId(26)]
    public abstract class FileMesh : Mesh
    {
        private Content<Geometry> _meshContent;

        /// <inheritdoc />
        protected FileMesh()
        {
            _meshContent = new Content<Geometry>();
        }

        /// <summary>
        /// The content ID of the mesh data.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public Content<Geometry> MeshId
        {
            get { return _meshContent; }
            set
            {
                _meshContent = value;
                value.Subscribe(this, (id, asset) => { SetGeometry(asset); });
            }
        }
    }
}