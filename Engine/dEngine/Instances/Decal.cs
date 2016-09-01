// Decal.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Graphics.Structs;
using dEngine.Instances.Attributes;
using dEngine.Instances.Materials;
using dEngine.Instances.Materials.Nodes;

namespace dEngine.Instances
{
    /// <summary>
    /// Projects a texture onto a surface.
    /// </summary>
    [TypeId(13)]
    [ToolboxGroup("Brick equipment")]
    [ExplorerOrder(4)]
    public class Decal : PVInstance
    {
        internal static Material DecalMaterial;
        internal static MaterialInstance DefaultMaterial;
        private Part _parentPart;

        private MaterialBase _material;

        private Content<Texture> _textureId;

        static Decal()
        {
            DecalMaterial = new Material();
            var textureSample = new TextureParameterNode();
        }

        /// <summary />
        public Decal()
        {
            var material = DecalMaterial;
            Material = DefaultMaterial;
        }

        /// <summary>
        /// The material to apply to the decal.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public MaterialBase Material
        {
            get { return _material; }
            set
            {
                if (value == _material) return;
                if (value.Domain != MaterialDomain.Decal)
                {
                    Logger.Warn($"Material \"{value}\" cannot be used as on decals.");
                    return;
                }
                _material = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Comment
        /// </summary>
        [InstMember(2)]
        [Obsolete("Use the Material property instead.")]
        public Content<Texture> TextureId
        {
            get { return _textureId; }
            set
            {
                if (value == _textureId)
                    return;
                _textureId = value;
                NotifyChanged();
            }
        }

        public override CFrame CFrame { get; set; }
        public override Vector3 Size { get; set; }

        /// <inheritdoc />
        protected override void OnAncestryChanged(Instance child, Instance parent)
        {
            base.OnAncestryChanged(child, parent);
            if (child != this)
                return;

            _parentPart = parent as Part;

            ChangeRenderObject();
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
        }

        #region IRenderable

        /// <inheritdoc />
        public RenderObject RenderObject { get; set; }

        /// <inheritdoc />
        public int RenderIndex { get; set; }

        /// <inheritdoc />
        public InstanceRenderData RenderData { get; set; }

        /// <inheritdoc />
        public void UpdateRenderData()
        {
        }

        private void ChangeRenderObject()
        {
        }

        #endregion
    }
}