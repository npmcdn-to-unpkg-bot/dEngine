// NegateOperation.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A part representing a CSG Negate operation - a subtraction of one object from another.
    /// </summary>
    [TypeId(143)]
    [Uncreatable]
    [ExplorerOrder(9)]
    public sealed class NegateOperation : PartOperation
    {
        /// <inheritdoc />
        public NegateOperation()
        {
            Renderer.TransparentParts.Add(this);
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            Renderer.TransparentParts.Remove(this);
        }
    }
}