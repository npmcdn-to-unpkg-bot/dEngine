// Sky.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Graphics.Structs;
using dEngine.Instances.Attributes;
using dEngine.Services;
using dEngine.Utility;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace dEngine.Instances
{
    /// <summary>
    /// An object which draws the sky.
    /// </summary>
    [TypeId(81)]
    [ToolboxGroup("Gameplay")]
    [ExplorerOrder(0)]
    public class Sky : Instance, IRenderable
    {
        private Content<Cubemap> _skyboxContent;

        /// <inheritdoc />
        public Sky()
        {
            _skyboxContent = new Content<Cubemap>();

            CubemapId = "internal://textures/sky512.png";

            // this could be pre-computed
            Renderer.InvokeResourceDependent(GenerateStarfield);

            Changed.Event += propertyName =>
            {
                if (propertyName == nameof(Parent))
                {
                    var lighting = Parent as Lighting;
                    if (lighting != null)
                        lighting.Skybox = this;
                    else if (Game.Lighting.Skybox == this)
                        Game.Lighting.Skybox = null;
                }
            };
        }

        internal Cubemap IrradianceMap { get; private set; }
        internal Cubemap RadianceMap { get; private set; }

        /// <summary>
        /// The cubemap texture.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        [ContentId(ContentType.Texture)]
        public Content<Cubemap> CubemapId
        {
            get { return _skyboxContent; }
            set
            {
                _skyboxContent = value;
                value.Subscribe(this, (s, cubemap) =>
                {
                    //IrradianceMap = CubemapFiltering.GetIrradianceMap(cubemap);
                    //RadianceMap = CubemapFiltering.GetRadianceMap(cubemap);
                });
                NotifyChanged();
            }
        }

        /// <summary>
        /// The texture for the stars at night.
        /// </summary>
        internal Texture Starfield { get; private set; }

        internal Cubemap Cubemap => _skyboxContent.Asset;
        
        private void GenerateStarfield()
        {
            Starfield?.Dispose();

            var data = new DataRectangle[6];

            for (var i = 0; i < 6; i++)
                data[i] = Noise.Render(1024, 1024);

            Starfield = new Texture(new Texture2D(Renderer.Device, new Texture2DDescription
            {
                Width = 1024,
                Height = 1024,
                ArraySize = 6,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.TextureCube,
                SampleDescription = new SampleDescription(1, 0)
            }, data));
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();

            Cubemap?.Dispose();
            Starfield?.Dispose();
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
            var camera = Game.Workspace?.CurrentCamera;

            var cf = camera == null
                ? CFrame.Identity
                : new CFrame(camera.CFrame.p);

            RenderData = new InstanceRenderData
            {
                Size = new Vector3(-100),
                Colour = Colour.Black,
                ModelMatrix = cf.GetModelMatrix(),
                ShadingModel = ShadingModel.Unlit,
                Emissive = 0
            };

            RenderObject?.UpdateInstance(this);
        }

        #endregion
    }
}