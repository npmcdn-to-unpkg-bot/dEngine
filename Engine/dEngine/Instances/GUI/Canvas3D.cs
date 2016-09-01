// Canvas3D.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Graphics;
using dEngine.Instances.Attributes;
using dEngine.Utility;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// A gui element which renders a 3D scene.
    /// </summary>
    [TypeId(146)]
    [ToolboxGroup("GUI")]
    public class Canvas3D : GuiElement, IWorld
    {
        private readonly Camera _camera;

        /// <inheritdoc />
        public Canvas3D()
        {
            RenderObjectProvider = new WorldRenderer();
            Game.AddWorld(this);
            _camera = new Camera();
        }

        /// <summary />
        public bool IsLoaded => true;

        /// <inheritdoc />
        [InstMember(1)] // _camera is not a child of the canvas, so serialize as a value
        public Camera CurrentCamera
        {
            get { return _camera; }
            set { throw new InvalidOperationException("CurrentCamera of Canvas3D cannot be set."); }
        }

        /// <inheritdoc />
        public PhysicsSimulation Physics { get; private set; }

        /// <inheritdoc />
        public WorldRenderer RenderObjectProvider { get; set; }

        /// <inheritdoc />
        public bool IsRenderable => true;

        /// <inheritdoc />
        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, float maxLength = 1000)
        {
            return Physics.FindPartOnRay(ray, maxLength).ToTuple();
        }

        /// <inheritdoc />
        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, LuaTable filterTable, float maxLength = 1000)
        {
            return Physics.FindPartOnRay(ray, filterTable.ToHashSet<Instance>(), maxLength).ToTuple();
        }

        /// <inheritdoc />
        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, Func<dynamic, dynamic> filterFunc,
            float maxLength = 1000)
        {
            return Physics.FindPartOnRay(ray, part => filterFunc.Invoke(part), maxLength).ToTuple();
        }

        /// <inheritdoc />
        public override void Arrange()
        {
            if (_camera != null)
                _camera.ViewportSize = AbsoluteSize;
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            _camera.Destroy();
            Game.RemoveWorld(this);
        }
    }
}