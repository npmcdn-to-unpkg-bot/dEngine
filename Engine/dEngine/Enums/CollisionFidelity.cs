// CollisionFidelity.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Instances;

namespace dEngine
{
    /// <summary>
    /// Determines how the collision models of a <see cref="PartOperation" /> behave.
    /// </summary>
    public enum CollisionFidelity
    {
        /// <summary>
        /// Uses a convex hull for collision.
        /// </summary>
        ConvexHull,

        /// <summary>
        /// Uses a bounding box for collision.
        /// </summary>
        BoundingBox
    }
}