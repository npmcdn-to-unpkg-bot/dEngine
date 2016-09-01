// VignetteEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Darkens the corners of the screen.
    /// </summary>
    [TypeId(156)]
    public class VignetteEffect : PostEffect
    {
        private float _intensity;

        /// <summary>
        /// Determines how dark the corners get.
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

        internal override void Render(ref DeviceContext context)
        {
            //throw new System.NotImplementedException();
        }
    }
}