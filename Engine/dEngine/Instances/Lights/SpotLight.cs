// SpotLight.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using dEngine.Services;

namespace dEngine.Instances
{
    /// <summary>
    /// A spot light.
    /// </summary>
    /// <seealso cref="PointLight" />
    [TypeId(167)]
    [ToolboxGroup("Lights")]
    [ExplorerOrder(3)]
    public sealed class SpotLight : Light
    {
        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            Lighting.RemoveSpotLight(this);
        }

        internal override void UpdateLightData()
        {
            throw new NotImplementedException();
        }
    }
}