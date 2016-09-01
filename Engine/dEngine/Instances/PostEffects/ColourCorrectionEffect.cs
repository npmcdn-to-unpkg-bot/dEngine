// ColourCorrectionEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Data;
using dEngine.Graphics;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Colour correction effect.
    /// </summary>
    [TypeId(176)]
    [ExplorerOrder(0)]
    public sealed class ColourCorrectionEffect : PostEffect
    {
        private readonly GfxShader.Pass _correctionPass = Shaders.Get("PostProcess").GetPass("GammaCorrection");

        private Content<Texture> _lookupId;

        private Colour _tintColour;

        private float _toeFactor;

        private float _intensity;

        /// <summary />
        public ColourCorrectionEffect()
        {
            _effectOrder = (int)EffectPrority.ColourCorrection;
        }

        /// <summary>
        /// The colour filter that is applied to the HDR scene colour.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Tone Mapping")]
        public Colour TintColour
        {
            get { return _tintColour; }
            set
            {
                if (value == _tintColour) return;
                _tintColour = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Summary
        /// </summary>
        [InstMember(2)]
        [EditorVisible("Tone Mapping")]
        [Range(0, 1)]
        public float ToeFactor
        {
            get { return _toeFactor; }
            set
            {
                if (value == _toeFactor) return;
                _toeFactor = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The intensity of the colour grading.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        [Range(0, 1)]
        public float Intensity
        {
            get { return _intensity; }
            set
            {
                if (value == _intensity) return;
                _intensity = value;
                NotifyChanged();
            }
        }


        /// <summary>
        /// The lookup texture for the colour correction.
        /// </summary>
        [InstMember(4)]
        [EditorVisible("Colour Grading")]
        public Content<Texture> LookupId
        {
            get { return _lookupId; }
            set
            {
                if (value == _lookupId) return;
                _lookupId = value;
                NotifyChanged();
            }
        }

        internal override void Render(ref DeviceContext context)
        {
            context.OutputMerger.SetTargets((DepthStencilView)null, Camera.BackBuffer);
            context.PixelShader.SetShaderResources(0, Camera.Buffer0);
            _correctionPass.Use(ref context);
            context.Draw(4, 0);
        }
    }
}