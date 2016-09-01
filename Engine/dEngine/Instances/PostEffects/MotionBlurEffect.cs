// MotionBlurEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Applies motion blur to fast-moving objects.
    /// </summary>
    [TypeId(178)]
    [ExplorerOrder(0)]
    public sealed class MotionBlurEffect : PostEffect
    {
        /// <summary />
        public MotionBlurEffect()
        {
            _effectOrder = (int)EffectPrority.MotionBlur;
        }

        internal override void Render(ref DeviceContext context)
        {
            throw new NotImplementedException();
        }
    }
}