// MaterialBase.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

#pragma warning disable 1591

namespace dEngine.Instances.Materials
{
    [TypeId(219)]
    public abstract class MaterialBase : Instance
    {
        public abstract MaterialDomain Domain { get; set; }
        public abstract ShadingModel ShadingModel { get; set; }
    }
}