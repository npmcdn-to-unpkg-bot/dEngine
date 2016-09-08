// Workspace.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using dEngine.Graphics;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;
using dEngine.Utility;
using Neo.IronLua;

namespace dEngine.Services
{
    /// <summary>
    /// The Workspace is a top-level object representing the game level.
    /// </summary>
    /// <remarks>
    /// Objects need to be parented to the Workspace to physically intearact with the world.
    /// It will also execute all scripts that are parented to it when the game is running.
    /// </remarks>
    [TypeId(3)]
    [ExplorerOrder(1)]
    [ToolboxGroup("Containers")]
    public sealed class Workspace : Service, IWorld
    {
        private Camera _currentCamera;
        private string _placeId = "";
        private float _voidHeight;

        internal Workspace()
        {
            VoidHeight = -500;

            PlaceLoaded = new Signal<string>(this);
            CameraChanged = new Signal<Camera>(this);

            CurrentCamera = MakeDefaultCamera();
            Physics = new PhysicsSimulation();
            RenderObjectProvider = new WorldRenderer();
            Gravity = new Vector3(0, -196.2f, 0);

            Terrain = new Terrain(this);

            Game.AddWorld(this);
        }

        /// <summary>
        /// The gravity of the scene.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Vector3 Gravity
        {
            get { return Physics.Gravity; }
            set { Physics.Gravity = value; }
        }

        /// <summary>
        /// The height at which parts are destroyed when they fall beneath.
        /// </summary>
        [InstMember(3)]
        [EditorVisible]
        public float VoidHeight
        {
            get { return _voidHeight; }
            set
            {
                _voidHeight = Mathf.Clamp(value, -50000, 50000);
                NotifyChanged(nameof(Gravity));
            }
        }

        /// <summary>
        /// The name of the currently loaded place.
        /// </summary>
        [EditorVisible]
        [InstMember(4)]
        public string PlaceId
        {
            get { return _placeId; }
            internal set
            {
                if (value == _placeId) return;
                _placeId = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The amount of time in seconds that the simulation
        /// has been running.
        /// </summary>
        [EditorVisible]
        public double DistributedGameTime => RunService.SimulationStopwatch.Elapsed.TotalSeconds;

        /// <summary>
        /// If true, the place has been loaded.
        /// </summary>
        public bool IsLoaded { get; internal set; }


        /// <remarks>
        /// If CurrentCamera is set to null, or the camera's ancestral <see cref="IWorld" /> is no longer Workspace, a new camera
        /// is created.
        /// </remarks>
        [InstMember(1)]
        [EditorVisible]
        public Camera CurrentCamera
        {
            get { return _currentCamera; }
            set
            {
                var lastCamera = _currentCamera;

                if (ReferenceEquals(lastCamera, value))
                    return;

                if (lastCamera != null)
                {
                    lock (Renderer.Locker)
                    {
                        // remove unnecessary resources
                        lastCamera.DisposeResources();
                    }

                    lastCamera.ParentChanged.Event -= OnCameraReparented;
                    if (ReferenceEquals(lastCamera.World, this))
                        lastCamera.Destroy();
                }

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (value != null)
                    if (!ReferenceEquals(value.World, this) && !_deserializing)
                        return;

                _currentCamera = value;
                Game.FocusedCamera = value;

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (value != null)
                {
                    value.RenderHandle = Engine.Handle;
                    Engine.Control?.BeginInvoke(
                        new Action(
                            () => { value.ViewportSize = new Vector2(Engine.Control.Width, Engine.Control.Height); }));
                    value.ParentChanged.Event += OnCameraReparented;
                }

                CameraChanged?.Fire(value);
                NotifyChanged(nameof(CurrentCamera));
            }
        }

        /// <summary />
        protected override bool OnParentFilter(Instance newParent)
        {
            return newParent is DataModel;
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<Workspace>();
        }

        private void OnCameraReparented(Instance parent)
        {
            if (ReferenceEquals(parent, this) || ReferenceEquals(parent?.World, this) || Engine.IsExiting)
                return;
            CurrentCamera = MakeDefaultCamera();
        }

        private Camera MakeDefaultCamera()
        {
            var camera = new Camera
            {
                Name = "Camera",
                Parent = this,
                CFrame = new CFrame(0, 20, 20, 1, 0, -0, -0, 0.780868828f, 0.624695063f, 0, -0.624695063f, 0.780868828f)
            };
            return camera;
        }

        internal override void AfterDeserialization(Inst.Context context)
        {
            base.AfterDeserialization(context);
            SetGame(Game.DataModel);
        }

        internal void SetGame(DataModel game)
        {
            if (game != null)
                ParentLocked = true;
        }

        internal override void BeforeDeserialization()
        {
            base.BeforeDeserialization();
            Terrain.Clear();
        }

        /// <summary>
        /// Loads a place file in the Roblox XML format.
        /// </summary>
        [Obsolete("For testing purposes")]
        public void LoadRbxlx(string contentId)
        {
            var stream = ContentProvider.DownloadStream(new Uri(contentId)).Result;
            if (stream == null)
                throw new InvalidOperationException("Could not load RBXLX: could not download file.");
            Rbxlx.Load(Game.Workspace, stream);
        }

        /// <summary>
        /// Loads the given level file. If called by the server, instructs connected clients to load map.
        /// </summary>
        /// <param name="placeName">The placeName of the level to load. If empty, unloads the place.</param>
        public void LoadPlace(string placeName)
        {
            if (placeName == _placeId)
                return;

            lock (Game.Workspace.Locker)
            {
                placeName = placeName ?? string.Empty;

                PlaceId = "";
                IsLoaded = false;

                if (string.IsNullOrWhiteSpace(placeName)) // empty string = unload
                    return;

                Logger.Trace($"Loading place. ({placeName})");
                var placeId = $"place://{placeName}";
                var stream = ContentProvider.DownloadStream(new Uri(placeId)).Result;

                if (stream == null)
                    throw new InvalidDataException($"Failed to load place \"{placeId}\": could not fetch.");
                try
                {
                    Inst.Deserialize(stream, this);
                }
                catch (Exception e)
                {
                    Logger.Error($"A serialization error occured while loading a place. ({placeName})");
                    ClearChildren();
                    PlaceId = "";
                    return;
                }
            }

            Logger.Trace($"Loaded place. ({placeName})");
            PlaceLoaded.Fire(placeName);
            IsLoaded = true;
        }

        /// <summary>
        /// Fired when <see cref="CurrentCamera" /> changes.
        /// </summary>
        public readonly Signal<Camera> CameraChanged;

        /// <summary>
        /// Fired when a place is loaded.
        /// </summary>
        /// <seealso cref="LoadPlace" />
        public readonly Signal<string> PlaceLoaded;

#region IWorld

        /// <inheritdoc />
        public PhysicsSimulation Physics { get; }

        /// <inheritdoc />
        public WorldRenderer RenderObjectProvider { get; }

        /// <summary>
        /// The terrain object.
        /// </summary>
        public Terrain Terrain { get; set; }

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

#endregion
    }
}