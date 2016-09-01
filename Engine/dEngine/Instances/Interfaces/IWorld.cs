// IWorld.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Graphics;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// Interface for worlds/environments.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// The <see cref="Camera" /> that renders this world.
        /// </summary>
        Camera CurrentCamera { get; set; }

        /// <summary>
        /// The <see cref="PhysicsSimulation" />.
        /// </summary>
        PhysicsSimulation Physics { get; }

        /// <summary>
        /// The <see cref="dEngine.Graphics.WorldRenderer" /> for this world.
        /// </summary>
        WorldRenderer RenderObjectProvider { get; }

        /// <summary>
        /// Determines if this world supports rendering. (Requires a <see cref="RenderObjectProvider" />)
        /// </summary>
        bool IsRenderable { get; }

        /// <summary>
        /// Determines if the world is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Performs a raycast for the given ray.
        /// </summary>
        LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, float maxLength = 1000);

        /// <summary>
        /// Performs a raycast for the given ray, filtering out blacklisted results.
        /// </summary>
        LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, LuaTable filterTable, float maxLength = 1000);

        /// <summary>
        /// Performs a raycast for the given ray, filtering based on a predicate.
        /// </summary>
        LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, Func<dynamic, dynamic> filterFunc,
            float maxLength = 1000);
    }
}