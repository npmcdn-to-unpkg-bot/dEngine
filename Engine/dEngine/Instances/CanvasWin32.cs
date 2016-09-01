// CanvasWin32.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Graphics;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// Canvas for rendering to a form.
    /// </summary>
    internal class CanvasWin32 : Instance, IWorld
    {
        private readonly Camera _camera;

        public CanvasWin32(IntPtr handle)
        {
            _camera = new Camera {RenderHandle = handle, CFrame = new CFrame(0, 0, 10)};
            RenderObjectProvider = new WorldRenderer();
            Game.AddWorld(this);
        }

        public Camera CurrentCamera
        {
            get { return _camera; }
            set { }
        }

        public PhysicsSimulation Physics => null;
        public WorldRenderer RenderObjectProvider { get; }
        public bool IsRenderable => true;
        public bool IsLoaded => true;

        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, float maxLength = 1000)
        {
            throw new NotImplementedException();
        }

        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, LuaTable filterTable, float maxLength = 1000)
        {
            throw new NotImplementedException();
        }

        public LuaTuple<Part, Vector3, Vector3> FindPartOnRay(Ray ray, Func<dynamic, dynamic> filterFunc,
            float maxLength = 1000)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _camera.Destroy();
        }
    }
}