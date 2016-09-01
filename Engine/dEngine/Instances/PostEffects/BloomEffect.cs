// BloomEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Data;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// An effect which makes bright parts of the screen glow.
    /// </summary>
    [TypeId(174)]
    [ExplorerOrder(0)]
    public sealed class BloomEffect : PostEffect
    {
        private Colour _dirtColour;
        private Content<Texture> _dirtMask;
        private float _dirtIntensity;
        private float _intensity;
        private float _size;
        private float _threshold;

        /// <inheritdoc />
        public BloomEffect()
        {
            _dirtMask = new Content<Texture>();
            _effectOrder = (int)EffectPrority.Bloom;
        }

        /// <summary>
        /// The intensity of the bloom.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
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
        /// The size in percent of the screen width.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public float Size
        {
            get { return _size; }
            set
            {
                if (value == _size) return;
                _size = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The size in percent of the screen width.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float Threshold
        {
            get { return _threshold; }
            set
            {
                if (value == _threshold) return;
                _threshold = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The texture to use for the dirty lens effect.
        /// </summary>
        [InstMember(4)]
        [EditorVisible("Dirt")]
        public Content<Texture> DirtMask
        {
            get { return _dirtMask; }
            set
            {
                _dirtMask = value;
                value.Subscribe(this, OnDirtMaskFetched);
                NotifyChanged();
            }
        }

        /// <summary>
        /// The intensity of the dirty lens effect.
        /// </summary>
        [InstMember(5)]
        [EditorVisible]
        public float DirtIntensity
        {
            get { return _intensity; }
            set
            {
                if (value == _dirtIntensity) return;
                _dirtIntensity = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The intensity of the dirty lens effect.
        /// </summary>
        [InstMember(6)]
        [EditorVisible]
        public Colour DirtColour
        {
            get { return _dirtColour; }
            set
            {
                if (value == _dirtColour) return;
                _dirtColour = value;
                NotifyChanged();
            }
        }

        private void OnDirtMaskFetched(string id, Texture texture)
        {
            throw new NotImplementedException();
        }

        internal override void Render(ref DeviceContext context)
        {
            //throw new NotImplementedException();
        }
    }
}