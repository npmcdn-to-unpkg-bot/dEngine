// MaterialInstance.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using dEngine.Instances.Attributes;

namespace dEngine.Instances.Materials
{
    /// <summary>
    /// An instance of a <see cref="Material" /> which can be used to modify paramaters on the fly.
    /// </summary>
    [Uncreatable]
    [TypeId(42)]
    public class MaterialInstance : MaterialBase
    {
        private readonly Dictionary<string, object> _parameters;

        internal MaterialInstance(Material material)
        {
            Material = material;
            _parameters = new Dictionary<string, object>();
        }
        
        /// <summary>
        /// The base material.
        /// </summary>
        [InstMember(1)]
        public MaterialBase Material { get; internal set; }

        /// <summary>
        /// The <see cref="MaterialDomain" /> of the parent material.
        /// </summary>
        public override MaterialDomain Domain
        {
            get { return Material.Domain; }
            set { throw new InvalidOperationException("The domain of a material instance cannot be set."); }
        }

        /// <summary>
        /// The <see cref="dEngine.ShadingModel" /> of the parent material.
        /// </summary>
        public override ShadingModel ShadingModel
        {
            get { return Material.ShadingModel; }
            set { throw new InvalidOperationException("The shading model of a material instance cannot be set."); }
        }

        /// <summary>
        /// Sets the value of a material paramater.
        /// </summary>
        public void SetParameter(string name, object value)
        {
            _parameters[name] = value;
        }
    }
}