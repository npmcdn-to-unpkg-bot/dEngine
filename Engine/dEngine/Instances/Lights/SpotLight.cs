﻿// SpotLight.cs - dEngine
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU Lesser General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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