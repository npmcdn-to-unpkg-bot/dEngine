// IRenderable.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics;
using dEngine.Graphics.Structs;

namespace dEngine.Instances
{
    /// <summary>
    /// An instance which can be rendered.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// The <see cref="dEngine.Graphics.RenderObject" /> this instance is a part of. This property is set by the
        /// <see cref="dEngine.Graphics.RenderObject.Add" /> method.
        /// </summary>
        RenderObject RenderObject { get; set; }

        /// <summary>
        /// The index of this instance in the RenderObject's instance array. This property is set by the
        /// <see cref="dEngine.Graphics.RenderObject.Add" /> method.
        /// </summary>
        int RenderIndex { get; set; }

        /// <summary>
        /// The render data.
        /// </summary>
        InstanceRenderData RenderData { get; set; }

        /// <summary>
        /// Update the <see cref="RenderData" /> struct and write the data to the <see cref="RenderObject" />'s CPU buffer.
        /// </summary>
        void UpdateRenderData();
    }
}