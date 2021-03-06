﻿// GodRaysEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Instances.Attributes;
using dEngine.Services;
using SharpDX;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Renders sun shafts.
    /// </summary>
    [TypeId(177)]
    [ExplorerOrder(0)]
    public sealed class GodRaysEffect : PostEffect
    {
        private readonly GfxShader.Pass _downsamplePass = Shaders.Get("PostProcess").GetPass("Downsample4x4Avg");
        private readonly GfxShader.Pass _godRaysPass = Shaders.Get("PostProcess").GetPass("GodRays");
        private float _intensity;
        private float _spread;
        private GfxShader.Pass _radialBlurPass = Shaders.Get("PostProcess").GetPass("RadialBlur");
        private Texture _noiseTexture;

        // Params1: XYZ: SunDirection
        // Params2: X: Intensity Y: ? Z: ? W: sunDir Z
        // Params3: XYZ: SunShaftColour
        // Params4: 

        /// <summary />
        public GodRaysEffect()
        {
            if (Renderer.IsInitialized)
                CreateNoiseTexture();
            else
                Renderer.Initialized += CreateNoiseTexture;

            UpdateSun();
            Game.Lighting.LightingChanged.Connect(UpdateSun);
        }

        /// <summary>
        /// The intensity of the sun rays.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        [Range(0, 1)]
        public float Intensity
        {
            get { return _intensity; }
            set
            {
                if (value == _intensity) return;
                _intensity = value;
                //_postEffectConstants.Data.Params2.X = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Determines how much the sun rays spread out.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        [Range(0, 1)]
        public float Spread
        {
            get { return _spread; }
            set
            {
                if (value == _spread) return;
                _spread = value;
                NotifyChanged();
            }
        }

        /// <summary />
        public override void Destroy()
        {
            base.Destroy();
            Game.Lighting.LightingChanged.Disconnect(UpdateSun);
        }

        private void UpdateSun(bool skyboxChanged = false)
        {
            var sunVector = Game.Lighting.SunVector;
            //_postEffectConstants.Data.Params1 = new SharpDX.Vector4(sunVector.x, sunVector.y, sunVector.z, 0);
            //_postEffectConstants.Data.Params3 = new SharpDX.Vector4(1, 1, 1, 1);
        }

        private void CreateNoiseTexture()
        {
            if (IsDestroyed) return;
            _noiseTexture = new Texture();
            _noiseTexture.Load(ContentProvider.DownloadStream(new Uri("internal://textures/noise2.png")).Result);
        }

        internal override void Render(ref DeviceContext context)
        {
            var camera = Camera;
            context.ClearRenderTargetView(camera.BufferDownscaleQuarter0, Color.Zero);
            context.OutputMerger.SetTargets((DepthStencilView)null, camera.BufferDownscaleQuarter0);
            context.PixelShader.SetShaderResources(0, camera.DepthStencilBuffer);
            //context.PixelShader.SetConstantBuffers(1, _postEffectConstants);
            _downsamplePass.Use(ref context);
            context.Draw(4, 0);

            context.ClearRenderTargetView(camera.BufferDownscaleQuarter1, Color.Zero);
            context.OutputMerger.SetTargets((DepthStencilView)null, camera.BufferDownscaleQuarter1);
            context.PixelShader.SetShaderResources(0, camera.BufferDownscaleQuarter0, _noiseTexture);
            _godRaysPass.Use(ref context);
            context.Draw(4, 0);
        }

        /// <summary />
        internal override void UpdateSize(Camera camera)
        {
            base.UpdateSize(camera);
            var w = Camera.BufferDownscaleQuarter0.Width;
            var h = Camera.BufferDownscaleQuarter0.Height;
            //_postEffectConstants.Data.TextureSize = new SharpDX.Vector4(w, h, 0, 0);
        }
    }
}