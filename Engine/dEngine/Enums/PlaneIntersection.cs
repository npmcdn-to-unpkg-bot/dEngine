// PlaneIntersection.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEngine
{
    /// <summary>
    /// Describes the result of a plane intersection.
    /// </summary>
    public enum PlaneIntersection
    {
        /// <summary>
        /// The object is behind the plane.
        /// </summary>
        Back,

        /// <summary>
        /// The object is in front of the plane.
        /// </summary>
        Front,

        /// <summary>
        /// The object is intersecting the plane.
        /// </summary>
        Intersecting
    }
}