// LensFlareEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Lens flare.
    /// </summary>
    [TypeId(224)]
    public class LensFlareEffect : PostEffect
    {
        /// <summary />
        public LensFlareEffect()
        {
            _effectOrder = (int)EffectPrority.LensFlare;
        }

        internal override void Render(ref DeviceContext context)
        {
            throw new NotImplementedException();
        }
    }
}