// GuiBase2D.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// The base class of all gui screens and elements.
    /// </summary>
    [TypeId(95)]
    [ToolboxGroup("GUI")]
    public abstract class GuiBase2D : GuiBase
    {
        /// <summary/>
        protected internal Vector2 _bbMax;
        /// <summary/>
        protected internal Vector2 _bbMin;

        /// <summary>
        /// Gets the absolute Position of the gui object.
        /// </summary>
        public abstract Vector2 AbsolutePosition { get; internal set; }

        /// <summary>
        /// Gets the absolute size of the gui object.
        /// </summary>
        public abstract Vector2 AbsoluteSize { get; internal set; }

        /// <summary>
        /// Returns true if the given point is within the bounds of this element,
        /// </summary>
        public bool BoundsCheck(Vector2 pos)
        {
            return (pos.x > AbsolutePosition.x) &&
                   (pos.y > AbsolutePosition.y) &&
                   (pos.x < AbsolutePosition.x + AbsoluteSize.x) &&
                   (pos.y < AbsolutePosition.y + AbsoluteSize.y);
        }

        internal abstract bool CheckCanDraw();
    }
}