// DropShadowEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Graphics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;

namespace dEngine.Instances
{
    internal class DropShadowEffect : IDisposable
    {
        private readonly AffineTransform2D _affineEffect;
        private readonly Composite _compositeEffect;
        private readonly Shadow _shadowEffect;
        private float _blurSize;
        private Colour _colour;
        private Image _inputImage;
        private float _shadowOpacity;
        private Matrix3x2 _transform;

        public DropShadowEffect()
        {
            _shadowOpacity = 1;

            _shadowEffect = new Shadow(Renderer.Context2D);
            _affineEffect = new AffineTransform2D(Renderer.Context2D);
            _compositeEffect = new Composite(Renderer.Context2D);

            _shadowEffect.BlurStandardDeviation = _blurSize;
            _affineEffect.TransformMatrix = _transform;

            Colour = Colour.Black;
        }

        public Vector2 Offset
        {
            get { return (Vector2)_transform.TranslationVector; }
            set
            {
                _transform.TranslationVector = (SharpDX.Vector2)value;
                //_affineEffect.TransformMatrix = _transform;
            }
        }

        public float WideningRadius
        {
            get { return _transform.ScaleVector.X; }
            set
            {
                _transform.ScaleVector = new SharpDX.Vector2(value, value);
                //_affineEffect.TransformMatrix = _transform;
            }
        }

        internal Colour Colour
        {
            get { return _colour; }
            set
            {
                _colour = value;
                UpdateColour();
            }
        }

        internal float Opacity
        {
            get { return _shadowOpacity; }
            set
            {
                _shadowOpacity = value;
                UpdateColour();
            }
        }

        internal float BlurSize
        {
            get { return _blurSize; }
            set
            {
                _blurSize = value;
                _shadowEffect.BlurStandardDeviation = value;
            }
        }

        public void Dispose()
        {
            _shadowEffect.Dispose();
            _affineEffect.Dispose();
            _compositeEffect.Dispose();
        }

        private void UpdateColour()
        {
            _shadowEffect.Color = new Color4(_colour.r, _colour.g, _colour.b, _shadowOpacity);
        }

        public void SetInput(Image image)
        {
            if (image == _inputImage)
                return;

            _inputImage = image;

            _shadowEffect.SetInput(0, image, true);
            _affineEffect.SetInputEffect(0, _shadowEffect);
            _compositeEffect.SetInputEffect(0, _affineEffect);
            _compositeEffect.SetInput(1, image, true);
        }
    }
}