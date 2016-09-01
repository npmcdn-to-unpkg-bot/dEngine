// ReflectionEffect.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using SharpDX.Direct3D11;

namespace dEngine.Instances
{
    /// <summary>
    /// Screen space reflection effect.
    /// </summary>
    [TypeId(179)]
    [ExplorerOrder(0)]
    public sealed class ReflectionEffect : PostEffect
    {
        internal override void Render(ref DeviceContext context)
        {
            throw new NotImplementedException();
        }
    }
}