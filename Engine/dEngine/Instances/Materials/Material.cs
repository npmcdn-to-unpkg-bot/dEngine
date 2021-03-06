﻿// Material.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics;
using dEngine.Instances.Attributes;
using dEngine.Instances.Materials.Nodes;

#pragma warning disable 1591

namespace dEngine.Instances.Materials
{
    /// <summary>
    /// </summary>
    [TypeId(35)]
    public class Material : MaterialBase
    {
        /// <summary />
        public Material()
        {
            Nodes = new MaterialNodeCollection();
            FinalNode = new FinalNode();
        }

        internal GfxShader Shader { get; set; }

        /// <summary>
        /// The material domain.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public override MaterialDomain Domain { get; set; }

        /// <summary>
        /// The shading model.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public override ShadingModel ShadingModel { get; set; }

        /// <summary>
        /// The nodes that this material contains.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        internal MaterialNodeCollection Nodes { get; set; }

        internal FinalNode FinalNode { get; }
        public static Material Smooth { get; private set; }
        public static Material Neon { get; private set; }
        public static Material Plastic { get; private set; }
        public static Material Brick { get; private set; }
        public static Material Foil { get; private set; }
        public static Material Cobblestone { get; private set; }
        public static Material Concrete { get; private set; }
        public static Material DiamondPlate { get; private set; }
        public static Material Fabric { get; private set; }
        public static Material Granite { get; private set; }
        public static Material Grass { get; private set; }
        public static Material Ice { get; private set; }
        public static Material Marble { get; private set; }
        public static Material Metal { get; private set; }
        public static Material Pebble { get; private set; }
        public static Material Rust { get; private set; }
        public static Material Sand { get; private set; }
        public static Material Slate { get; private set; }
        public static Material Wood { get; private set; }
        public static Material WoodPlanks { get; private set; }

        /// <summary>
        /// Adds the node to this material.
        /// </summary>
        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        /// <summary>
        /// Removes the node from this material.
        /// </summary>
        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
        }

        /// <summary>
        /// Returns a <see cref="MaterialInstance" /> of this material.
        /// </summary>
        public MaterialInstance CreateInstance()
        {
            return new MaterialInstance(this);
        }
    }
}