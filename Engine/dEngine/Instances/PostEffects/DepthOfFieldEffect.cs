// DepthOfFieldEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Blurs parts of the scene outside a specified range.
    /// </summary>
    [TypeId(175)]
    [ExplorerOrder(0)]
    public sealed class DepthOfField : PostEffect
    {
        /// <summary />
        public DepthOfField()
        {
            _effectOrder = (int)EffectPrority.DepthOfField;
        }

        internal override void Render(ref DeviceContext context)
        {
            throw new NotImplementedException();
        }
    }
}